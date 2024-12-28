namespace ToolGood.Bedrock.DataCommon
{
    public class DataEnumSqlAttribute : DataNameAttribute
    {
        public string Sql { get; set; }

        public DataEnumSqlAttribute(string displayName, string sql) : base(displayName)
        {
            Sql = sql;
        }
    }


}
