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
            Admin admin4 = new Admin() { Id = 2, Name = "admin3", Type = AdminType.T1 };

            var message1 = DiffHelper.Diff(admin1, admin2);
            var message2 = DiffHelper.Diff(admin2, admin3);
            var message3 = DiffHelper.Diff(admin3, admin4);

        }
    }
    [DataName("账号表")]
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
