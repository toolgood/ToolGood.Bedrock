using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using ToolGood.Bedrock.Dependency;
using ToolGood.Bedrock.Dependency.AutofacContainer;
using ToolGood.Bedrock.Internals;
using ToolGood.Bedrock.Web.Loggers;
using ToolGood.Bedrock.Web.Middlewares;

namespace ToolGood.Bedrock.Web
{

    public abstract class StartupBase
    {
        public IConfiguration Configuration { get; }
        public IContainer AutofacContainer;


        public StartupBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected abstract MyConfig GetMyConfig();

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var config = GetMyConfig();
            if (config.UseResponseCaching) { services.AddResponseCaching(); }
            if (config.UseResponseCompression) {
                services.AddResponseCompression(options => {
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
                });
            }
            if (config.UseSession) {
                services.AddDistributedMemoryCache();
                services.AddSession(options => {
                    options.Cookie.Name = "sid";
                    options.IdleTimeout = TimeSpan.FromHours(3);
                    options.Cookie.IsEssential = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
                });
                services.Configure<CookiePolicyOptions>(options => {
                    options.CheckConsentNeeded = context => false;
                    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
                    options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
                });
            } else if (config.UseCookie) {
                services.Configure<CookiePolicyOptions>(options => {
                    options.CheckConsentNeeded = context => false;
                    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
                });
            }
            if (config.UseIHttpContextAccessor) { services.AddHttpContextAccessor(); }
            if (config.UseMvc) {
                services.AddMvc(options => { options.Filters.Add<HttpGlobalExceptionFilter>(); })
                  .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                  .AddJsonOptions(options => {
                      options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                      options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                      options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                      options.SerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
                      options.SerializerSettings.Converters.Add(new JsonCustomDoubleConvert());// json序列化时， 防止double，末尾出现小数点浮动,
                      options.SerializerSettings.Converters.Add(new JsonCustomDoubleNullConvert());// json序列化时， 防止double，末尾出现小数点浮动,
                  }
              );
            }
            if (config.UseResumeFile) { services.AddMyResumeFileResult(); }
            if (config.UseCors) { services.AddCors(); }


            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            ServiceRegister(services);


            var builder = new ContainerBuilder();
            builder.Populate(services);
            IocManagerRegister(ContainerManager.UseAutofacContainer(builder));
            ContainerManager.BeginLeftScope();
            AutofacContainer = (ContainerManager.Instance.Container as AutofacObjectContainer).Container;
            return new AutofacServiceProvider(AutofacContainer);
        }
        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        public abstract void ServiceRegister(IServiceCollection services);

        /// <summary>
        /// IOC注册
        /// </summary>
        /// <param name="containerManager"></param>
        public abstract void IocManagerRegister(ContainerManager containerManager);


        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            MyHostingEnvironment.ApplicationName = env.ApplicationName;
            MyHostingEnvironment.ContentRootPath = env.ContentRootPath;
            MyHostingEnvironment.EnvironmentName = env.EnvironmentName;
            MyHostingEnvironment.WebRootPath = env.WebRootPath;
            MyHostingEnvironment.IsDevelopment = env.IsDevelopment();
            var config = GetMyConfig();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                if (config.UseHsts) {
                    app.UseHsts();
                }
            }
            app.UseStaticFiles();
            if (config.UseSession) { app.UseCookiePolicy(); app.UseSession(); } else if (config.UseCookie) { app.UseCookiePolicy(); }
            if (config.UseResponseCaching) { app.UseResponseCaching(); }
            if (config.UseResponseCompression) { app.UseResponseCompression(); }
            if (config.UseCors) { app.UseCors(); }

            app.UseEnableRequestRewind();
            app.UseQueryArgs();
            //app.UseRequestLogger();

            if (config.UseMvc) {
                app.UseMvc(routes => {
                    RouteRegister(routes);
                    routes.MapRoute(
                      name: "areas",
                      template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
            }
            if (config.UselLetsEncrypt) { app.UselMyLetsEncrypt(env); }


        }
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes"></param>
        public abstract void RouteRegister(IRouteBuilder routes);





    }
}
