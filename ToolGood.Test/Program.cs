using System.Data;
using ToolGood.Bedrock.DataCommon;

namespace ToolGood.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Admin admin1 = new Admin() { Name = "admin", Description = "123" };
            Admin admin2 = new Admin() { Id = 2, Name = "admin", Description = "123" };
            Admin admin3 = new Admin() { Id = 2, Name = "admin3", Description = "", Type = AdminType.T1, Roles = "1,2,3" };
            Admin admin4 = new Admin() { Id = 2, Name = "admin3", Type = AdminType.T1,Date=new DateTime(2014,1,1) };

            var message1 = DataDiffHelper.Diff(admin1, admin2);
            var message2 = DataDiffHelper.Diff(admin2, admin3);
            var message3 = DataDiffHelper.Diff(admin3, admin4);

            List<string> list1=new List<string>() {"admin","group" };
            List<string> list2=new List<string>() {"admin","1","2" };

            var lm1 = DataDiffHelper.Diff(list1, list2);

            List<int> ids1 = new List<int>() { 1,2};
            List<int> ids2 = new List<int>() { 1,3,4};
            Dictionary<int, string> idDict = new Dictionary<int, string>();
            idDict[1] = "id1";
            idDict[2] = "id2";
            idDict[3] = "id3";
            idDict[4] = "id4";
            var idm1 = DataDiffHelper.Diff(ids1, ids2);
            var idm2 = DataDiffHelper.Diff(ids1, ids2, idDict);


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
