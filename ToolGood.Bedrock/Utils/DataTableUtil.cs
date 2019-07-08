using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Data
{
    /// <summary>
    /// DataTable
    /// </summary>
    public static class DataTableUtil
    {

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> list)
        {
            var type = typeof(T);
            var properties = type.GetProperties().ToList();
            properties.RemoveAll(q => q.CanRead == false);
            var newDt = new DataTable(type.Name);
            properties.ForEach(propertie => {
                Type columnType;
                if (propertie.PropertyType.IsGenericType && propertie.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                    columnType = propertie.PropertyType.GetGenericArguments()[0];
                } else {
                    columnType = propertie.PropertyType;
                }
                newDt.Columns.Add(propertie.Name, columnType);
            });

            foreach (var item in list) {
                var newRow = newDt.NewRow();
                properties.ForEach(propertie => {
                    newRow[propertie.Name] = propertie.GetValue(item, null) ?? DBNull.Value;
                });
                newDt.Rows.Add(newRow);
            }

            return newDt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable dt)
        {
            var lst = new List<T>();
            var plist = new List<System.Reflection.PropertyInfo>(typeof(T).GetProperties());
            plist.RemoveAll(q => q.CanWrite == false);

            foreach (DataRow item in dt.Rows) {
                T t = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++) {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null) {
                        if (!Convert.IsDBNull(item[i])) {
                            var obj = item[i].ConvertTo(info.PropertyType);
                            info.SetValue(t, obj, null);
                        }
                    }
                }
                lst.Add(t);
            }
            return lst;
        }

         


    }

}
