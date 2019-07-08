namespace ToolGood.Bedrock.Web
{
    public class MyConfig
    {
        /// <summary>
        /// 使用缓存
        /// </summary>
        public bool UseResponseCaching { get; set; } = true;
        /// <summary>
        /// 使用 压缩
        /// </summary>
        public bool UseResponseCompression { get; set; } = true;
        /// <summary>
        /// 使用 Session
        /// </summary>
        public bool UseSession { get; set; } = true;
        /// <summary>
        /// 使用 Cookie
        /// </summary>
        public bool UseCookie { get; set; } = true;
        /// <summary>
        /// 使用 Let's Encrypt
        /// </summary>
        public bool UselLetsEncrypt { get; set; } = true;
        /// <summary>
        /// 使用 https
        /// </summary>
        public bool UseHsts { get; set; } = false;
        /// <summary>
        /// 使用 MVC
        /// </summary>
        public bool UseMvc { get; set; } = true;
        /// <summary>
        /// 使用 跨域
        /// </summary>
        public bool UseCors { get; set; } = false;

        /// <summary>
        /// 使用 IHttpContextAccessor
        /// </summary>
        public bool UseIHttpContextAccessor { get; set; } = true;

        /// <summary>
        /// 使用 主题 
        /// </summary>
        public bool UseTheme { get; set; } = false;
        /// <summary>
        /// 使用 插件
        /// </summary>
        public bool UsePlugin { get; set; } = false;

    }
}
