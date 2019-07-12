using Mapster;
using System;

namespace System
{
    /// <summary>
    /// 对象映射
    /// </summary>
    public static partial class ObjectExtension
    {
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapTo<TDestination>(this object source)
        {
            return source.Adapt<TDestination>();
        }

        /// <summary>
        /// 对象映射
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="ignoreNames">忽略字段</param>
        /// <returns></returns>
        public static TDestination MapTo<TDestination>(this object source, params string[] ignoreNames)
        {
            if (source == null) { return default(TDestination); }
            var setting = new TypeAdapterConfig().NewConfig(source.GetType(), typeof(TDestination)).Ignore(ignoreNames);
            return source.Adapt<TDestination>(setting.Config);
        }

        /// <summary>
        /// 对象映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return source.Adapt<TDestination>();
        }

        /// <summary>
        /// 对象映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="ignoreNames">忽略字段</param>
        /// <returns></returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, params string[] ignoreNames)
        {
            var setting = TypeAdapterConfig<TSource, TDestination>.NewConfig().Ignore(ignoreNames);
            //var setting = new TypeAdapterConfig().NewConfig<TSource, TDestination>();
            //setting.Ignore(args);
            return source.Adapt<TDestination>(setting.Config);
        }

        /// <summary>
        /// 对象映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return source.Adapt<TSource, TDestination>(destination);
        }

        /// <summary>
        /// 对象映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="ignoreNames">忽略字段</param>
        /// <returns></returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination, params string[] ignoreNames)
        {
            var setting = TypeAdapterConfig<TSource, TDestination>.NewConfig().Ignore(ignoreNames);

            return source.Adapt<TSource, TDestination>(destination, setting.Config);
        }

        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static object MapTo(this object source, object destination, Type sourceType, Type destinationType)
        {
            return source.Adapt(destination, sourceType, destinationType);
        }

        /// <summary>
        /// 对象映射
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <param name="ignoreNames">忽略字段</param>
        /// <returns></returns>
        public static object MapTo(this object source, object destination, Type sourceType, Type destinationType, params string[] ignoreNames)
        {
            var setting = new TypeAdapterConfig().NewConfig(sourceType, destinationType).Ignore(ignoreNames);
            return source.Adapt(destination, sourceType, destinationType, setting.Config);
        }
    }
}