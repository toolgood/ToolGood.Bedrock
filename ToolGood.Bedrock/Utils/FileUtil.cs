using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Bedrock.Utils
{
    public static class FileUtil
    {
        /// <summary>
        /// 判断文件是否正在被使用
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool FileInUse(string filePath)
        {
            try {
                if (!System.IO.File.Exists(filePath)) // The path might also be invalid.
                {
                    return false;
                }

                using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open)) {
                    return false;
                }
            } catch {
                return true;
            }
        }
    }
}
