using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ToolGood.Bedrock.DataCommon.JsonDiffer
{
    public class JsonDifferentiator
    {
        public OutputMode OutputMode { get; private set; }
        public bool ShowOriginalValues { get; private set; }

        public JsonDifferentiator(OutputMode outputMode, bool showOriginalValues)
        {
            this.OutputMode = outputMode;
            this.ShowOriginalValues = showOriginalValues;
        }

        private static TargetNode PointTargetNode(JToken diff, string property, ChangeMode mode, OutputMode outMode)
        {
            string symbol = string.Empty;

            switch (mode) {
                case ChangeMode.Changed:
                    symbol = outMode == OutputMode.Symbol ? $"*{property}" : "changed";
                    break;

                case ChangeMode.Added:
                    symbol = outMode == OutputMode.Symbol ? $"+{property}" : "added";
                    break;

                case ChangeMode.Removed:
                    symbol = outMode == OutputMode.Symbol ? $"-{property}" : "removed";
                    break;

                case ChangeMode.Id:
                    symbol = outMode == OutputMode.Symbol ? $"${property}" : "id";
                    break;
            }

            if (outMode == OutputMode.Detailed && diff[symbol] == null) {
                diff[symbol] = JToken.Parse("{}");
            }

            return new TargetNode(symbol, (outMode == OutputMode.Symbol) ? null : property);

        }

        public static JToken Differentiate(JToken newValue, JToken oldValue, OutputMode outputMode = OutputMode.Symbol, bool showOriginalValues = false)
        {
            if (JToken.DeepEquals(newValue, oldValue)) return null;

            if (newValue != null && oldValue != null && newValue?.GetType() != oldValue?.GetType())
                throw new InvalidOperationException($"Operands' types must match; '{newValue.GetType().Name}' <> '{oldValue.GetType().Name}'");

            var propertyNames = (newValue?.Children() ?? default).Union(oldValue?.Children() ?? default)?.Select(_ => (_ as JProperty)?.Name)?.Distinct();

            if (!propertyNames.Any() && (newValue is JValue || oldValue is JValue)) {
                return (newValue == null) ? oldValue : newValue;
            }

            var difference = JToken.Parse("{}");

            foreach (var property in propertyNames) {
                if (property == null) {
                    if (newValue == null) {
                        difference = oldValue;
                    }
                    // array of object?
                    else if (newValue is JArray && newValue.Children().All(c => !(c is JValue))) {
                        var difrences = new JArray();
                        var maximum = Math.Max(newValue?.Count() ?? 0, oldValue?.Count() ?? 0);

                        for (int i = 0; i < maximum; i++) {
                            var firstsItem = newValue?.ElementAtOrDefault(i);
                            var secondsItem = oldValue?.ElementAtOrDefault(i);

                            var diff = Differentiate(firstsItem, secondsItem, outputMode, showOriginalValues);


                            if (diff != null) {
                                if (firstsItem["id"] != null) {
                                    if (diff["*id"] != null || diff["+id"] != null || diff["-id"] != null) {
                                    } else {
                                        var diff2 = new JObject();
                                        diff2["#id"] = firstsItem["id"];
                                        foreach (var (k, v) in (JObject)diff) {
                                            diff2[k] = v;
                                        }
                                        diff = diff2;
                                    }
                                }
                                difrences.Add(diff);
                            }
                        }

                        if (difrences.HasValues) {
                            difference = difrences;
                        }
                    } else {
                        difference = newValue;
                    }

                    continue;
                }

                if (newValue?[property] == null) {
                    var secondVal = oldValue?[property]?.Parent as JProperty;

                    var targetNode = PointTargetNode(difference, property, ChangeMode.Added, outputMode);

                    if (targetNode.Property != null) {
                        difference[targetNode.Symbol][targetNode.Property] = secondVal.Value;
                    } else
                        difference[targetNode.Symbol] = secondVal.Value;

                    continue;
                }

                if (oldValue?[property] == null) {
                    var firstVal = newValue?[property]?.Parent as JProperty;

                    var targetNode = PointTargetNode(difference, property, ChangeMode.Removed, outputMode);

                    if (targetNode.Property != null) {
                        difference[targetNode.Symbol][targetNode.Property] = firstVal.Value;
                    } else
                        difference[targetNode.Symbol] = firstVal.Value;

                    continue;
                }

                if (newValue?[property] is JValue value) {
                    if (!JToken.DeepEquals(newValue?[property], oldValue?[property])) {
                        var targetNode = PointTargetNode(difference, property, ChangeMode.Changed, outputMode);

                        if (targetNode.Property != null) {
                            difference[targetNode.Symbol][targetNode.Property] = showOriginalValues ? oldValue?[property] : value;
                        } else
                            difference[targetNode.Symbol] = showOriginalValues ? oldValue?[property] : value;
                        //difference["changed"][property] = showOriginalValues ? second?[property] : value;
                    }

                    continue;
                }

                if (newValue?[property] is JObject) {

                    var targetNode = oldValue?[property] == null
                        ? PointTargetNode(difference, property, ChangeMode.Removed, outputMode)
                        : PointTargetNode(difference, property, ChangeMode.Changed, outputMode);

                    var firstsItem = newValue[property];
                    var secondsItem = oldValue[property];

                    var diffrence = Differentiate(firstsItem, secondsItem, outputMode, showOriginalValues);

                    if (diffrence != null) {

                        if (targetNode.Property != null) {
                            difference[targetNode.Symbol][targetNode.Property] = diffrence;
                        } else
                            difference[targetNode.Symbol] = diffrence;

                    }

                    continue;
                }

                if (newValue?[property] is JArray) {
                    var difrences = new JArray();

                    var targetNode = oldValue?[property] == null
                       ? PointTargetNode(difference, property, ChangeMode.Removed, outputMode)
                       : PointTargetNode(difference, property, ChangeMode.Changed, outputMode);

                    var maximum = Math.Max(newValue?[property]?.Count() ?? 0, oldValue?[property]?.Count() ?? 0);

                    for (int i = 0; i < maximum; i++) {
                        var firstsItem = newValue[property]?.ElementAtOrDefault(i);
                        var secondsItem = oldValue[property]?.ElementAtOrDefault(i);

                        var diff = Differentiate(firstsItem, secondsItem, outputMode, showOriginalValues);

                        if (diff != null) {
                            difrences.Add(diff);
                        }
                    }

                    if (difrences.HasValues) {
                        if (targetNode.Property != null) {
                            difference[targetNode.Symbol][targetNode.Property] = difrences;
                        } else
                            difference[targetNode.Symbol] = difrences;
                    }

                    continue;
                }
            }

            return difference;
        }

        public JToken Differentiate(JToken first, JToken second)
        {
            return Differentiate(first, second, this.OutputMode, this.ShowOriginalValues);
        }
    }
}
