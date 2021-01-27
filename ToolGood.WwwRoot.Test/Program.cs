using System;

namespace ToolGood.WwwRoot.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //{
            //    WwwRootSetting setting = new WwwRootSetting();
            //    setting.NameSpace = "ToolGood.WwwRoot.Test";
            //    setting.InFolderPath = @"F:\git\ToolGood.Bedrock\ToolGood.Bedrock.Web.Test\wwwroot\";
            //    setting.OutFolderPath = @"F:\git\ToolGood.Bedrock\ToolGood.Bedrock.Web30.Test\Controllers\wwwroot";
            //    //setting.CompressionType = "gzip";

            //    setting.ExcludeFileSuffixs.Add(".old.js");
            //    setting.ExcludeFileSuffixs.Add(".map");
            //    setting.BuildControllers();
            //}
            {
                WwwRootSetting setting = new WwwRootSetting();
                setting.NameSpace = "ToolGood.TextFilter.Controllers";
                setting.InFolderPath = @"F:\somain\ToolGood.TextFilter\ToolGood.TextFilter.Website\wwwroot";
                setting.OutFolderPath = @"F:\somain\ToolGood.TextFilter\ToolGood.TextFilter.Website\Controllers\wwwroot";
                //setting.CompressionType = "gzip";

                setting.ExcludeFileSuffixs.Add(".old.js");
                setting.ExcludeFileSuffixs.Add(".map");
                setting.BuildControllers();
            }

#if Release

#endif

            //  F:\somain\ToolGood.TextFilter\ToolGood.TextFilter.Windows\wwwroot

            Console.WriteLine("Hello World!");
        }
    }
}
