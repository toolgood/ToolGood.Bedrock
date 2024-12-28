﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using ToolGood.ReadyGo3;

namespace ToolGood.Bedrock.DataCommon
{
    /// <summary>
    /// 数据变动转成文本 帮助类
    /// </summary>
    public static class DataDiffHelper
    {
        /// <summary>
        /// 数据变动转成文本
        /// </summary>
        /// <param name="left">原数据</param>
        /// <param name="right">新数据</param>
        /// <returns></returns>
        public static string Diff<T>(T left, T right) where T : class
        {
            DataDiffTypeInfo myTypeInfo = new DataDiffTypeInfo(typeof(T));
            return myTypeInfo.DiffMessage(left, right);
        }

        /// <summary>
        /// 数据变动转成文本
        /// </summary>
        /// <param name="left">原数据</param>
        /// <param name="right">新数据</param>
        /// <param name="sqlHelper"></param>
        /// <returns></returns>
        public static string Diff<T>(T left, T right, SqlHelper sqlHelper) where T : class
        {
            DataDiffTypeInfo myTypeInfo = new DataDiffTypeInfo(typeof(T));
            myTypeInfo.SetEnumNameFromDatabase(sqlHelper);
            return myTypeInfo.DiffMessage(left, right);
        }

        /// <summary>
        /// 数据变动转成文本
        /// </summary>
        /// <param name="lefts">原数据</param>
        /// <param name="rights"></param>
        /// <returns></returns>
        public static string Diff(List<string> lefts, List<string> rights)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var removes = lefts.Except(rights).ToList();
            var adds = rights.Except(lefts).ToList();

            if (adds.Count > 0) {
                stringBuilder.Append("增加：");
                for (int i = 0; i < adds.Count; i++) {
                    if (i > 0) { stringBuilder.Append('|'); }
                    stringBuilder.Append(adds[i]);
                }
            }
            if (removes.Count > 0) {
                if (stringBuilder.Length > 0) { stringBuilder.Append('，'); }
                stringBuilder.Append("删除：");
                for (int i = 0; i < removes.Count; i++) {
                    if (i > 0) { stringBuilder.Append('|'); }
                    stringBuilder.Append(removes[i]);
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 数据变动转成文本
        /// </summary>
        /// <param name="lefts">原数据</param>
        /// <param name="rights">新数据</param>
        /// <returns></returns>
        public static string Diff<T>(List<T> lefts, List<T> rights) where T : struct
        {
            StringBuilder stringBuilder = new StringBuilder();
            var removes = lefts.Except(rights).ToList();
            var adds = rights.Except(lefts).ToList();

            if (adds.Count > 0) {
                stringBuilder.Append("增加：");
                for (int i = 0; i < adds.Count; i++) {
                    if (i > 0) { stringBuilder.Append('|'); }
                    stringBuilder.Append(adds[i]);
                }
            }
            if (removes.Count > 0) {
                if (stringBuilder.Length > 0) { stringBuilder.Append('，'); }
                stringBuilder.Append("删除：");
                for (int i = 0; i < removes.Count; i++) {
                    if (i > 0) { stringBuilder.Append('|'); }
                    stringBuilder.Append(removes[i]);
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 数据变动转成文本
        /// </summary>
        /// <param name="lefts">原数据</param>
        /// <param name="rights">新数据</param>
        /// <param name="dict">字典</param>
        /// <returns></returns>
        public static string Diff<T>(List<T> lefts, List<T> rights, Dictionary<T, string> dict) where T : struct
        {
            StringBuilder stringBuilder = new StringBuilder();
            var removes = lefts.Except(rights).ToList();
            var adds = rights.Except(lefts).ToList();

            if (adds.Count > 0) {
                stringBuilder.Append("增加：");
                for (int i = 0; i < adds.Count; i++) {
                    if (i > 0) { stringBuilder.Append('|'); }
                    stringBuilder.Append(adds[i]);
                    stringBuilder.Append('=');
                    if (dict.TryGetValue(adds[i], out string name)) {
                        stringBuilder.Append(name);
                    }
                }
            }
            if (removes.Count > 0) {
                if (stringBuilder.Length > 0) { stringBuilder.Append('，'); }
                stringBuilder.Append("删除：");
                for (int i = 0; i < removes.Count; i++) {
                    if (i > 0) { stringBuilder.Append('|'); }
                    stringBuilder.Append(removes[i]);
                    stringBuilder.Append('=');
                    if (dict.TryGetValue(removes[i], out string name)) {
                        stringBuilder.Append(name);
                    }
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 数据变动转成文本
        /// </summary>
        /// <param name="lefts">原数据</param>
        /// <param name="rights">新数据</param>
        /// <param name="func"></param>
        /// <param name="dict">字典</param>
        /// <returns></returns>
        public static string Diff<T, T1>(List<T> lefts, List<T> rights, Func<T, T1> func, Dictionary<T1, string> dict)
            where T : class
            where T1 : struct
        {
            var left = new List<T1>(lefts.Count);
            foreach (var item in lefts) {
                left.Add(func(item));
            }
            var right = new List<T1>(rights.Count);
            foreach (var item in rights) {
                right.Add(func(item));
            }
            return Diff(left, right, dict);
        }
    }

    internal class DataDiffTypeInfo
    {
        public string Name { get; set; }
        public DataDiffPropertyInfo IdPropertyInfo { get; set; }
        public List<DataDiffPropertyInfo> PropertyInfos { get; set; }

        public DataDiffTypeInfo(Type type)
        {
            PropertyInfos = new List<DataDiffPropertyInfo>();

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
                    DataDiffPropertyInfo myPropertyInfo = new DataDiffPropertyInfo();
                    myPropertyInfo.Property = p;
                    IdPropertyInfo = myPropertyInfo;
                    continue;
                }
                var ass = p.GetCustomAttributes();
                foreach (var a in ass) {
                    if (a is DataEnumSqlAttribute dataEnumSql) {
                        DataDiffPropertyInfo myPropertyInfo = new DataDiffPropertyInfo();
                        myPropertyInfo.Property = p;
                        myPropertyInfo.DisplayName = dataEnumSql.DisplayName;
                        myPropertyInfo.Sql = dataEnumSql.Sql;
                        PropertyInfos.Add(myPropertyInfo);
                    } else if (a is DataEnumAttribute dataEnum) {
                        DataDiffPropertyInfo myPropertyInfo = new DataDiffPropertyInfo();
                        myPropertyInfo.Property = p;
                        myPropertyInfo.DisplayName = dataEnum.DisplayName;
                        myPropertyInfo.EnumNames = new Dictionary<string, string>();
                        for (int i = 0; i < dataEnum.EnumName.Length; i++) {
                            myPropertyInfo.EnumNames[i.ToString()] = dataEnum.EnumName[i];
                        }
                        PropertyInfos.Add(myPropertyInfo);
                    } else if (a is DataNameAttribute dataName) {
                        DataDiffPropertyInfo myPropertyInfo = new DataDiffPropertyInfo();
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
                    stringBuilder.Append($"新增[{Name ?? "id"}]{id}");
                    foreach (var propertyInfo in PropertyInfos) {
                        propertyInfo.NewValue(right, stringBuilder);
                    }
                } else {
                    var id = IdPropertyInfo.Property.GetValue(right);
                    stringBuilder.Append($"修改[{Name ?? "id"}]{id}");
                    foreach (var propertyInfo in PropertyInfos) {
                        propertyInfo.Diff(left, right, stringBuilder);
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }

    internal class DataDiffPropertyInfo
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
            if (null == rightValue) { return; }

            if (stringBuilder.Length != 0) { stringBuilder.Append('，'); }
            if (EnumNames == null) {
                if (Property.PropertyType == typeof(DateTime)) {
                    stringBuilder.Append($"{DisplayName}：{(DateTime)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateTimeOffset)) {
                    stringBuilder.Append($"{DisplayName}：{(DateTimeOffset)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeSpan)) {
                    stringBuilder.Append($"{DisplayName}：{(TimeSpan)rightValue:d HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeOnly)) {
                    stringBuilder.Append($"{DisplayName}：{(TimeOnly)rightValue:HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateOnly)) {
                    stringBuilder.Append($"{DisplayName}：{(DateOnly)rightValue:yyyy-MM-dd}");
                } else if (Property.PropertyType == typeof(DateTime?)) {
                    stringBuilder.Append($"{DisplayName}：{(DateTime?)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateTimeOffset?)) {
                    stringBuilder.Append($"{DisplayName}：{(DateTimeOffset?)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeSpan?)) {
                    stringBuilder.Append($"{DisplayName}：{(TimeSpan?)rightValue:d HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeOnly?)) {
                    stringBuilder.Append($"{DisplayName}：{(TimeOnly?)rightValue:HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateOnly?)) {
                    stringBuilder.Append($"{DisplayName}：{(DateOnly?)rightValue:yyyy-MM-dd}");
                } else {
                    stringBuilder.Append($"{DisplayName}：{rightValue ?? "(NULL)"}");
                }
                return;
            }
            if (Property.PropertyType.IsEnum) {
                stringBuilder.Append(DisplayName);
                stringBuilder.Append('：');
                stringBuilder.Append(rightValue);
                if (EnumNames.TryGetValue(rightValue.ToString(), out string rv)) {
                    stringBuilder.Append('=');
                    stringBuilder.Append(rv);
                }
            } else if (Property.PropertyType == typeof(string)) {
                stringBuilder.Append(DisplayName);
                stringBuilder.Append('：');
                if (rightValue == null) {
                    stringBuilder.Append("(NULL)");
                } else {
                    var rs = rightValue.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < rs.Length; i++) {
                        if (i > 0) { stringBuilder.Append('|'); }
                        var r = rs[i];
                        stringBuilder.Append(r);
                        if (EnumNames.TryGetValue(r.ToString(), out string rv)) {
                            stringBuilder.Append('=');
                            stringBuilder.Append(rv);
                        }
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
                stringBuilder.Append('：');
                if (rightValue == null) {
                    stringBuilder.Append("(NULL)");
                } else {
                    stringBuilder.Append(rightValue);
                    if (EnumNames.TryGetValue(rightValue.ToString(), out string rv)) {
                        stringBuilder.Append('=');
                        stringBuilder.Append(rv);
                    }
                }
            } else {
                stringBuilder.Append($"{DisplayName}：{rightValue ?? "(NULL)"}");
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
                    stringBuilder.Append($"{DisplayName}：{(DateTime)leftValue:yyyy-MM-dd HH:mm:ss}->{(DateTime)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateTimeOffset)) {
                    stringBuilder.Append($"{DisplayName}：{(DateTimeOffset)leftValue:yyyy-MM-dd HH:mm:ss}->{(DateTimeOffset)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeSpan)) {
                    stringBuilder.Append($"{DisplayName}：{(TimeSpan)leftValue:d HH:mm:ss}->{(TimeSpan)rightValue:d HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeOnly)) {
                    stringBuilder.Append($"{DisplayName}：{(TimeOnly)leftValue:HH:mm:ss}->{(TimeOnly)rightValue:HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateOnly)) {
                    stringBuilder.Append($"{DisplayName}：{(DateOnly)leftValue:yyyy-MM-dd}->{(DateOnly)rightValue:yyyy-MM-dd}");
                } else if (Property.PropertyType == typeof(DateTime?)) {
                    stringBuilder.Append($"{DisplayName}：{(DateTime?)leftValue:yyyy-MM-dd HH:mm:ss}->{(DateTime?)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateTimeOffset?)) {
                    stringBuilder.Append($"{DisplayName}：{(DateTimeOffset?)leftValue:yyyy-MM-dd HH:mm:ss}->{(DateTimeOffset?)rightValue:yyyy-MM-dd HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeSpan?)) {
                    stringBuilder.Append($"{DisplayName}：{(TimeSpan?)leftValue:d HH:mm:ss}->{(TimeSpan?)rightValue:d HH:mm:ss}");
                } else if (Property.PropertyType == typeof(TimeOnly?)) {
                    stringBuilder.Append($"{DisplayName}：{(TimeOnly?)leftValue:HH:mm:ss}->{(TimeOnly?)rightValue:HH:mm:ss}");
                } else if (Property.PropertyType == typeof(DateOnly?)) {
                    stringBuilder.Append($"{DisplayName}：{(DateOnly?)leftValue:yyyy-MM-dd}->{(DateOnly?)rightValue:yyyy-MM-dd}");
                } else {
                    stringBuilder.Append($"{DisplayName}：{leftValue ?? "(NULL)"}->{rightValue ?? "(NULL)"}");
                }
                return;
            }
            if (Property.PropertyType.IsEnum) {
                stringBuilder.Append(DisplayName);
                stringBuilder.Append('：');

                stringBuilder.Append(leftValue);
                if (EnumNames.TryGetValue(leftValue.ToString(), out string lv)) {
                    stringBuilder.Append('=');
                    stringBuilder.Append(lv);
                }
                stringBuilder.Append('-');
                stringBuilder.Append('>');

                stringBuilder.Append(rightValue);
                if (EnumNames.TryGetValue(rightValue.ToString(), out string rv)) {
                    stringBuilder.Append('=');
                    stringBuilder.Append(rv);
                }
            } else if (Property.PropertyType == typeof(string)) {
                stringBuilder.Append(DisplayName);
                stringBuilder.Append('：');
                if (null == leftValue) {
                    stringBuilder.Append("(NULL)");
                } else {
                    var ls = leftValue.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < ls.Length; i++) {
                        if (i > 0) { stringBuilder.Append('|'); }
                        var l = ls[i];
                        stringBuilder.Append(l);
                        if (EnumNames.TryGetValue(l.ToString(), out string lv)) {
                            stringBuilder.Append('=');
                            stringBuilder.Append(lv);
                        }
                    }
                }
                stringBuilder.Append('-');
                stringBuilder.Append('>');

                if (null == rightValue) {
                    stringBuilder.Append("(NULL)");
                } else {
                    var rs = rightValue.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < rs.Length; i++) {
                        if (i > 0) { stringBuilder.Append('|'); }
                        var r = rs[i];
                        stringBuilder.Append(r);
                        if (EnumNames.TryGetValue(r.ToString(), out string rv)) {
                            stringBuilder.Append('=');
                            stringBuilder.Append(rv);
                        }
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
                stringBuilder.Append('：');

                if (leftValue == null) {
                    stringBuilder.Append("(NULL)");
                } else {
                    stringBuilder.Append(leftValue);
                    if (EnumNames.TryGetValue(leftValue.ToString(), out string lv)) {
                        stringBuilder.Append('=');
                        stringBuilder.Append(lv);
                    }
                }

                stringBuilder.Append('-');
                stringBuilder.Append('>');

                if (rightValue == null) {
                    stringBuilder.Append("(NULL)");
                } else {
                    stringBuilder.Append(rightValue);
                    if (EnumNames.TryGetValue(rightValue.ToString(), out string rv)) {
                        stringBuilder.Append('=');
                        stringBuilder.Append(rv);
                    }
                }
            } else {
                stringBuilder.Append($"{DisplayName}：{leftValue ?? "(NULL)"}=>{rightValue ?? "(NULL)"}");
            }
        }
    }
}