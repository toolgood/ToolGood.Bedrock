using System;
using System.Collections.Generic;

namespace ToolGood.Bedrock
{
    public static class EnumUtil
    {

        /// <summary>
        /// 获取 枚举 键值对
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetDescriptions(Type type)
        {
            return EnumExtension.GetDescriptions(type);
        }


    }
}
