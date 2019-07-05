using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ToolGood.Bedrock.Web.Theme
{
    internal static class ReflectionExtensions
    {
        //public static bool HasProperty<T>(this T obj, string name)
        //    where T : class
        //{
        //    return obj.GetType().GetRuntimeProperty(name) != null;
        //}

        //public static bool HasValue<T>(this T obj, string name)
        //     where T : class
        //{
        //    var currentProperty = obj.GetType().GetRuntimeProperty(name);
        //    if (currentProperty == null)
        //        return false;
        //    var caurrentValue = currentProperty.GetValue(obj);
        //    var defaultValue = Activator.CreateInstance(obj.GetType()).GetType().GetRuntimeProperty(name).GetValue(obj);
        //    return caurrentValue != defaultValue;
        //}

        //public static void SetValue<T>(this T obj, string name, object value)
        //     where T : class
        //{
        //    obj.GetType().GetRuntimeProperty(name).SetValue(obj, value);
        //}

        //public static object GetValue<T>(this T obj, string name)
        // where T : class
        //{
        //    return obj.GetType().GetRuntimeProperty(name).GetValue(obj);
        //}


        public static IEnumerable<T> GetImplementationsOf<T>(this Assembly assembly)
        {
            var result = new List<T>();

            var types = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(T).IsAssignableFrom(t))
                .ToList();

            foreach (var type in types) {
                var instance = (T)Activator.CreateInstance(type);
                result.Add(instance);
            }

            return result;
        }

        public static IEnumerable<T> GetImplementationsOf<T>(this IEnumerable<Assembly> assemblies)
        {
            var result = new List<T>();

            foreach (var assembly in assemblies) {
                var types = assembly.GetTypes()
                    .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(T).IsAssignableFrom(t))
                    .ToList();

                foreach (var type in types) {
                    var instance = (T)Activator.CreateInstance(type);
                    result.Add(instance);
                }
            }

            return result;
        }


    }

}
