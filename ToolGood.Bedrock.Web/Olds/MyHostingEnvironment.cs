using System.IO;

namespace ToolGood.Bedrock.Web
{
    public static class MyHostingEnvironment
    {
        internal static bool IsDevelopment { get; set; }

        internal static string EnvironmentName { get; set; }

        internal static string ApplicationName { get; set; }

        internal static string WebRootPath { get; set; }

        internal static string ContentRootPath { get; set; }

        public static string MapPath(string path)
        {
            return IsAbsolute(path) ? path : Path.Combine(ContentRootPath, path.TrimStart('~', '/').Replace("/", Path.DirectorySeparatorChar.ToString()));
        }
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
