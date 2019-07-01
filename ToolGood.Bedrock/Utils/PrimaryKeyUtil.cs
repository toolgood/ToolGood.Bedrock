using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using ToolGood.Bedrock;
using ToolGood.Bedrock.Utils.IdGenerates;

namespace ToolGood.Bedrock
{
    public static class PrimaryKeyUtil
    {
        /// <summary>
        /// 生成类似Mongodb的ObjectId有序、不重复Guid
        /// https://www.cnblogs.com/yushuo/p/9406906.html 回复2楼
        /// </summary>
        /// <returns></returns>
        public static Guid GenerateGuid_1()
        {
            return GuidGenerate_1.Generate();
        }
        /// <summary>
        /// 获取有序的唯一ID。
        /// http://www.codeproject.com/Articles/388157/GUIDs-as-fast-primary-keys-under-multiple-database
        /// </summary>
        /// <param name="sequentialGuidType">有序GUID的类型（sqlServer用AtEnd，mysql用AsString或者AsBinary，oracle用AsBinary，postgresql用AsString或者AsBinary）</param>
        /// <returns></returns>
        public static Guid GenerateGuid_2(SequentialGuidType guidType = SequentialGuidType.SequentialAtEnd)
        {
            return GuidGenerate_2.Generate(guidType);
        }


        /// <summary>
        /// 设置LongGenerate ，使用 Singleton＜LongGenerate_1＞.Instance
        /// </summary>
        /// <returns></returns>
        public static long LongGenerate_1()
        {
            var longGenerate_2 = Singleton<LongGenerate_1>.Instance;
            return longGenerate_2.GetId();
        }


        /// <summary>
        /// 设置LongGenerate ，使用 Singleton＜LongGenerate_2＞.Instance
        /// </summary>
        /// <returns></returns>
        public static long LongGenerate_2()
        {
            var longGenerate_2 = Singleton<LongGenerate_2>.Instance;
            return longGenerate_2.NextId();
        }



    }
}
