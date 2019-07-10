//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;

//namespace ToolGood.Bedrock
//{
//    public sealed class AssertUtil
//    {
//        public class AssertionException : Exception
//        {
//            public AssertionException(string message) : base(message) { }
//        }
//        static class Utils
//        {
//            // Format any value for diagnostic display
//            public static string FormatValue(object value)
//            {
//                if (value == null)
//                    return "null";
//                var str = value as string;
//                if (str != null) {
//                    str = str.Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\0", "\\0");
//                    return $"\"{str}\"";
//                }
//                if (value.GetType() == typeof(int) || value.GetType() == typeof(long) || value.GetType() == typeof(bool))
//                    return value.ToString();
//                var d = value as System.Collections.IDictionary;
//                if (d != null)
//                    return string.Format("{{{0}}}", string.Join(", ", AsDictionaryEntries(d).Select(de => string.Format("{{ {0}, {1} }}", FormatValue(de.Key), FormatValue(de.Value)))));
//                var e = value as System.Collections.IEnumerable;
//                if (e != null)
//                    return string.Format("[{0}]", string.Join(", ", e.Cast<object>().Select(v => FormatValue(v))));
//                var x = value as Exception;
//                if (x != null)
//                    return string.Format("[{0}] {1}", value.GetType().FullName, x.Message);
//                return string.Format("[{0}] {1}", value.GetType().FullName, value.ToString());
//            }
//            public static IEnumerable<System.Collections.DictionaryEntry> AsDictionaryEntries(System.Collections.IDictionary dictionary)
//            {
//                foreach (var de in dictionary)
//                    yield return (System.Collections.DictionaryEntry)de;
//            }
//            public static int CountCommonPrefix(string a, string b, bool IgnoreCase)
//            {
//                int i = 0;
//                while (i < Math.Min(a.Length, b.Length) && (IgnoreCase ? (char.ToUpperInvariant(a[i]) == char.ToUpperInvariant(b[i])) : (a[i] == b[i])))
//                    i++;
//                return i;
//            }
//            public static string GetStringExtract(string str, int offset)
//            {
//                if (offset > 15)
//                    str = "..." + str.Substring(offset - 10);
//                if (str.Length > 30)
//                    str = str.Substring(0, 20) + "...";
//                return str;
//            }
//        }
//        public static void IsTrue(bool test, string throwMessage = null)
//        {
//            Throw(test, () => throwMessage ?? "Expression is not true");
//        }
//        public static void IsFalse(bool test, string throwMessage = null)
//        {
//            Throw(!test, () => throwMessage ?? "Expression is not false");
//        }

//        public static void AreSame(object a, object b, string throwMessage = null)
//        {
//            Throw(object.ReferenceEquals(a, b), () => throwMessage ?? "Object references are not the same");
//        }
//        public static void AreNotSame(object a, object b, string throwMessage = null)
//        {
//            Throw(!object.ReferenceEquals(a, b), () => throwMessage ?? "Object references are the same");
//        }

//        public static void AreEqual(object a, object b, string throwMessage = null)
//        {
//            AreEqual(a, b, () => TestEqual(a, b), throwMessage);
//        }
//        public static void AreNotEqual(object a, object b, string throwMessage = null)
//        {
//            AreNotEqual(a, b, () => TestEqual(a, b), throwMessage);
//        }
//        public static void AreEqual(double a, double b, double within, string throwMessage = null)
//        {
//            AreEqual(a, b, () => Math.Abs(a - b) < within, throwMessage);
//        }
//        public static void AreNotEqual(double a, double b, double within, string throwMessage = null)
//        {
//            AreNotEqual(a, b, () => Math.Abs(a - b) < within, throwMessage);
//        }
//        public static void AreEqual<T>(T a, T b, string throwMessage = null)
//        {
//            AreEqual(a, b, () => Object.Equals(a, b), throwMessage);
//        }
//        public static void AreNotEqual<T>(T a, T b, string throwMessage = null)
//        {
//            AreNotEqual(a, b, () => Object.Equals(a, b), throwMessage);
//        }
//        public static void AreEqual(string a, string b, bool ignoreCase, string throwMessage = null)
//        {
//            Throw(string.Compare(a, b, ignoreCase) == 0, () => {
//                var offset = Utils.CountCommonPrefix(a, b, ignoreCase);
//                var xa = Utils.FormatValue(Utils.GetStringExtract(a, offset));
//                var xb = Utils.FormatValue(Utils.GetStringExtract(b, offset));
//                return string.Format(throwMessage ?? "Strings are not equal at offset {0}  lhs: {1}  rhs: {2}{3}^", offset, xa, xb, new string(' ', Utils.CountCommonPrefix(xa, xb, ignoreCase) + 7));
//            });
//        }
//        public static void AreEqual(string a, string b, string throwMessage = null)
//        {
//            AreEqual(a, b, false, throwMessage);
//        }
//        public static void AreNotEqual(string a, string b, bool ignoreCase = false, string throwMessage = null)
//        {
//            Throw(string.Compare(a, b, ignoreCase) != 0, () => string.Format(throwMessage ?? "Strings are not equal  lhs: {0}  rhs: {1}", Utils.FormatValue(a), Utils.FormatValue(b)));
//        }

//        public static void IsEmpty(string val, string throwMessage = null)
//        {
//            Throw(val != null && val.Length == 0, () => string.Format(throwMessage ?? "String is not empty: {0}", Utils.FormatValue(val)));
//        }
//        public static void IsNotEmpty(string val, string throwMessage = null)
//        {
//            Throw(val != null && val.Length != 0, () => throwMessage ?? "String is empty");
//        }
//        public static void IsNullOrEmpty(string val, string throwMessage = null)
//        {
//            Throw(string.IsNullOrEmpty(val), () => throwMessage ?? $"String is not empty: {Utils.FormatValue(val)}");
//        }
//        public static void IsNotNullOrEmpty(string val, string throwMessage = null)
//        {
//            Throw(!string.IsNullOrEmpty(val), () => string.Format(throwMessage ?? "String is not empty: {0}", Utils.FormatValue(val)));
//        }
//        public static void IsEmpty(System.Collections.IEnumerable collection, string throwMessage = null)
//        {
//            Throw(collection != null && collection.Cast<object>().Count() == 0, () => string.Format(throwMessage ?? "Collection is not empty  Items: {0}", Utils.FormatValue(collection)));
//        }
//        public static void IsNotEmpty(System.Collections.IEnumerable collection, string throwMessage = null)
//        {
//            Throw(collection != null && collection.Cast<object>().Count() != 0, () => throwMessage ?? "Collection is empty");
//        }

//        public static void Contains(System.Collections.IEnumerable collection, object item, string throwMessage = null)
//        {
//            Throw(collection.Cast<object>().Contains(item), () => string.Format(throwMessage ?? "Collection doesn't contain {0}  Items: {1}", Utils.FormatValue(item), Utils.FormatValue(collection)));
//        }
//        public static void DoesNotContain(System.Collections.IEnumerable collection, object item, string throwMessage = null)
//        {
//            Throw(!collection.Cast<object>().Contains(item), () => string.Format(throwMessage ?? "Collection does contain {0}", Utils.FormatValue(item)));
//        }
//        public static void Contains(string str, string contains, bool ignoreCase, string throwMessage = null)
//        {
//            Throw(str.IndexOf(contains, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture) >= 0,
//              () => string.Format(throwMessage ?? "String doesn't contain substring  expected: {0}  found:  {1}", Utils.FormatValue(contains), Utils.FormatValue(str)));
//        }
//        public static void Contains(string str, string contains, string throwMessage = null)
//        {
//            Contains(str, contains, false, throwMessage);
//        }
//        public static void DoesNotContain(string str, string contains, bool ignoreCase = false, string throwMessage = null)
//        {
//            Throw(str.IndexOf(contains, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture) < 0,
//              () => string.Format(throwMessage ?? "String does contain substring  didn't expect: {0}  found:   {1}", Utils.FormatValue(contains), Utils.FormatValue(str)));
//        }
//        public static void Matches(string str, string regex, RegexOptions options = RegexOptions.None, string throwMessage = null)
//        {
//            Throw(new Regex(regex, options).IsMatch(str), () => string.Format(throwMessage ?? "String doesn't match expression  regex: \"{0}\"  found: {1}", regex, Utils.FormatValue(str)));
//        }
//        public static void DoesNotMatch(string str, string regex, RegexOptions options = RegexOptions.None, string throwMessage = null)
//        {
//            Throw(!(new Regex(regex, options).IsMatch(str)), () => string.Format(throwMessage ?? "String matches expression  regex: \"{0}\"  found: {1}", regex, Utils.FormatValue(str)));
//        }
//        public static void IsNull(object val, string throwMessage = null)
//        {
//            Throw(val == null, () => string.Format(throwMessage ?? "Object reference is not null - {0}", Utils.FormatValue(val)));
//        }
//        public static void IsNotNull(object val, string throwMessage = null)
//        {
//            Throw(val != null, () => throwMessage ?? "Object reference is null");
//        }
//        public static void Compare(object a, object b, Func<int, bool> Check, string comparison, string throwMessage = null)
//        {
//            Throw(Check((a as IComparable).CompareTo(b)), () => string.Format(throwMessage ?? "Comparison failed: {0} {1} {2}", Utils.FormatValue(a), comparison, Utils.FormatValue(b)));
//        }

//        public static void Greater<T>(T a, T b, string throwMessage = null)
//        {
//            Compare(a, b, r => r > 0, ">", throwMessage);
//        }
//        public static void GreaterOrEqual<T>(T a, T b, string throwMessage = null)
//        {
//            Compare(a, b, r => r >= 0, ">", throwMessage);
//        }
//        public static void Less<T>(T a, T b, string throwMessage = null)
//        {
//            Compare(a, b, r => r < 0, ">", throwMessage);
//        }
//        public static void LessOrEqual<T>(T a, T b, string throwMessage = null)
//        {
//            Compare(a, b, r => r <= 0, ">", throwMessage);
//        }

//        public static void IsInstanceOf(Type t, object o, string throwMessage = null)
//        {
//            IsNotNull(o); Throw(o.GetType() == t, () => string.Format(throwMessage ?? "Object type mismatch, expected {0} found {1}", t.FullName, o.GetType().FullName));
//        }
//        public static void IsNotInstanceOf(Type t, object o, string throwMessage = null)
//        {
//            IsNotNull(o); Throw(o.GetType() != t, () => string.Format(throwMessage ?? "Object type mismatch, should not be {0}", t.FullName));
//        }
//        public static void IsInstanceOf<T>(object o, string throwMessage = null)
//        {
//            IsInstanceOf(typeof(T), o, throwMessage);
//        }
//        public static void IsNotInstanceOf<T>(object o, string throwMessage = null)
//        {
//            IsNotInstanceOf(typeof(T), o, throwMessage);
//        }
//        public static void IsAssignableFrom(Type t, object o, string throwMessage = null)
//        {
//            IsNotNull(o); Throw(o.GetType().IsAssignableFrom(t), () => string.Format(throwMessage ?? "Object type mismatch, expected a type assignable from {0} found {1}", t.FullName, o.GetType().FullName));
//        }
//        public static void IsNotAssignableFrom(Type t, object o, string throwMessage = null)
//        {
//            IsNotNull(o); Throw(!o.GetType().IsAssignableFrom(t), () => string.Format(throwMessage ?? "Object type mismatch, didn't expect a type assignable from {0} found {1}", t.FullName, o.GetType().FullName));
//        }
//        public static void IsAssignableFrom<T>(object o, string throwMessage = null)
//        {
//            IsAssignableFrom(typeof(T), o, throwMessage);
//        }
//        public static void IsNotAssignableFrom<T>(object o, string throwMessage = null)
//        {
//            IsNotAssignableFrom(typeof(T), o, throwMessage);
//        }
//        public static void IsAssignableTo(Type t, object o, string throwMessage = null)
//        {
//            IsNotNull(o); Throw(t.IsAssignableFrom(o.GetType()), () => string.Format(throwMessage ?? "Object type mismatch, expected a type assignable to {0} found {1}", t.FullName, o.GetType().FullName));
//        }
//        public static void IsNotAssignableTo(Type t, object o, string throwMessage = null)
//        {
//            IsNotNull(o); Throw(!t.IsAssignableFrom(o.GetType()), () => string.Format(throwMessage ?? "Object type mismatch, didn't expect a type assignable to {0} found {1}", t.FullName, o.GetType().FullName));
//        }
//        public static void IsAssignableTo<T>(object o, string throwMessage = null)
//        {
//            IsAssignableTo(typeof(T), o, throwMessage);
//        }
//        public static void IsNotAssignableTo<T>(object o, string throwMessage = null)
//        {
//            IsNotAssignableTo(typeof(T), o, throwMessage);
//        }

//        public static void AllItemsAreNotNull<T>(IEnumerable<T> coll, string throwMessage = null)
//        {
//            int index = 0;
//            foreach (var i in coll) {
//                if (i == null) {
//                    throw new AssertionException(string.Format(throwMessage ?? "Collection has a null item at index {0}", index));
//                }
//                index++;
//            }
//        }
//        public static void AllItemsAreUnique<T>(IEnumerable<T> coll, string throwMessage = null)
//        {
//            var list = coll.ToList();
//            for (int i = 0; i < list.Count; i++) {
//                for (int j = i + 1; j < list.Count; j++) {
//                    if (object.Equals(list[i], list[j])) {
//                        throw new AssertionException(string.Format(throwMessage ?? "Collection items are not unique  [{0}] = {1}  [{2}] = {3}", i, list[i], j, list[j]));
//                    }
//                }
//            }
//        }
//        public static void AllItemsAreEqual<Ta, Tb>(IEnumerable<Ta> a, IEnumerable<Tb> b, Func<Ta, Tb, bool> CompareEqual, string throwMessage = null)
//        {
//            var e1 = a.GetEnumerator();
//            var e2 = b.GetEnumerator();
//            int index = 0;
//            while (true) {
//                bool have1 = e1.MoveNext();
//                bool have2 = e2.MoveNext();
//                if (!have1 && !have2)
//                    return;
//                if (!have1 || !have2 || !CompareEqual(e1.Current, e2.Current)) {
//                    throw new AssertionException(string.Format(throwMessage ?? "Collection are not equal at index {0}  a[{0}] = {1}  b[{0}] = {2}", index, e1.Current, e2.Current));
//                }
//                index++;
//            }
//        }
//        public static void AllItemsAreEqual<T>(IEnumerable<T> a, IEnumerable<T> b, string throwMessage = null)
//        {
//            AllItemsAreEqual<T, T>(a, b, (x, y) => object.Equals(x, y), throwMessage);
//        }
//        public static void AllItemsAreInstancesOf(Type t, System.Collections.IEnumerable coll, string throwMessage = null)
//        {
//            int index = 0;
//            foreach (object o in coll) {
//                if (o == null || o.GetType() != t) {
//                    throw new AssertionException(string.Format(throwMessage ?? "Collection item at index {0} is of the wrong type, expected {1} but found {2}", index, t.FullName, o == null ? "null" : o.GetType().FullName));
//                }
//                index++;
//            }
//        }
//        public static void AllItemsAreInstancesOf<T>(System.Collections.IEnumerable coll, string throwMessage = null)
//        {
//            AllItemsAreInstancesOf(typeof(T), coll, throwMessage);
//        }
//        public static void IsSubsetOf<Ta, Tb>(IEnumerable<Ta> subset, IEnumerable<Tb> superset, Func<Ta, Tb, bool> CompareEqual, string throwMessage = null)
//        {
//            var list = superset.ToList();
//            int index = 0;
//            foreach (var i in subset) {
//                int pos = IndexOf<Ta, Tb>(i, list, CompareEqual);
//                if (pos < 0) {
//                    throw new AssertionException(string.Format(throwMessage ?? "Collection is not a subset (check subset index {0}  subset = {1}  superset = {2}", index, Utils.FormatValue(subset), Utils.FormatValue(superset)));
//                }
//                list.RemoveAt(pos);
//                index++;
//            }
//        }
//        public static void IsSubsetOf<T>(IEnumerable<T> subset, IEnumerable<T> superset, string throwMessage = null)
//        {
//            IsSubsetOf<T, T>(subset, superset, (x, y) => Object.Equals(x, y), throwMessage);
//        }
//        public static void IsNotSubsetOf<Ta, Tb>(IEnumerable<Ta> subset, IEnumerable<Tb> superset, Func<Ta, Tb, bool> CompareEqual, string throwMessage = null)
//        {
//            var list = superset.ToList();
//            foreach (var i in subset) {
//                int pos = IndexOf<Ta, Tb>(i, list, CompareEqual);
//                if (pos < 0)
//                    return;
//                list.RemoveAt(pos);
//            }
//            throw new AssertionException(string.Format(throwMessage ?? "Collection is a subset  subset = {0}  superset = {1}", Utils.FormatValue(subset), Utils.FormatValue(superset)));
//        }
//        public static void IsNotSubsetOf<T>(IEnumerable<T> subset, IEnumerable<T> superset, string throwMessage = null)
//        {
//            IsNotSubsetOf<T, T>(subset, superset, (x, y) => Object.Equals(x, y), throwMessage);
//        }
//        public static void AreEquivalent<Ta, Tb>(IEnumerable<Ta> a, IEnumerable<Tb> b, Func<Ta, Tb, bool> CompareEqual, string throwMessage = null)
//        {
//            Throw(TestEquivalent(a, b, CompareEqual), () => string.Format(throwMessage ?? "Collections are not equivalent  lhs: {0}  rhs: {1}", Utils.FormatValue(a), Utils.FormatValue(b)));
//        }
//        public static void AreNotEquivalent<Ta, Tb>(IEnumerable<Ta> a, IEnumerable<Tb> b, Func<Ta, Tb, bool> CompareEqual, string throwMessage = null)
//        {
//            Throw(!TestEquivalent(a, b, CompareEqual), () => string.Format(throwMessage ?? "Collections are not equivalent  lhs: {0}  rhs: {1}", Utils.FormatValue(a), Utils.FormatValue(b)));
//        }
//        public static void AreEquivalent<T>(IEnumerable<T> a, IEnumerable<T> b, string throwMessage = null)
//        {
//            AreEquivalent<T, T>(a, b, (x, y) => Object.Equals(x, y), throwMessage);
//        }
//        public static void AreNotEquivalent<T>(IEnumerable<T> a, IEnumerable<T> b, string throwMessage = null)
//        {
//            AreNotEquivalent<T, T>(a, b, (x, y) => Object.Equals(x, y), throwMessage);
//        }


//        private static void Throw(bool Condition, Func<string> message)
//        {
//            if (!Condition) {
//                throw new AssertionException(message());
//            }
//        }
//        private static bool TestEqual(object a, object b)
//        {
//            if (a == null && b == null)
//                return true;
//            if (a == null || b == null)
//                return false;
//            return Object.Equals(a, b);
//        }
//        private static void AreEqual(object a, object b, Func<bool> Compare, string throwMessage = null)
//        {
//            Throw(Compare(), () => string.Format(throwMessage ?? "Objects are not equal  lhs: {0}  rhs: {1}", Utils.FormatValue(a), Utils.FormatValue(b)));
//        }
//        private static void AreNotEqual(object a, object b, Func<bool> Compare, string throwMessage = null)
//        {
//            Throw(!Compare(), () => string.Format(throwMessage ?? "Objects are not equal  lhs: {0}  rhs: {1}", Utils.FormatValue(a), Utils.FormatValue(b)));
//        }
//        private static int IndexOf<Ta, Tb>(Ta Item, List<Tb> list, Func<Ta, Tb, bool> CompareEqual)
//        {
//            for (int j = 0; j < list.Count; j++) {
//                if (CompareEqual(Item, list[j]))
//                    return j;
//            }
//            return -1;
//        }
//        private static bool TestEquivalent<Ta, Tb>(IEnumerable<Ta> a, IEnumerable<Tb> b, Func<Ta, Tb, bool> CompareEqual)
//        {
//            var list = b.ToList();
//            foreach (var i in a) {
//                int pos = IndexOf(i, list, CompareEqual);
//                if (pos < 0)
//                    return false;
//                list.RemoveAt(pos);
//            }
//            return list.Count == 0;
//        }

//    }

//}
