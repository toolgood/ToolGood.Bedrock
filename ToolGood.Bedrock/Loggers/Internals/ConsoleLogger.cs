using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Bedrock.Internals
{
    public class ConsoleLogger : LoggerBase
    {
        /// <summary>是否使用多种颜色，默认使用</summary>
        public Boolean UseColor { get; set; } = true;

        public override void WriteLog(string type, string content)
        {
            if (type == "DebuggingMode") return;
            if (!UseColor) {
                Console.WriteLine(content);
                return;
            }
            ConsoleColor cc;

            switch (type) {
                case "Warn":
                    cc = ConsoleColor.Yellow;
                    break;
                case "Error":
                case "Fatal":
                    cc = ConsoleColor.Red;
                    break;
                default:
                    cc = ConsoleColor.Gray;
                    break;
            }
 
            var old = Console.ForegroundColor;
            Console.ForegroundColor = cc;
            Console.WriteLine(content);
            Console.ForegroundColor = old;

        }
    }
}
