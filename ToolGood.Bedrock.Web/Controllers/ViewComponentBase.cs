//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using ToolGood.Bedrock.Web.Constants;

//namespace ToolGood.Bedrock.Web
//{
//    [ViewComponent]
//    public abstract class ViewComponentBase : ViewComponent
//    {
//        protected int SuccessCode { get { return CommonConstants.SuccessCode; } }
//        protected int ErrorCode { get { return CommonConstants.ErrorCode; } }
//        protected QueryArgs QueryArgs { get; set; }

//        public Task<IViewComponentResult> InvokeAsync()
//        {
//            ViewData["SuccessCode"] = SuccessCode;
//            ViewData["ErrorCode"] = ErrorCode;

//            if (HttpContext.Items.ContainsKey("ToolGood.Bedrock.QueryArgsBase")) {
//                QueryArgs = HttpContext.Items["ToolGood.Bedrock.QueryArgsBase"] as QueryArgs;
//                LogUtil.QueryArgs = QueryArgs;
//                ViewData["QueryArgs"] = QueryArgs;
//            }
//            return InvokeView();
//        }
//        protected abstract Task<IViewComponentResult> InvokeView();


//    }
//}
