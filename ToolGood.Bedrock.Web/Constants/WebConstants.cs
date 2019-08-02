namespace ToolGood.Bedrock.Web.Constants
{
    /// <summary>
    /// WEB 常量
    /// </summary>
    public static class WebConstants
    {
        public const string AdminArea = "admin";
        public const string ApiArea = "api";
        public const string AppArea = "app";
        public const string MemberArea = "member";

        public const string AdminCookie = "aid";
        public const string MemberCookie = "mid";

        public const string AdminSession = "admin";
        public const string MemberSession = "member";

        public const string AdminLoginCode = "aCode";
        public const string MemberLoginCode = "mCode";

        public const string AdminToken = "atk";
        public const string MemberToken = "mtk";

        public const string Theme = "theme";

        /// <summary>
        /// cookie 密码
        /// </summary>
        public static string CookiePassword = "abc123456789";

        public static string AdminNoAccessUrl = "/admin/NoAccess";

        /// <summary>
        /// 登录地址
        /// </summary>
        public static string AdminLoginUrl = "/admin/login";

        /// <summary>
        /// 登录密码
        /// </summary>
        public static string AdminLoginPassword = "a123456";

        /// <summary>
        /// 使用管理密码
        /// </summary>
        public static bool UsedManagerPassword = true;

        /// <summary>
        /// 管理密码
        /// </summary>
        public static string AdminManagerPassword = "a12346789";

        /// <summary>
        /// 强制密码过期
        /// </summary>
        public static bool ForcedPasswordExpiration = false;

        /// <summary>
        /// 强制密码过期 天数
        /// </summary>
        public static int ForcedPasswordExpirationDays = 7;
    }

}
