using System;

namespace ToolGood.WwwRoot.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            WwwRootSetting setting = new WwwRootSetting();
            setting.NameSpace = "ToolGood.WwwRoot.Test";
            setting.FolderPath = @"F:\git\ToolGood.Bedrock\ToolGood.Bedrock.Web30.Test\wwwroot\";
            setting.OutFolderPath = @"F:\git\ToolGood.Bedrock\ToolGood.Bedrock.Web30.Test\Controllers\wwwroot";
            setting.BuildControllers();


            Console.WriteLine("Hello World!");
        }
    }
}
