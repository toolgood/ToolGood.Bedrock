using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToolGood.Bedrock.Extensions
{
    public static class TypeUtil
    {
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullName">命名空间.类型名</param>
        /// <param name="assemblyName">程序集</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string fullName, string assemblyName)
        {
            string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            Type o = Type.GetType(path);//加载类型
            object obj = Activator.CreateInstance(o, true);//根据类型创建实例
            return (T)obj;//类型转换并返回
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string assemblyName, string nameSpace, string className)
        {
            string fullName = nameSpace + "." + className;//命名空间.类型名
                                                          //此为第一种写法
#if !NET45
            //object ect = Assembly.Load(new AssemblyName(assemblyName)).CreateInstance(fullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例s

            //.net core 2.1这种方法也已经支持
            object ect = Assembly.Load(assemblyName).CreateInstance(fullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例s
#else
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例
#endif
            return (T)ect;//类型转换并返回
        }

        /// <summary>
        /// 获取静态类属性
        /// </summary>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <param name="memberName">属性名称（忽略大小写）</param>
        /// <returns></returns>
        public static object GetStaticMember(string assemblyName, string nameSpace, string className, string memberName)
        {
            string fullName = nameSpace + "." + className;//命名空间.类型名
            string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            var type = Type.GetType(path);
            PropertyInfo[] props = type.GetProperties();
            var prop = props.FirstOrDefault(z => z.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
            return prop.GetValue(null, null);
        }

        /// <summary>
        /// 获取静态类属性
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="memberName">属性名称（忽略大小写）</param>
        /// <returns></returns>
        public static object GetStaticMember(Type type, string memberName)
        {
            PropertyInfo[] props = type.GetProperties();
            var prop = props.FirstOrDefault(z => z.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
            return prop.GetValue(null, null);
        }
    }
}
