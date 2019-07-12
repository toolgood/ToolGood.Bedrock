using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ObjectExtension
    {
        /// <summary>
        /// 判断是否包含某列名（不区分大小写）
        /// </summary>
        /// <param name="row">要判断的行</param>
        /// <param name="columnName">要判断的列名字</param>
        /// <returns>包含则返回true</returns>
        public static bool IsContainColumn(this DataRow row, string columnName)
        {
            if (row == null) return false;
            return row.Table.IsContainColumn(columnName);
        }

        /// <summary>
        /// 判断是否包含某列名（不区分大小写）
        /// </summary>
        /// <param name="table">要判断的datatable</param>
        /// <param name="columnName">要判断的列名字</param>
        /// <returns>包含则返回true</returns>
        public static bool IsContainColumn(this DataTable table, string columnName)
        {
            if (table == null) return false;
            if (string.IsNullOrWhiteSpace(columnName)) return false;
            return table.Columns.IndexOf(columnName) >= 0;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="row">要获取的DataRow</param>
        /// <param name="columnName">列名</param>
        /// <returns>值</returns>
        public static object GetValue(this DataRow row, string columnName)
        {
            if (IsContainColumn(row, columnName)) {
                var value = row[columnName];
                if (value == DBNull.Value) {
                    value = null;
                }
                return value;
            } else {
                return null;
            }
        }
    }
}
