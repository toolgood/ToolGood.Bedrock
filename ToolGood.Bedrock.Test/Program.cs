using System;
using System.Linq;
using ToolGood.Bedrock;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ToolGood.Bedrock.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            UserPassword password = new UserPassword();
            password.Data = new UserPassword.Dto() {
                //UserName="111",
                Ints = new List<int>() { 1, 2 },
                Actions = new List<UserPassword.Action>() {
                    new UserPassword.Action()
                }
            };
            int a = 2;
           

            var msg = password.CheckDate();
            var vvv = "".IsNotNull();




            //var t = 1.ToSafeEnum<aa>(aa.a);
            var ts = "123,123,,456".Split(",,");
            var ts2 = "123,123,,456".SplitChar(",,");

            var str = System.Text.Encoding.UTF8.GetBytes("我好像上次也没找出问题。。");
            var str2 = str.Reverse().ToArray();

            var b64 = Base64.ToBase64String(str);
            var b64_2 = Base64_test.ToBase62String(str2);
            var t1 = Base62_test.ToBase62String(str);
            //var t2_ = BaseUtil.Encode(str);

            var t2 = Base58_test.ToBase62String(str);
            var t2_2 = Base58_test.ToBase62String2(str);
            var t2_3 = Base58_test.ToBase62String3(str);
            var t2_4 = Base58_test.ToBase62String4(str);

            Console.WriteLine("Hello World!");
        }
    }
    public class UserPassword : EncryptedQueryArgs<UserPassword.Dto>
    {
        public class Dto
        {
            //[Required]
            [StringLength(20)]
            public string UserName { get; set; }

            public string Password { get; set; }

            public List<int> Ints { get; set; }

            public DateTime DateTime { get; set; }

            public List<Action> Actions { get; set; }
        }
        public class Action
        {
            [Required]

            public string Name { get; set; }
        }

    }


    public enum aa
    {
        a, b, c, d, e, f
    }


    public class Base64_test
    {
        public static string ToBase62String(byte[] input)
        {
            var baseEncoding = new BaseEncoding("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-", BaseEncoding.EndianFormat.Little);
            return baseEncoding.Encode(input);
        }
    }
    public class Base62_test
    {
        public static string ToBase62String(byte[] input)
        {
            var baseEncoding = new BaseEncoding("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
            return baseEncoding.Encode(input);
        }
    }
    public class Base58_test
    {
        public static string ToBase62String(byte[] input)
        {
            var baseEncoding = new BaseEncoding("123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz", BaseEncoding.EndianFormat.Little);
            return baseEncoding.Encode(input);
        }
        public static string ToBase62String2(byte[] input)
        {
            var baseEncoding = new BaseEncoding("123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz", BaseEncoding.EndianFormat.Big);
            return baseEncoding.Encode(input);
        }
        public static string ToBase62String3(byte[] input)
        {
            var baseEncoding = new BaseEncoding("123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz", BaseEncoding.EndianFormat.Little, true);
            return baseEncoding.Encode(input);
        }
        public static string ToBase62String4(byte[] input)
        {
            var baseEncoding = new BaseEncoding("123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz", BaseEncoding.EndianFormat.Big, true);
            return baseEncoding.Encode(input);
        }


    }



}
