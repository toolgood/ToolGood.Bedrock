namespace ToolGood.Bedrock.DataCommon.JsonDiffer
{
    public class TargetNode
    {
        public TargetNode(string symbol, string property)
        {
            Symbol = symbol;
            Property = property;
        }

        public string Symbol { get; set; }
        public string Property { get; set; }
    }
}
