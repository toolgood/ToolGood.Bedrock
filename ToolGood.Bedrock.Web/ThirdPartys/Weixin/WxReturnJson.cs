namespace ToolGood.Bedrock.Web.ThirdPartys.Weixin
{
    /// <summary>
    /// 微信接口返回值
    /// </summary>
    public class WxReturnJson
    {
        /// <summary>
        /// 返回代码  0=正确
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 返货错误
        /// </summary>
        public string errmsg { get; set; }
    }
    #endregion
}
