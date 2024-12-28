using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using ToolGood.ReadyGo3;

namespace ToolGood.Bedrock.DataCommon
{
    public class DiffHelper
    {
        public static string Diff<T>(T left, T right)
        {
            MyTypeInfo myTypeInfo = new MyTypeInfo(typeof(T));
            return myTypeInfo.DiffMessage(left, right);
        }
        public static string Diff<T>(T left, T right, SqlHelper sqlHelper)
        {
            MyTypeInfo myTypeInfo = new MyTypeInfo(typeof(T));
            myTypeInfo.SetEnumNameFromDatabase(sqlHelper);
            return myTypeInfo.DiffMessage(left, right);
        }
        public static string Diff(List<string> lefts, List<string> rights)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var removes = lefts.Except(rights).ToList();
            var adds = rights.Except(lefts).ToList();

            if (adds.Count > 0) {
                stringBuilder.Append("增加：");
                for (int i = 0; i < adds.Count; i++) {
                    if (i > 0) { stringBuilder.Append(','); }
                    stringBuilder.Append(adds[i]);
                }
            }
            if (removes.Count > 0) {
                if (stringBuilder.Length > 0) { stringBuilder.Append('，'); }
                stringBuilder.Append("删除：");
                for (int i = 0; i < removes.Count; i++) {
                    if (i > 0) { stringBuilder.Append(','); }
                    stringBuilder.Append(removes[i]);
                }
            }
            return stringBuilder.ToString();
        }
        public static string Diff(List<int> lefts, List<int> rights)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var removes = lefts.Except(rights).ToList();
            var adds = rights.Except(lefts).ToList();

            if (adds.Count > 0) {
                stringBuilder.Append("增加：");
                for (int i = 0; i < adds.Count; i++) {
                    if (i > 0) { stringBuilder.Append(','); }
                    stringBuilder.Append(adds[i]);
                }
            }
            if (removes.Count > 0) {
                if (stringBuilder.Length > 0) { stringBuilder.Append('，'); }
                stringBuilder.Append("删除：");
                for (int i = 0; i < removes.Count; i++) {
                    if (i > 0) { stringBuilder.Append(','); }
                    stringBuilder.Append(removes[i]);
                }
            }
            return stringBuilder.ToString();
        }
        public static string Diff(List<int> lefts, List<int> rights, Dictionary<int, string> dict)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var removes = lefts.Except(rights).ToList();
            var adds = rights.Except(lefts).ToList();

            if (adds.Count > 0) {
                stringBuilder.Append("增加：");
                for (int i = 0; i < adds.Count; i++) {
                    //if (i > 0) { stringBuilder.Append(','); }
                    stringBuilder.Append(adds[i]);
                    stringBuilder.Append('(');
                    if (dict.TryGetValue(adds[i], out string name)) {
                        stringBuilder.Append(name);
                    }
                    stringBuilder.Append(')');
                }
            }
            if (removes.Count > 0) {
                if (stringBuilder.Length > 0) { stringBuilder.Append('，'); }
                stringBuilder.Append("删除：");
                for (int i = 0; i < removes.Count; i++) {
                    stringBuilder.Append(adds[i]);
                    stringBuilder.Append('(');
                    if (dict.TryGetValue(adds[i], out string name)) {
                        stringBuilder.Append(name);
                    }
                    stringBuilder.Append(')');
                }
            }
            return stringBuilder.ToString();
        }
        public static string Diff<T>(List<T> lefts, List<T> rights, Func<T, int> func, Dictionary<int, string> dict)
        {
            var left = new List<int>(lefts.Count);
            foreach (var item in lefts) {
                left.Add(func(item));
            }
            var right = new List<int>(rights.Count);
            foreach (var item in rights) {
                right.Add(func(item));
            }
            return Diff(left, right, dict);
        }
    }

    class MyTypeInfo
    {
        public string Name { get; set; }
        public MyPropertyInfo IdPropertyInfo { get; set; }
        public List<MyPropertyInfo> PropertyInfos { get; set; }

        public MyTypeInfo(Type type)
        {
            PropertyInfos = new List<MyPropertyInfo>();

            var cas = type.GetCustomAttributes();
            foreach (var ca in cas) {
                if (ca is DataNameAttribute attribute) {
                    Name = attribute.DisplayName;
                    break;
                }
            }

            var ps = type.GetProperties();
            foreach (var p in ps) {
                if (p.Name.Equals("id", StringComparison.OrdinalIgnoreCase)) {
                    MyPropertyInfo myPropertyInfo = new MyPropertyInfo();
                    myPropertyInfo.Property = p;
                    IdPropertyInfo = myPropertyInfo;
                    continue;
                }
                var ass = p.GetCustomAttributes();
                foreach (var a in ass) {
                    if (a is DataEnumSqlAttribute dataEnumSql) {
                        MyPropertyInfo myPropertyInfo = new MyPropertyInfo();
                        myPropertyInfo.Property = p;
                        myPropertyInfo.DisplayName = dataEnumSql.DisplayName;
                        myPropertyInfo.Sql = dataEnumSql.Sql;
                        PropertyInfos.Add(myPropertyInfo);
                    } else if (a is DataEnumAttribute dataEnum) {
                        MyPropertyInfo myPropertyInfo = new MyPropertyInfo();
                        myPropertyInfo.Property = p;
                        myPropertyInfo.DisplayName = dataEnum.DisplayName;
                        myPropertyInfo.EnumNames = new Dictionary<string, string>();
                        for (int i = 0; i < dataEnum.EnumName.Length; i++) {
                            myPropertyInfo.EnumNames[i.ToString()] = dataEnum.EnumName[i];
                        }
                        PropertyInfos.Add(myPropertyInfo);
                    } else if (a is DataNameAttribute dataName) {
                        MyPropertyInfo myPropertyInfo = new MyPropertyInfo();
                        myPropertyInfo.Property = p;
                        myPropertyInfo.DisplayName = dataName.DisplayName;
                        if (p.PropertyType.IsEnum) {
                            myPropertyInfo.EnumNames = GetDescriptions(p.PropertyType);
                        }
                        PropertyInfos.Add(myPropertyInfo);
                    }
                }
            }
        }
        /// <summary>
        /// Enum  获取枚举值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetDescriptions(Type type)
        {
            var typeHandle = type.TypeHandle;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            var enumList = Enum.GetValues(type);
            foreach (int item in enumList) {
                var field = type.GetField(Enum.GetName(type, item));
                if (field == null) continue;

                var attr = field.GetCustomAttributes(typeof(DataNameAttribute), false) as DataNameAttribute[];
                if ((attr != null) && (attr.Length == 1)) {
                    dict.Add(field.Name, attr[0].DisplayName);
                }
            }
            return dict;
        }

        public void SetEnumNameFromDatabase(SqlHelper helper)
        {
            foreach (var item in PropertyInfos) {
                if (string.IsNullOrEmpty(item.Sql)) { continue; }
                try {
                    var table = helper.ExecuteDataTable(item.Sql);
                    item.EnumNames = new Dictionary<string, string>();
                    foreach (DataRow row in table.Rows) {
                        var key = row[0].ToString().Trim();
                        var value = row[1].ToString().Trim();
                        item.EnumNames[key] = value;
                    }
                } catch (Exception) { }
            }
        }

        public string DiffMessage<T>(T left, T right)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (IdPropertyInfo != null) {
                if (IdPropertyInfo.IsChange(left, right)) {
                    var id = IdPropertyInfo.Property.GetValue(right);
                    stringBuilder.Append($"新增{Name}[{id}]");
                    foreach (var propertyInfo in PropertyInfos) {
                        propertyInfo.NewValue(right, stringBuilder);
                    }
                } else {
                    var id = IdPropertyInfo.Property.GetValue(right);
                    stringBuilder.Append($"修改{Name}[{id}]");

                    foreach (var propertyInfo in PropertyInfos) {
                        propertyInfo.Diff(left, right, stringBuilder);
                    }
                }
            }
            return stringBuilder.ToString();
        }

    }

    class MyPropertyInfo
    {
        public PropertyInfo Property { get; set; }
        public string DisplayName { get; set; }
        public string Sql { get; set; }
        public Dictionary<string, string> EnumNames { get; set; }

        public bool IsChange<T>(T left, T right)
        {
            var leftValue = Property.GetValue(left);
            var rightValue = Property.GetValue(right);
            if (leftValue.Equals(rightValue)) { return false; }
            return true;
        }

        public void NewValue(object right, StringBuilder stringBuilder)
        {
            var rightValue = Property.GetValue(right);
            if (stringBuilder.Length != 0) { stringBuilder.Append('，'); }
            if (EnumNames == null) {
                if (Property.PropertyType == typeof(DateTime)) {
                    stringBuilder.Append($"{DisplayName}:{(DateTime)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateTimeOffset)) {
                    stringBuilder.Append($"{DisplayName}:{(DateTimeOffset)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeSpan)) {
                    stringBuilder.Append($"{DisplayName}:{(TimeSpan)rightValue:d HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeOnly)) {
                    stringBuilder.Append($"{DisplayName}:{(TimeOnly)rightValue:HH:mm:ss}");

                } else if (Property.PropertyType == typeof(DateTime?)) {
                    stringBuilder.Append($"{DisplayName}:{(DateTime?)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateTimeOffset?)) {
                    stringBuilder.Append($"{DisplayName}:{(DateTimeOffset?)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeSpan?)) {
                    stringBuilder.Append($"{DisplayName}:{(TimeSpan?)rightValue:d HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeOnly?)) {
                    stringBuilder.Append($"{DisplayName}:{(TimeOnly?)rightValue:HH:mm:ss}");
                }
                stringBuilder.Append($"{DisplayName}:{rightValue ?? "(NULL)"}");
                return;
            }
            if (Property.PropertyType.IsEnum) {
                stringBuilder.Append(DisplayName);
                stringBuilder.Append(':');
                stringBuilder.Append(rightValue);
                if (EnumNames.TryGetValue(rightValue.ToString(), out string rv)) {
                    stringBuilder.Append('(');
                    stringBuilder.Append(rv);
                    stringBuilder.Append(')');
                }
            } else if (Property.PropertyType == typeof(string)) {
                stringBuilder.Append(DisplayName);
                stringBuilder.Append(':');
                if (rightValue == null) {
                    stringBuilder.Append("(NULL)");
                } else {
                    var rs = rightValue.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var r in rs) {
                        stringBuilder.Append(r);
                        stringBuilder.Append('(');
                        if (EnumNames.TryGetValue(r.ToString(), out string rv)) {
                            stringBuilder.Append(rv);
                        }
                        stringBuilder.Append(')');
                    }
                }
            } else if (Property.PropertyType == typeof(byte)
                || Property.PropertyType == typeof(ushort)
                || Property.PropertyType == typeof(uint)
                || Property.PropertyType == typeof(ulong)
                || Property.PropertyType == typeof(sbyte)
                || Property.PropertyType == typeof(short)
                || Property.PropertyType == typeof(int)
                || Property.PropertyType == typeof(long)

                || Property.PropertyType == typeof(byte?)
                || Property.PropertyType == typeof(ushort?)
                || Property.PropertyType == typeof(uint?)
                || Property.PropertyType == typeof(ulong?)
                || Property.PropertyType == typeof(sbyte?)
                || Property.PropertyType == typeof(short?)
                || Property.PropertyType == typeof(int?)
                || Property.PropertyType == typeof(long?)
                ) {
                stringBuilder.Append(DisplayName);
                stringBuilder.Append(':');
                if (rightValue == null) {
                    stringBuilder.Append("(NULL)");
                } else {
                    stringBuilder.Append(rightValue);
                    stringBuilder.Append('(');
                    if (EnumNames.TryGetValue(rightValue.ToString(), out string rv)) {
                        stringBuilder.Append(rv);
                    }
                    stringBuilder.Append(')');
                }
            } else {
                stringBuilder.Append($"{DisplayName}:{rightValue??"(NULL)"}");
            }
        }

        public void Diff<T>(T left, T right, StringBuilder stringBuilder)
        {
            var leftValue = Property.GetValue(left);
            var rightValue = Property.GetValue(right);

            if (null == leftValue && null == rightValue) { return; }
            if (null != leftValue && leftValue.Equals(rightValue)) { return; }
            if (stringBuilder.Length != 0) { stringBuilder.Append('，'); }

            if (EnumNames == null) {
                if (Property.PropertyType == typeof(DateTime)) {
                    stringBuilder.Append($"{DisplayName}:{(DateTime)leftValue:yyyy-MM-dd HH:mm:ss}=>{(DateTime)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateTimeOffset)) {
                    stringBuilder.Append($"{DisplayName}:{(DateTimeOffset)leftValue:yyyy-MM-dd HH:mm:ss}=>{(DateTimeOffset)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeSpan)) {
                    stringBuilder.Append($"{DisplayName}:{(TimeSpan)leftValue:d HH:mm:ss}=>{(TimeSpan)rightValue:d HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeOnly)) {
                    stringBuilder.Append($"{DisplayName}:{(TimeOnly)leftValue:HH:mm:ss}=>{(TimeOnly)rightValue:HH:mm:ss}");

                } else if (Property.PropertyType == typeof(DateTime?)) {
                    stringBuilder.Append($"{DisplayName}:{(DateTime?)leftValue:yyyy-MM-dd HH:mm:ss}=>{(DateTime?)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateTimeOffset?)) {
                    stringBuilder.Append($"{DisplayName}:{(DateTimeOffset?)leftValue:yyyy-MM-dd HH:mm:ss}=>{(DateTimeOffset?)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeSpan?)) {
                    stringBuilder.Append($"{DisplayName}:{(TimeSpan?)leftValue:d HH:mm:ss}=>{(TimeSpan?)rightValue:d HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeOnly?)) {
                    stringBuilder.Append($"{DisplayName}:{(TimeOnly?)leftValue:HH:mm:ss}=>{(TimeOnly?)rightValue:HH:mm:ss}");
                }
                stringBuilder.Append($"{DisplayName}:{leftValue ?? "(NULL)"}=>{rightValue ?? "(NULL)"}");
                return;
            }
            if (Property.PropertyType.IsEnum) {
                stringBuilder.Append(DisplayName);
                stringBuilder.Append(':');

                stringBuilder.Append(leftValue);
                if (EnumNames.TryGetValue(leftValue.ToString(), out string lv)) {
                    stringBuilder.Append('(');
                    stringBuilder.Append(lv);
                    stringBuilder.Append(')');
                }
                stringBuilder.Append('=');
                stringBuilder.Append('>');

                stringBuilder.Append(rightValue);
                if (EnumNames.TryGetValue(rightValue.ToString(), out string rv)) {
                    stringBuilder.Append('(');
                    stringBuilder.Append(rv);
                    stringBuilder.Append(')');
                }
            } else if (Property.PropertyType == typeof(string)) {
                stringBuilder.Append(DisplayName);
                stringBuilder.Append(':');
                if (null == leftValue) {
                    stringBuilder.Append("(NULL)");
                } else {
                    var ls = leftValue.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var l in ls) {
                        stringBuilder.Append(l);
                        stringBuilder.Append('(');
                        if (EnumNames.TryGetValue(l.ToString(), out string lv)) {
                            stringBuilder.Append(lv);
                        }
                        stringBuilder.Append(')');
                    }
                }
                stringBuilder.Append('=');
                stringBuilder.Append('>');

                if (null == rightValue) {
                    stringBuilder.Append("(NULL)");
                } else {
                    var rs = rightValue.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var r in rs) {
                        stringBuilder.Append(r);
                        stringBuilder.Append('(');
                        if (EnumNames.TryGetValue(r.ToString(), out string rv)) {
                            stringBuilder.Append(rv);
                        }
                        stringBuilder.Append(')');
                    }
                }
            } else if (Property.PropertyType == typeof(byte)
                || Property.PropertyType == typeof(ushort)
                || Property.PropertyType == typeof(uint)
                || Property.PropertyType == typeof(ulong)
                || Property.PropertyType == typeof(sbyte)
                || Property.PropertyType == typeof(short)
                || Property.PropertyType == typeof(int)
                || Property.PropertyType == typeof(long)

                || Property.PropertyType == typeof(byte?)
                || Property.PropertyType == typeof(ushort?)
                || Property.PropertyType == typeof(uint?)
                || Property.PropertyType == typeof(ulong?)
                || Property.PropertyType == typeof(sbyte?)
                || Property.PropertyType == typeof(short?)
                || Property.PropertyType == typeof(int?)
                || Property.PropertyType == typeof(long?)
                ) {
                stringBuilder.Append(DisplayName);
                stringBuilder.Append(':');

                if (leftValue == null) {
                    stringBuilder.Append("(NULL)");
                } else {
                    stringBuilder.Append(leftValue);
                    stringBuilder.Append('(');
                    if (EnumNames.TryGetValue(leftValue.ToString(), out string lv)) {
                        stringBuilder.Append(lv);
                    }
                    stringBuilder.Append(')');
                }

                stringBuilder.Append('=');
                stringBuilder.Append('>');

                if (rightValue == null) {
                    stringBuilder.Append("(NULL)");
                } else {
                    stringBuilder.Append(rightValue);
                    stringBuilder.Append('(');
                    if (EnumNames.TryGetValue(rightValue.ToString(), out string rv)) {
                        stringBuilder.Append(rv);
                    }
                    stringBuilder.Append(')');
                }
            } else {
                stringBuilder.Append($"{DisplayName}:{leftValue ?? "(NULL)"}=>{rightValue ?? "(NULL)"}");
            }

        }
    }


}
