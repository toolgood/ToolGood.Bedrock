using System.Collections.Generic;

namespace ToolGood.Bedrock.Web.Constants
{
    public static class FileConstants
    {
        public static List<string> Video {
            get { return new List<string> { ".mp4", ".avi", ".rmvb" }; }
        }
        public static List<string> Zip {
            get { return new List<string> { ".rar", ".zip", ".7z" }; }
        }
        public static List<string> Pdf {
            get { return new List<string> { ".pdf" }; }
        }
        public static List<string> Txt {
            get { return new List<string> { ".txt" }; }
        }
        public static List<string> Doc {
            get { return new List<string> { ".doc", ".docx" }; }
        }
        public static List<string> Excel {
            get { return new List<string> { ".xls", ".xlsx" }; }
        }
        public static List<string> Ppt {
            get { return new List<string> { ".ppt", ".pptx" }; }
        }


        public static string RootPath = null;

        public static string ImagePath = "/upload/images/";
        public static string FilePath = "/upload/files/";
        public static string VideoPath = "/upload/videos/";

        public static long ImageMaxSize = 5 * 1024 * 1024;
        public static long VideoMaxSize = 102400000;
        public static long FileMaxSize = 51200000;

        public static readonly List<string> ImageAllowExt = new List<string>()
        {
            ".jpg", ".jpe" ,".jpeg", ".png", ".gif", ".bmp", ".svg"
        };
        public static readonly List<string> VideoAllowExt = new List<string>()
        {
            ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg", ".ogg",
            ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid"
        };
        public static readonly List<string> FileAllowExt = new List<string>() {
            ".jpg", ".jpe" ,".jpeg", ".png", ".gif", ".bmp", ".svg",
            ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
            ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid",
            ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".cab", ".iso",
            ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md", ".xml"
        };
        public static readonly List<string> DownloadAllowExt = new List<string>() {
            ".exe",
            ".jpg", ".jpe" ,".jpeg", ".png", ".gif", ".bmp", ".svg",
            ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
            ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid",
            ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".cab", ".iso",
            ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md", ".xml"
        };

    }

}
