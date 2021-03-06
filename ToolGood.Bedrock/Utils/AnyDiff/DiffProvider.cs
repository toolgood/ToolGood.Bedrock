﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using TypeSupport;
using TypeSupport.Extensions;

namespace ToolGood.AnyDiff
{
    /// <summary>
    /// Compare two objects for value differences
    /// </summary>
    public class DiffProvider
    {
        /// <summary>
        /// The default max depth to use
        /// </summary>
        internal const int DefaultMaxDepth = 32;
        /// <summary>
        /// The default type support options to use
        /// </summary>
        public static TypeSupportOptions DefaultTypeSupportOptions = TypeSupportOptions.Collections | TypeSupportOptions.Attributes | TypeSupportOptions.Caching | TypeSupportOptions.Methods;

        /// <summary>
        /// The list of attributes to use when ignoring fields/properties
        /// </summary>
        private readonly List<object> _ignoreAttributes = new List<object> {
            typeof(IgnoreDataMemberAttribute),
            typeof(NonSerializedAttribute),
            "JsonIgnoreAttribute",
        };

        /// <summary>
        /// Compare two objects for value differences
        /// </summary>
        /// <param name="left">Object A</param>
        /// <param name="right">Object B</param>
        /// <returns></returns>
        public List<Difference> ComputeDiff(object left, object right)
        {
            return ComputeDiff(left, right, DefaultMaxDepth, ComparisonOptions.All);
        }

        /// <summary>
        /// Compare two objects for value differences
        /// </summary>
        /// <param name="left">Object A</param>
        /// <param name="right">Object B</param>
        /// <param name="options">Specify the comparison options</param>
        /// <returns></returns>
        public List<Difference> ComputeDiff(object left, object right, ComparisonOptions options)
        {
            return ComputeDiff(left, right, DefaultMaxDepth, options);
        }

        /// <summary>
        /// Compare two objects for value differences
        /// </summary>
        /// <param name="left">Object A</param>
        /// <param name="right">Object B</param>
        /// <param name="options">Specify the comparison options</param>
        /// <param name="propertyList">A list of property names or full path names to ignore</param>
        /// <returns></returns>
        public List<Difference> ComputeDiff(object left, object right, ComparisonOptions options, params string[] propertyList)
        {
            return ComputeDiff(left, right, DefaultMaxDepth, options, propertyList);
        }

        /// <summary>
        /// Compare two objects for value differences
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public List<Difference> ComputeDiff<T>(T left, T right)
        {
            return ComputeDiff(left, right, DefaultMaxDepth, ComparisonOptions.All);
        }

        /// <summary>
        /// Compare two objects for value differences
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="options">Specify the comparison options</param>
        /// <param name="propertyList"></param>
        /// <returns></returns>
        public List<Difference> ComputeDiff<T>(T left, T right, ComparisonOptions options, params Expression<Func<T, object>>[] propertyList)
        {
            return ComputeDiff(left, right, DefaultMaxDepth, options, propertyList);
        }

        /// <summary>
        /// Compare two objects for value differences
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="maxDepth"></param>
        /// <param name="options">Specify the comparison options</param>
        /// <param name="propertyList">A list of property names or full path names to ignore</param>
        /// <returns></returns>
        public List<Difference> ComputeDiff(object left, object right, int maxDepth, ComparisonOptions options, params string[] propertyList)
        {
            return RecurseProperties(left, right, null, new List<Difference>(), 0, maxDepth, new ObjectHashcodeMap(), string.Empty, options, propertyList.ToList());
        }

        /// <summary>
        /// Compare two objects for value differences
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left">Object A</param>
        /// <param name="right">Object B</param>
        /// <param name="maxDepth">Maximum recursion depth</param>
        /// <param name="options">Specify the comparison options</param>
        /// <returns></returns>
        public List<Difference> ComputeDiff<T>(T left, T right, int maxDepth, ComparisonOptions options, params Expression<Func<T, object>>[] propertyList)
        {
            var ignorePropertiesList = new List<string>();
            var expressionManager = new ExpressionManager();
            if (propertyList != null) {
                foreach (var expression in propertyList) {
                    var name = expressionManager.GetPropertyPath(expression.Body);
                    ignorePropertiesList.Add(name);
                }
            }
            return RecurseProperties(left, right, null, new List<Difference>(), 0, maxDepth, new ObjectHashcodeMap(), string.Empty, options, ignorePropertiesList);
        }

        /// <summary>
        /// Recurse the object's tree
        /// </summary>
        /// <param name="left">The left object to compare</param>
        /// <param name="right">The right object to compare</param>
        /// <param name="parent">The parent object</param>
        /// <param name="differences">A list of differences currently found in the tree</param>
        /// <param name="currentDepth">The current depth of the tree recursion</param>
        /// <param name="maxDepth">The maximum number of tree children to recurse</param>
        /// <param name="objectTree">A hash table containing the tree that has already been traversed, to prevent recursion loops</param>
        /// <param name="options">Specify the comparison options</param>
        /// <param name="propertyList">A list of property names or full path names to ignore</param>
        /// <returns></returns>
        private List<Difference> RecurseProperties(object left, object right, object parent, List<Difference> differences, int currentDepth, int maxDepth, ObjectHashcodeMap objectTree, string path, ComparisonOptions options, List<string> propertyList)
        {
            if (GetPropertyInclusionState(null, path, options, propertyList, null) == FilterResult.Exclude)
                return differences;

            if (!options.BitwiseHasFlag(ComparisonOptions.AllowCompareDifferentObjects)
                && left != null && right != null
                && left?.GetType() != right?.GetType())
                throw new ArgumentException("Objects Left and Right must be of the same type.");

            if (left == null && right == null)
                return differences;

            if (maxDepth > 0 && currentDepth >= maxDepth)
                return differences;

            var typeSupport = new ExtendedType(left != null ? left.GetType() : right.GetType(), DefaultTypeSupportOptions);
            if (typeSupport.Attributes.Any(x => _ignoreAttributes.Contains(x)))
                return differences;
            if (typeSupport.IsDelegate)
                return differences;

            if (options.BitwiseHasFlag(ComparisonOptions.AllowEqualsOverride)) {
                var objectEquality = CompareForObjectEquality(typeSupport, left, right);
                if (objectEquality)
                    return differences; // no differences found, no need to continue
            }

            // increment the current recursion depth
            currentDepth++;

            // construct a hashtable of objects we have already inspected (simple recursion loop preventer)
            // we use this hashcode method as it does not use any custom hashcode handlers the object might implement
            if (left != null) {
                if (objectTree.Contains(left))
                    return differences;
                objectTree.Add(left);
            }

            // get list of properties
            var properties = new List<ExtendedProperty>();
            if (options.BitwiseHasFlag(ComparisonOptions.CompareProperties)) {
                var pis = left.GetProperties(PropertyOptions.All);
                foreach (var pi in pis) {
                    if (pi.PropertyInfo.CanWrite == false || pi.PropertyInfo.CanRead == false) { continue; }

                    var atts = pi.PropertyInfo.GetCustomAttributes(true);
                    var canAdd = true;
                    foreach (var att in atts) {
                        var name = att.GetType().Name.ToLower();
                        if (name.Contains("ignore") || name.Contains("result")) {
                            canAdd = false;
                            break;
                        }
                    }
                    if (canAdd) { properties.Add(pi); }
                }
            }

            // get all fields, except for backed auto-property fields
            var fields = new List<ExtendedField>();
            if (options.BitwiseHasFlag(ComparisonOptions.CompareFields)) {
                var fis = left.GetFields(FieldOptions.All);
                foreach (var fi in fis) {
                    if (fi.IsBackingField) { continue; }

                    var atts = fi.FieldInfo.GetCustomAttributes(true);
                    var canAdd = true;
                    foreach (var att in atts) {
                        var name = att.GetType().Name.ToLower();
                        if (name.Contains("ignore") || name.Contains("result") || name.Contains("nonserialized")) {
                            canAdd = false;
                            break;
                        }
                    }
                    if (canAdd) { fields.Add(fi); }
                }
            }

            var rootPath = path;
            var localPath = string.Empty;
            foreach (var property in properties) {
                localPath = $"{rootPath}.{property.Name}";
                if (GetPropertyInclusionState(property.Name, localPath, options, propertyList, property.CustomAttributes) == FilterResult.Exclude)
                    continue;
                object leftValue = null;
                try {
                    if (left != null)
                        leftValue = left.GetPropertyValue(property);
                } catch (Exception) {
                    // catch any exceptions accessing the property
                }
                object rightValue = null;
                try {
                    if (right != null)
                        rightValue = right.GetPropertyValue(property);
                } catch (Exception) {
                    // catch any exceptions accessing the property
                }
                var showName = property.Name;
                var attr = property.PropertyInfo.GetCustomAttribute<DisplayNameAttribute>(true);
                if (attr != null) {
                    showName = attr.DisplayName;
                }

                differences = GetDifferences(property.Name, showName, property.Type, GetTypeConverter(property), leftValue, rightValue, parent, differences, currentDepth, maxDepth, objectTree, localPath, options, propertyList);
            }
            foreach (var field in fields) {
                localPath = $"{rootPath}.{field.Name}";
                if (GetPropertyInclusionState(field.Name, localPath, options, propertyList, field.CustomAttributes) == FilterResult.Exclude)
                    continue;
                object leftValue = null;
                if (left != null)
                    leftValue = left.GetFieldValue(field);
                object rightValue = null;
                if (right != null)
                    rightValue = right.GetFieldValue(field);
                var showName = field.Name;
                var attr = field.FieldInfo.GetCustomAttribute<DisplayNameAttribute>(true);
                if (attr != null) {
                    showName = attr.DisplayName;
                }
                differences = GetDifferences(field.Name, showName, field.Type, GetTypeConverter(field), leftValue, rightValue, parent, differences, currentDepth, maxDepth, objectTree, localPath, options, propertyList);
            }

            return differences;
        }

        /// <summary>
        /// Get the differences between two objects
        /// </summary>
        /// <param name="propertyName">The name of the property being compared</param>
        /// <param name="propertyType">The type of property being compared. The left property is assumed unless allowCompareDifferentObjects=true</param>
        /// <param name="typeConverter">An optional TypeConverter to treat the type as a different type</param>
        /// <param name="left">The left object to compare</param>
        /// <param name="right">The right object to compare</param>
        /// <param name="parent">The parent object</param>
        /// <param name="differences">A list of differences currently found in the tree</param>
        /// <param name="currentDepth">The current depth of the tree recursion</param>
        /// <param name="maxDepth">The maximum number of tree children to recurse</param>
        /// <param name="objectTree">A hash table containing the tree that has already been traversed, to prevent recursion loops</param>
        /// <param name="options">Specify the comparison options</param>
        /// <param name="propertyList">A list of property names or full path names to ignore</param>
        /// <returns></returns>
        private List<Difference> GetDifferences(string propertyName, string showName, Type propertyType, TypeConverter typeConverter, object left, object right, object parent, List<Difference> differences, int currentDepth, int maxDepth, ObjectHashcodeMap objectTree, string path, ComparisonOptions options, List<string> propertyList)
        {
            if (GetPropertyInclusionState(propertyName, path, options, propertyList, null) == FilterResult.Exclude)
                return differences;

            object leftValue = null;
            object rightValue = null;
            leftValue = left;

            if (options.BitwiseHasFlag(ComparisonOptions.AllowCompareDifferentObjects) && rightValue != null)
                rightValue = GetValueForProperty(right, propertyName);
            else
                rightValue = right;

            if (rightValue == null && leftValue != null || leftValue == null && rightValue != null) {
                differences.Add(new Difference((leftValue ?? rightValue).GetType(), propertyName, path, leftValue, rightValue, typeConverter));
                return differences;
            }

            if (leftValue == null && rightValue == null)
                return differences;

            var propertyTypeSupport = propertyType.GetExtendedType(DefaultTypeSupportOptions);
            var isCollection = propertyType != typeof(string) && propertyType.GetInterface(nameof(IEnumerable)) != null;
            if (isCollection && options.BitwiseHasFlag(ComparisonOptions.CompareCollections)) {
                var genericArguments = propertyType.GetGenericArguments();
                var isArray = propertyTypeSupport.IsArray;
                var elementType = propertyTypeSupport.ElementType;
                // iterate the collection
                var aValueCollection = (leftValue as IEnumerable);
                var bValueCollection = (rightValue as IEnumerable);
                var bValueCollectionCount = GetCountFromEnumerable(bValueCollection);
                var bValueEnumerator = bValueCollection?.GetEnumerator();
                if (aValueCollection != null) {
                    if (!options.BitwiseHasFlag(ComparisonOptions.AllowCollectionsToBeOutOfOrder)) {
                        var arrayIndex = 0;
                        // compare elements must be the same order
                        foreach (var collectionItem in aValueCollection) {
                            var hasValue = bValueEnumerator?.MoveNext() ?? false;
                            leftValue = collectionItem;
                            if (hasValue) {
                                rightValue = bValueEnumerator?.Current;
                                if (leftValue == null && rightValue == null)
                                    continue;
                                if (leftValue == null && rightValue != null)
                                    differences.Add(new Difference(rightValue?.GetType() ?? elementType, propertyName, showName, path, arrayIndex, leftValue, rightValue, typeConverter));
                                // check array element for difference
                                if (leftValue != null && !leftValue.GetType().IsValueType && leftValue.GetType() != typeof(string)) {
                                    differences = RecurseProperties(leftValue, rightValue, parent, differences, currentDepth, maxDepth, objectTree, path, options, propertyList);
                                } else if (leftValue != null && leftValue.GetType().IsGenericType && leftValue.GetType().GetGenericTypeDefinition() == typeof(KeyValuePair<,>)) {
                                    // compare keys and values of a KVP
                                    var leftKvpKey = GetValueForProperty(leftValue, "Key");
                                    var leftKvpValue = GetValueForProperty(leftValue, "Value");
                                    var rightKvpKey = GetValueForProperty(rightValue, "Key");
                                    var rightKvpValue = GetValueForProperty(rightValue, "Value");

                                    Type leftKeyType = leftKvpKey?.GetType() ?? genericArguments.First();
                                    Type leftValueType = leftKvpValue?.GetType() ?? genericArguments.Skip(1).First();

                                    // compare the key
                                    if (leftKvpKey != null && !leftKeyType.IsValueType && leftKeyType != typeof(string))
                                        differences = RecurseProperties(leftKvpKey, rightKvpKey, leftValue, differences, currentDepth, maxDepth, objectTree, path, options, propertyList);
                                    else {
                                        if (!IsMatch(leftKvpKey, rightKvpKey))
                                            differences.Add(new Difference(leftKeyType, propertyName, path, showName, arrayIndex, leftKvpKey, rightKvpKey, typeConverter));
                                    }

                                    // compare the value
                                    if (leftKvpValue != null && !leftValueType.IsValueType && leftValueType != typeof(string))
                                        differences = RecurseProperties(leftKvpValue, rightKvpValue, leftValue, differences, currentDepth, maxDepth, objectTree, path, options, propertyList);
                                    else {
                                        if (!IsMatch(leftValue, rightValue))
                                            differences.Add(new Difference(leftValueType, propertyName, path, showName, arrayIndex, leftKvpValue, rightKvpValue, typeConverter));
                                    }
                                } else {
                                    if (!IsMatch(leftValue, rightValue))
                                        differences.Add(new Difference(leftValue?.GetType() ?? elementType, propertyName, showName, path, arrayIndex, leftValue, rightValue, typeConverter));
                                }
                            } else {
                                // left has a value in collection, right does not. That's a difference
                                rightValue = null;
                                differences.Add(new Difference(leftValue?.GetType() ?? elementType, propertyName, showName, path, arrayIndex, leftValue, rightValue, typeConverter));
                            }
                            arrayIndex++;
                        }
                        if (bValueCollectionCount > arrayIndex) {
                            // right side has extra elements
                            var rightSideExtraElements = bValueCollectionCount - arrayIndex;
                            if (bValueEnumerator != null) {
                                for (var i = 0; i < rightSideExtraElements; i++) {
                                    var hasValue = bValueEnumerator?.MoveNext() ?? false;
                                    if (hasValue) {
                                        differences.Add(new Difference(aValueCollection.GetType(), propertyName, showName, path, arrayIndex, null, bValueEnumerator.Current, typeConverter));
                                        arrayIndex++;
                                    }
                                }
                            }
                        }
                    } else {
                        // compare elements can be of different order (use the hashcode for this)
                        var leftHashCodeEntryList = new Dictionary<int, CollectionKey>(); // hashcode, value/count
                        var rightHashCodeEntryList = new Dictionary<int, CollectionKey>(); // hashcode, value/count
                        var arrayIndex = 0;
                        foreach (var collectionItem in aValueCollection) {
                            var hashCode = collectionItem.GetHashCode();
                            if (leftHashCodeEntryList.ContainsKey(hashCode)) {
                                var val = leftHashCodeEntryList[hashCode];
                                val.Matches++;
                                val.OriginalIndex = arrayIndex; // populate the highest value
                            } else
                                leftHashCodeEntryList.Add(hashCode, new CollectionKey(arrayIndex, collectionItem, 1));
                            arrayIndex++;
                        }
                        arrayIndex = 0;
                        foreach (var collectionItem in bValueCollection) {
                            var hashCode = collectionItem.GetHashCode();
                            if (rightHashCodeEntryList.ContainsKey(hashCode)) {
                                var val = rightHashCodeEntryList[hashCode];
                                val.Matches++;
                                val.OriginalIndex = arrayIndex; // populate the highest value
                            } else
                                rightHashCodeEntryList.Add(hashCode, new CollectionKey(arrayIndex, collectionItem, 1));
                            arrayIndex++;
                        }
                        var orderedLeft = leftHashCodeEntryList.OrderBy(x => x.Key);
                        var orderedRight = rightHashCodeEntryList.OrderBy(x => x.Key);
                        // compare the left collection
                        foreach (var item in orderedLeft) {
                            var matchFound = orderedRight.Where(x => x.Key == item.Key).Select(x => x.Value).FirstOrDefault();
                            if (matchFound == null) {
                                differences.Add(new Difference((leftValue ?? rightValue).GetType(), propertyName, showName, path, item.Value.OriginalIndex, item.Value.Value, null, typeConverter));
                            } else {
                                var isEqual = CompareForObjectEquality(item.Value.Value.GetExtendedType(DefaultTypeSupportOptions), item.Value.Value, matchFound.Value);
                                if (!isEqual)
                                    differences.Add(new Difference((leftValue ?? rightValue).GetType(), propertyName, showName, path, item.Value.OriginalIndex, item.Value.Value, null, typeConverter));
                                else if (matchFound.Matches != item.Value.Matches && item.Value.Matches > matchFound.Matches)
                                    // also compare that the number of instances of the value in a collection are the same
                                    differences.Add(new Difference((leftValue ?? rightValue).GetType(), propertyName, showName, path, item.Value.OriginalIndex, item.Value.Value, null, typeConverter));
                            }
                        }
                        // compare the right collection
                        foreach (var item in orderedRight) {
                            var matchFound = orderedLeft.Where(x => x.Key == item.Key).Select(x => x.Value).FirstOrDefault();
                            if (matchFound == null) {
                                differences.Add(new Difference((leftValue ?? rightValue).GetType(), propertyName, showName, path, item.Value.OriginalIndex, null, item.Value.Value, typeConverter));
                            } else {
                                var isEqual = CompareForObjectEquality(item.Value.Value.GetExtendedType(DefaultTypeSupportOptions), item.Value.Value, matchFound.Value);
                                if (!isEqual)
                                    differences.Add(new Difference((leftValue ?? rightValue).GetType(), propertyName, showName, path, item.Value.OriginalIndex, null, item.Value.Value, typeConverter));
                                else if (matchFound.Matches != item.Value.Matches && item.Value.Matches > matchFound.Matches)
                                    // also compare that the number of instances of the value in a collection are the same
                                    differences.Add(new Difference((leftValue ?? rightValue).GetType(), propertyName, showName, path, item.Value.OriginalIndex, null, item.Value.Value, typeConverter));
                            }
                        }
                    }

                }
            } else if (!propertyType.IsValueType && propertyType != typeof(string)) {
                differences = RecurseProperties(leftValue, rightValue, leftValue, differences, currentDepth, maxDepth, objectTree, path, options, propertyList);
            } else {
                if (!IsMatch(leftValue, rightValue))
                    differences.Add(new Difference(propertyType, propertyName, showName, path, leftValue, rightValue, typeConverter));
            }

            return differences;
        }

        private static long GetCountFromEnumerable(IEnumerable enumerable)
        {
            if (enumerable == null)
                return 0L;
            var count = 0L;
            var enumerableType = enumerable.GetType().GetExtendedType(DefaultTypeSupportOptions);
            if (enumerableType.IsCollection)
                return ((ICollection)enumerable).Count;
            if (enumerableType.IsArray)
                return ((Array)enumerable).LongLength;

            // count the enumerable
            foreach (var row in enumerable)
                count++;

            // try to reset the enumerable, if IEnumerable implements it
            var enumerator = enumerable.GetEnumerator();
            try {
                enumerator.Reset();
            } catch (NotImplementedException) { }
            return count;
        }

        private static TypeConverter GetTypeConverter(PropertyInfo property)
        {
            return GetTypeConverter(property.GetCustomAttributes(typeof(TypeConverterAttribute), false).FirstOrDefault() as TypeConverterAttribute);
        }

        private static TypeConverter GetTypeConverter(FieldInfo property)
        {
            return GetTypeConverter(property.GetCustomAttributes(typeof(TypeConverterAttribute), false).FirstOrDefault() as TypeConverterAttribute);
        }

        private static TypeConverter GetTypeConverter(TypeConverterAttribute attribute)
        {
            if (attribute != null) {
                var typeConverter = Activator.CreateInstance(Type.GetType(attribute.ConverterTypeName)) as TypeConverter;
                return typeConverter;
            }
            return null;
        }

        /// <summary>
        /// Returns true if two objects match equality
        /// </summary>
        /// <param name="typeSupport">The extended type for the left object</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private bool CompareForObjectEquality(ExtendedType typeSupport, object left, object right)
        {
            // order of precedence: IEquatable => Equals => (==)
            // if the object implements IEquatable, use it
            var hasIEquatable = typeSupport.Implements(typeof(IEquatable<>));
            if (hasIEquatable) {
                var equatableMethod = typeSupport.Methods?.FirstOrDefault(x => x.Name == "Equals" && x.Parameters.Any(y => y.ParameterType == typeSupport.Type));
                var isEquatable = (bool)equatableMethod?.MethodInfo.Invoke(left, new object[] { right }) == true;
                if (isEquatable)
                    return true; // no differences found
            } else {
                // if the object overrides Equals(), use it
                var hasEqualsOverride = typeSupport.Methods?.Any(x => x.Name == "Equals" && x.IsOverride) == true;
                if (hasEqualsOverride) {
                    if (left.Equals(right))
                        return true; // no differences found
                } else {
                    // if the object overrides the equality operator (==), use it
                    var hasEqualityOperator = typeSupport.Methods?.Any(x => x.Name == "op_Equality" && x.IsOperatorOverload) == true;
                    if (hasEqualityOperator) {
                        var operatorMethod = typeSupport.Methods?.FirstOrDefault(x => x.Name == "op_Equality" && x.Parameters.All(y => y.ParameterType == typeSupport.Type));
                        var isEqual = (bool)operatorMethod?.MethodInfo.Invoke(left, new object[] { left, right }) == true;
                        if (isEqual)
                            return true; // no differences found
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns if the object/property should be included or excluded
        /// </summary>
        /// <param name="name">Property or field name</param>
        /// <param name="path">Full path to object</param>
        /// <param name="options">Comparison options</param>
        /// <param name="propertyList">List of names or paths to process for inclusion or exclusion</param>
        /// <param name="attributes"> </param>
        /// <returns></returns>
        private FilterResult GetPropertyInclusionState(string name, string path, ComparisonOptions options, List<string> propertyList, IEnumerable<CustomAttributeData> attributes)
        {
            var isIncludeList = options.BitwiseHasFlag(ComparisonOptions.IncludeList);
            var isExcludeList = !isIncludeList || options.BitwiseHasFlag(ComparisonOptions.ExcludeList);

            var excludeByNameOrPath = isExcludeList && (propertyList?.Contains(name) == true || propertyList?.Contains(path) == true);
            if (excludeByNameOrPath)
                return FilterResult.Exclude; // exclude the property (by being in the exclusion list)

            if (isIncludeList) {
                var noInheritance = options.BitwiseHasFlag(ComparisonOptions.IncludeListNoInheritance);
                var includeByNameOrPath = (propertyList?.Contains(name) == true || propertyList?.Contains(path) == true);
                if (!includeByNameOrPath && !noInheritance) {
                    // for inclusion lists, if the parent path is allowed then allow its children
                    var pathParts = path.Split('.');
                    var parentPaths = pathParts.Skip(1).Take(pathParts.Length - 2).Select(x => $".{x}").ToList();
                    foreach (var parentPath in parentPaths) {
                        includeByNameOrPath = propertyList.Contains(parentPath);
                        if (includeByNameOrPath)
                            break;
                    }
                }

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(path) || includeByNameOrPath)
                    return FilterResult.Include; // include the property
                return FilterResult.Exclude; // exclude the property (by not being in inclusion list)
            }
#if FEATURE_CUSTOM_ATTRIBUTES
            if (attributes?.Any(x => !options.BitwiseHasFlag(ComparisonOptions.DisableIgnoreAttributes) && (_ignoreAttributes.Contains(x.AttributeType) || _ignoreAttributes.Contains(x.AttributeType.Name))) == true)
#else
            if (attributes?.Any(x => !options.BitwiseHasFlag(ComparisonOptions.DisableIgnoreAttributes) && (_ignoreAttributes.Contains(x.Constructor.DeclaringType) || _ignoreAttributes.Contains(x.Constructor.DeclaringType.Name))) == true)
#endif
                return FilterResult.Exclude; // exclude the property (by attribute)
            return FilterResult.Include; // include the property (default)
        }

        private static object GetValueForProperty(object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            var property = obj.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName));
#if FEATURE_SETVALUE
            return property?.GetValue(obj);
#else
            return property?.GetValue(obj, null);
#endif
        }

        private static bool IsMatch(object leftValue, object rightValue)
        {
            var isMatch = false;
            // both values being null are a match
            if (ReferenceEquals(leftValue, null) && ReferenceEquals(rightValue, null)) {
                isMatch = true;
            }
            // only one value being null is not a match
            else if (ReferenceEquals(leftValue, null) || ReferenceEquals(rightValue, null)) {
                isMatch = false;
            }
            // use equality check
            else if (leftValue.Equals(rightValue)) {
                isMatch = true;
            }
            return isMatch;
        }

        private enum FilterResult
        {
            Exclude,
            Include
        }

        private class CollectionKey
        {
            public object Value { get; set; }
            public int Matches { get; set; }
            public int OriginalIndex { get; set; }
            public CollectionKey(int originalIndex, object value, int matches)
            {
                OriginalIndex = originalIndex;
                Value = value;
                Matches = matches;
            }
        }
    }
}
