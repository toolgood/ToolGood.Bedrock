using System.Linq.Expressions;

namespace ToolGood.HtmlExtract
{
    class FieldExpVistor : ExpressionVisitor
    {
        public string Field { get; set; }
        protected override Expression VisitMember(MemberExpression node)
        {
            Field = node.Member.Name;
            return base.VisitMember(node);
        }
    }

}
