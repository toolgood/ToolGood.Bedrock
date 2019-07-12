using System.IO;

namespace ToolGood.Bedrock.Web
{
    /// <summary>
    /// 对应asp.net的 HostingEnvironment
    /// </summary>
    public static class MyHostingEnvironment
    {
        internal static bool IsDevelopment { get; set; }

        internal static string EnvironmentName { get; set; }

        internal static string ApplicationName { get; set; }

        internal static string WebRootPath { get; set; }

        internal static string ContentRootPath { get; set; }
        /// <summary>
        /// 获取文件物理路径  
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            return IsAbsolute(path) ? path : Path.Combine(ContentRootPath, path.TrimStart('~', '/').Replace("/", Path.DirectorySeparatorChar.ToString()));
        }
        /// <summary>
        /// 以WebRoot为根目录获取文件物理路径  
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MapWebRootPath(string path)
        {
            return IsAbsolute(path) ? path : Path.Combine(WebRootPath, path.TrimStart('~', '/').Replace("/", Path.DirectorySeparatorChar.ToString()));
        }
        private static bool IsAbsolute(string path)
        {
            return Path.VolumeSeparatorChar == ':' ? path.IndexOf(Path.VolumeSeparatorChar) > 0 : path.IndexOf('\\') > 0;
        }

    }
}
