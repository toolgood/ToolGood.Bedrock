using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ToolGood.Bedrock.Utils.IdGenerates
{
    public sealed class GuidGenerate_1
    {
        private static readonly DateTime dt1970 = new DateTime(1970, 1, 1);
        private static readonly Random rnd = new Random();
        private static readonly int __staticMachine = ((0x00ffffff & Environment.MachineName.GetHashCode()) + AppDomain.CurrentDomain.Id) & 0x00ffffff;
        private static readonly int __staticPid = Process.GetCurrentProcess().Id;
        private static int __staticIncrement = rnd.Next();

        /// <summary>
        /// 生成类似Mongodb的ObjectId有序、不重复Guid
        /// https://www.cnblogs.com/yushuo/p/9406906.html 回复2楼
        /// </summary>
        /// <returns></returns>
        public static Guid Generate()
        {
            var now = DateTime.Now;
            var uninxtime = (int)now.Subtract(dt1970).TotalSeconds;
            int increment = Interlocked.Increment(ref __staticIncrement) & 0x00ffffff;
            var rand = rnd.Next(0, int.MaxValue);
            var guid = $"{uninxtime.ToString("x8").PadLeft(8, '0')}{__staticMachine.ToString("x8").PadLeft(8, '0').Substring(2, 6)}{__staticPid.ToString("x8").PadLeft(8, '0').Substring(6, 2)}{increment.ToString("x8").PadLeft(8, '0')}{rand.ToString("x8").PadLeft(8, '0')}";
            return Guid.Parse(guid);
        }

    }
}
