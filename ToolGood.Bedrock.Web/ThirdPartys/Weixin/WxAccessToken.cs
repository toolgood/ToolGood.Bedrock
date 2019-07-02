namespace ToolGood.Bedrock.Web.ThirdPartys.Weixin
{
    /// <summary>
    /// 微信帮助
    /// </summary>
    public class WxAccessToken
    {
        /// <summary>
        /// access_token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public int expires_in { get; set; }
    }
    #endregion
}
