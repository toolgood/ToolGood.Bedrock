using System.Data;
using System.Text;
using ToolGood.Bedrock.DataCommon;
using ToolGood.Bedrock.DataCommon.YamlToJson;

namespace ToolGood.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Admin admin1 = new Admin() { Name = "admin", Description = "123", IsFreeze3 = true };
            Admin admin2 = new Admin() { Id = 2, Name = "admin", Description = "123", IsFreeze = false };
            Admin admin3 = new Admin() { Id = 2, Name = "admin3", Description = "", Type = AdminType.T1, Roles = "1,2,3", IsFreeze = true, IsFreeze2 = true };
            Admin admin4 = new Admin() { Id = 2, Name = "admin3", Type = AdminType.T1, Date = new DateTime(2014, 1, 1), IsFreeze3 = true };

            var message0 = DataDiffHelper.Diff(admin1);
            var message1 = DataDiffHelper.Diff(admin1, admin2);
            var message2 = DataDiffHelper.Diff(admin2, admin3);
            var message3 = DataDiffHelper.Diff(admin3, admin4);

            List<string> list1 = new List<string>() { "admin", "group" };
            List<string> list2 = new List<string>() { "admin", "1", "2" };

            var lm1 = DataDiffHelper.Diff(list1, list2);

            List<int> ids1 = new List<int>() { 1, 2 };
            List<int> ids2 = new List<int>() { 1, 3, 4 };
            Dictionary<int, string> idDict = new Dictionary<int, string>();
            idDict[1] = "id1";
            idDict[2] = "id2";
            idDict[3] = "id3";
            idDict[4] = "id4";
            var idm1 = DataDiffHelper.Diff("ids", ids1, ids2);
            var idm2 = DataDiffHelper.Diff("ids", ids1, ids2, idDict);


            var j1 = ("{'id':1, 'foo':'bar'}");
            var j2 = ("{'id':1, 'foo':'baz'}");
            var jd1 = DataDiffHelper.JsonDiff(j1, j2);

            var ja1 = ("[{'id':1, 'foo':'bar'},{'id':2, 'foo':'bar3'},{'id':3, 'foo':'bark'}]");
            var ja2 = ("[{'id':2, 'foo':'bar3'},{'id':1, 'foo':'baz'},{'id':3, 'foo':'bar9'}]");


            var ja3 = ("[{'foo':'bar', 'id':1},{'id':2, 'foo':'bar3'},{'id':3, 'foo':'bark'}]");
            var ja4 = ("[{'foo':'bar3','id':2},{'id':1, 'foo':'baz'},{'id':3, 'foo':'bar9'},{  'foo':'bardddd'}]");

            var jad1 = DataDiffHelper.JsonDiff(ja1, ja2);
            var jad2 = DataDiffHelper.JsonDiff(ja3, ja4);


            var y1 = @"
aaa:ddd
bbb:ccc
";

            var y2 = @"
aaa:ddd2
ccc:sss
ddd:
   sddd:123
";
            var yd1 = DataDiffHelper.YamlDiff(y1, y2, Newtonsoft.Json.Formatting.Indented);

            var sb = new StringBuilder();
            sb.AppendLine("contentVersion: 1.0.0.0");
            sb.AppendLine("parameters: {}");
            sb.AppendLine("variables: {}");
            sb.AppendLine("variables11: ");
            sb.AppendLine("   variable2221: 12223");
            sb.AppendLine("   122: 你好");
            sb.AppendLine("resources: []");
            sb.AppendLine("outputs: {}");

            var yaml = sb.ToString();
            var json = StringHelper.ToJson(yaml);

            var sb2 = new StringBuilder();

            var json2 = StringHelper.ToJson(y1);

        }
    }
    //[DataName("账号表")]
    public class Admin
    {
        public int Id { get; set; }
        [DataName("账号")]
        public string Name { get; set; }

        [DataName("描述")]
        public string Description { get; set; }


        [DataName("类型")]
        public AdminType Type { get; set; }

        [DataEnum("角色", "", "管理员", "员工", "客户")]
        public string Roles { get; set; }

        [DataName("日期")]
        public DateTime? Date { get; set; }

        [DataName("冻结")]
        public bool? IsFreeze { get; set; }


        [DataEnum("冻结2", "", "已冻结")]
        public bool? IsFreeze2 { get; set; }

        [DataEnum("", "可用", "已冻结")]
        public bool? IsFreeze3 { get; set; }
    }


    public enum AdminType
    {
        None,
        [DataName("等级1")]
        T1,
        [DataName("等级2")]
        T2,
        [DataName("等级3")]
        T3,

    }
}
