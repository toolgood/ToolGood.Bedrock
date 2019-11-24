using System;

namespace ToolGood.WwwRoot.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            WwwRootSetting setting = new WwwRootSetting();
            setting.NameSpace = "ToolGood.WwwRoot.Test";
            setting.InFolderPath = @"F:\git\ToolGood.Bedrock\ToolGood.Bedrock.Web.Test\wwwroot\";
            setting.OutFolderPath = @"F:\git\ToolGood.Bedrock\ToolGood.Bedrock.Web30.Test\Controllers\wwwroot";
            //setting.CompressionType = "gzip";

            setting.ExcludeFileSuffixs.Add(".old.js");
            setting.ExcludeFileSuffixs.Add(".map");
            setting.BuildControllers();


            Console.WriteLine("Hello World!");
        }
    }
}
