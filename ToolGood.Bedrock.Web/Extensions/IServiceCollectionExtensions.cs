using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ToolGood.Bedrock.Web
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 生命周期
        /// </summary>
        public enum LifeStyle
        {
            /// <summary>
            /// 默认
            /// </summary>
            Transient = 1,
            /// <summary>
            /// 单例
            /// </summary>
            Singleton,
            /// <summary>
            /// 在一个生命周期域中，每一个依赖或调用创建一个单一的共享的实例，且每一个不同的生命周期域，实例是唯一的，不共享的。
            /// </summary>
            PerLifetimeScope
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifeStyle">生命周期</param>
        /// <returns></returns>
        public static IServiceCollection RegisterType(this IServiceCollection service, Type implementationType, LifeStyle lifeStyle = LifeStyle.PerLifetimeScope)
        {
            switch (lifeStyle) {
                case LifeStyle.Transient: service.AddTransient(implementationType); break;
                case LifeStyle.Singleton: service.AddSingleton(implementationType); break;
                case LifeStyle.PerLifetimeScope: service.AddScoped(implementationType); break;
                default: break;
            }
            return service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifeStyle">生命周期</param>
        /// <returns></returns>
        public static IServiceCollection RegisterType(this IServiceCollection service, Type serviceType, Type implementationType, LifeStyle lifeStyle = LifeStyle.PerLifetimeScope)
        {
            switch (lifeStyle) {
                case LifeStyle.Transient: service.AddTransient(serviceType, implementationType); break;
                case LifeStyle.Singleton: service.AddSingleton(serviceType, implementationType); break;
                case LifeStyle.PerLifetimeScope: service.AddScoped(serviceType, implementationType); break;
                default: break;
            }
            return service;
        }


        /// <summary>
        /// 根据程序集注册
        /// </summary>
        /// <param name="service"></param>
        /// <param name="assemblyName">程序集</param>
        /// <param name="predicate">筛选条件</param>
        /// <param name="lifeStyle">生命周期</param>
        public static IServiceCollection RegisterAssemblyTypes(this IServiceCollection service, string assemblyName, Func<Type, bool> predicate = null, LifeStyle lifeStyle = LifeStyle.Singleton)
        {
            var assembly = Assembly.Load(assemblyName);
            var types = assembly.GetTypes();
            foreach (var implementationType in types) {
                if (implementationType.IsAbstract) { continue; }
                if (implementationType.IsInterface) { continue; }
                if (implementationType.IsImport) { continue; }
                if (implementationType.IsClass == false) { continue; }
                if (implementationType.IsGenericType) { continue; }


                if (predicate != null) {
                    if (predicate(implementationType) == false) {
                        continue;
                    }
                }
                var interfacesTypes = implementationType.GetInterfaces();
                if (interfacesTypes.Length == 1) {
                    switch (lifeStyle) {
                        case LifeStyle.Transient: service.AddTransient(interfacesTypes[0], implementationType); break;
                        case LifeStyle.Singleton: service.AddSingleton(interfacesTypes[0], implementationType); break;
                        case LifeStyle.PerLifetimeScope: service.AddScoped(interfacesTypes[0], implementationType); break;
                        default: break;
                    }
                } else {
                    switch (lifeStyle) {
                        case LifeStyle.Transient: service.AddTransient(implementationType); break;
                        case LifeStyle.Singleton: service.AddSingleton(implementationType); break;
                        case LifeStyle.PerLifetimeScope: service.AddScoped(implementationType); break;
                        default: break;
                    }
                }

            }
            return service;
        }

        /// <summary>
        /// 根据程序集注册
        /// </summary>
        /// <param name="service"></param>
        /// <param name="assembly">程序集</param>
        /// <param name="predicate">筛选条件</param>
        /// <param name="lifeStyle">生命周期</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAssemblyTypes(this IServiceCollection service, Assembly assembly, Func<Type, bool> predicate = null, LifeStyle lifeStyle = LifeStyle.PerLifetimeScope)
        {
            var types = assembly.GetTypes();
            foreach (var implementationType in types) {
                if (implementationType.IsAbstract) { continue; }
                if (implementationType.IsInterface) { continue; }
                if (implementationType.IsImport) { continue; }
                if (implementationType.IsClass == false) { continue; }
                if (implementationType.IsGenericType) { continue; }

                if (predicate != null) {
                    if (predicate(implementationType) == false) {
                        continue;
                    }
                }
                var interfacesTypes = implementationType.GetInterfaces();
                if (interfacesTypes.Length == 1) {
                    switch (lifeStyle) {
                        case LifeStyle.Transient: service.AddTransient(interfacesTypes[0], implementationType); break;
                        case LifeStyle.Singleton: service.AddSingleton(interfacesTypes[0], implementationType); break;
                        case LifeStyle.PerLifetimeScope: service.AddScoped(interfacesTypes[0], implementationType); break;
                        default: break;
                    }
                } else {
                    switch (lifeStyle) {
                        case LifeStyle.Transient: service.AddTransient(implementationType); break;
                        case LifeStyle.Singleton: service.AddSingleton(implementationType); break;
                        case LifeStyle.PerLifetimeScope: service.AddScoped(implementationType); break;
                        default: break;
                    }
                }
            }
            return service;
        }


    }
}

