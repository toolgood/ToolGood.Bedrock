using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;
using ToolGood.Bedrock.Dependency;
using ToolGood.Bedrock.Dependency.AutofacContainer;
using ToolGood.Bedrock.Web.Loggers;
using ToolGood.Bedrock.Web.Middlewares;
using ToolGood.Bedrock.Web.ResumeFiles.Executor;
using ToolGood.Bedrock.Web.ResumeFiles.ResumeFileResult;
using ToolGood.Bedrock.Web.Theme;
using IStartup = ToolGood.Bedrock.Web.Theme.IStartup;

namespace ToolGood.Bedrock.Web
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class StartupCore
    {
        #region _appConfigFiles _appConfigFiles2
        /// <summary>
        /// 固定路径
        /// </summary>
        private static readonly string[] _appConfigFiles = new string[] {
            "app.config",
            "App_Data/app.config",
            "App_Data/Config/app.config",
            "App_Data/Configs/app.config",
            "Config/app.config",
            "Configs/app.config",

            "appSettings.config",
            "App_Data/appSettings.config",
            "App_Data/Config/appSettings.config",
            "App_Data/Configs/appSettings.config",
            "Config/appSettings.config",
            "Configs/appSettings.config",

            "appSetting.config",
            "App_Data/appSetting.config",
            "App_Data/Config/appSetting.config",
            "App_Data/Configs/appSetting.config",
            "Config/appSetting.config",
            "Configs/appSetting.config",
        };
        /// <summary>
        /// 环境可变路径
        /// </summary>
        private static readonly string[] _appConfigFiles2 = new string[] {
            "app.{0}.config",
            "App_Data/app.{0}.config",
            "App_Data/Config/app.{0}.config",
            "App_Data/Configs/app.{0}.config",
            "Config/app.{0}.config",
            "Configs/app.{0}.config",

            "app_{0}.config",
            "App_Data/app_{0}.config",
            "App_Data/Config/app_{0}.config",
            "App_Data/Configs/app_{0}.config",
            "Config/app_{0}.config",
            "Configs/app_{0}.config",

            "appSettings.{0}.config",
            "App_Data/appSettings.{0}.config",
            "App_Data/Config/appSettings.{0}.config",
            "App_Data/Configs/appSettings.{0}.config",
            "Config/appSettings.{0}.config",
            "Configs/appSettings.{0}.config",

            "appSettings_{0}.config",
            "App_Data/appSettings_{0}.config",
            "App_Data/Config/appSettings_{0}.config",
            "App_Data/Configs/appSettings_{0}.config",
            "Config/appSettings_{0}.config",
            "Configs/appSettings_{0}.config",

            "appSetting.{0}.config",
            "App_Data/appSetting.{0}.config",
            "App_Data/Config/appSetting.{0}.config",
            "App_Data/Configs/appSetting.{0}.config",
            "Config/appSetting.{0}.config",
            "Configs/appSetting.{0}.config",

            "appSetting_{0}.config",
            "App_Data/appSetting_{0}.config",
            "App_Data/Config/appSetting_{0}.config",
            "App_Data/Configs/appSetting_{0}.config",
            "Config/appSetting_{0}.config",
            "Configs/appSetting_{0}.config",
        };

        private string GetAppConfigPath()
        {
            foreach (var file in _appConfigFiles) {
                var f = MyHostingEnvironment.MapPath(file);
                if (File.Exists(f)) { return f; }
            }

            string[] appEnvironments;
            if (System.Diagnostics.Debugger.IsAttached) { //调试时 先采用 开发文件
                appEnvironments = new string[] { MyHostingEnvironment.EnvironmentName, "dev", "test", "prod" };
            } else {
                appEnvironments = new string[] { MyHostingEnvironment.EnvironmentName, "prod", "test", "dev" };
            }
            foreach (var item in appEnvironments) {
                foreach (var file in _appConfigFiles2) {
                    var f = MyHostingEnvironment.MapPath(string.Format(file, item));
                    if (File.Exists(f)) { return f; }
                }
            }
            return null;
        }

        #endregion

        public virtual IConfiguration Configuration { get; }
        public virtual IContainer AutofacContainer { get; private set; }

        public StartupCore(IWebHostEnvironment env)
        {
            MyHostingEnvironment.ApplicationName = env.ApplicationName;
            MyHostingEnvironment.ContentRootPath = env.ContentRootPath;
            MyHostingEnvironment.EnvironmentName = env.EnvironmentName;
            MyHostingEnvironment.WebRootPath = env.WebRootPath;
            MyHostingEnvironment.IsDevelopment = env.IsDevelopment();


            var builder = new ConfigurationBuilder()

                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
            var path = GetAppConfigPath();
            if (string.IsNullOrEmpty(path)==false) {
                builder.AddXmlFile(path, true, false);
            }
            Configuration = builder.Build();
            //var builder = new ConfigurationBuilder()
            //   .SetBasePath(env.ContentRootPath)
            //   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //   .AddEnvironmentVariables();
            //Configuration = builder.Build();
            //services.AddSingleton<IConfiguration>(Configuration);

            //Configuration = configuration;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IConfiguration>(Configuration);
        
            var hostingEnvironment = services.BuildServiceProvider().GetService<IWebHostEnvironment>();

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
                    options.IOTimeout = TimeSpan.FromSeconds(1);
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
                var mvcBuilder = services.AddControllers(options => {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                })
                  .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                  .AddDataAnnotationsLocalization()
                 
                  .AddJsonOptions(options => {
                      options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);//System.Text.Json 序列化中文时的编码问题
                      options.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Allow; // 忽略注释
                      options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase; // 首字母小写
                      options.JsonSerializerOptions.WriteIndented = false;  // 不格式化json字符串 
                      options.JsonSerializerOptions.AllowTrailingCommas = true;  // 可以结尾有逗号
                      options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;  // 忽略只读属性
                      options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;  // 忽略大小写
                  }
              );
            }
            services.TryAddSingleton<IActionResultExecutor<ResumePhysicalFileResult>, ResumePhysicalFileResultExecutor>();
            services.TryAddSingleton<IActionResultExecutor<ResumeVirtualFileResult>, ResumeVirtualFileResultExecutor>();
            services.TryAddSingleton<IActionResultExecutor<ResumeFileStreamResult>, ResumeFileStreamResultExecutor>();
            services.TryAddSingleton<IActionResultExecutor<ResumeFileContentResult>, ResumeFileContentResultExecutor>();

            if (config.UseCors) { services.AddCors(options => { options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()); }); }

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            ServiceRegister(services);
            if (config.UsePlugin) { foreach (var startup in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetImplementationsOf<IStartup>()) { startup.ConfigureServices(services, Configuration); } }

            var builder = new ContainerBuilder();
            builder.Populate(services);
            IocManagerRegister(ContainerManager.UseAutofacContainer(builder));
            if (config.UsePlugin) { foreach (var startup in AppLoader.Instance(hostingEnvironment).AppAssemblies.GetImplementationsOf<IStartup>()) { startup.IocManagerRegister(ContainerManager.Instance); } }

            ContainerManager.BeginLeftScope();
            AutofacContainer = (ContainerManager.Instance.Container as AutofacObjectContainer).Container;
            return new AutofacServiceProvider(AutofacContainer);
        }

        public class JsonCustomDoubleConvert : IInputFormatter
        {
            public bool CanRead(InputFormatterContext context)
            {
                throw new NotImplementedException();
            }

            public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var config = GetMyConfig();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                if (config.UseHsts) {
                    app.UseHsts();
                }
            }
            if (config.UseResponseCompression) { app.UseResponseCompression(); }//使用压缩
            if (config.UseResponseCaching) { app.UseResponseCaching(); }//使用缓存

            #region UseStaticFiles
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings.Add(".properties", "application/octet-stream");
            provider.Mappings.Add(".bcmap", "application/octet-stream");
            MineRegister(provider);//注册Mine
            app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });
            if (config.UselLetsEncrypt) {
                var path = Path.Combine(env.ContentRootPath, ".well-known");
                if (Directory.Exists(path) == false) {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(Path.Combine(path, "Check"));//防止 .well-known 被删除
                }
                app.UseStaticFiles(new StaticFileOptions {
                    FileProvider = new PhysicalFileProvider(path),
                    RequestPath = new PathString("/.well-known"),
                    ServeUnknownFileTypes = true
                });
            }
            #endregion

            if (config.UsePlugin) { foreach (var startup in AppLoader.Instance(env).AppAssemblies.GetImplementationsOf<IStartup>()) { startup.Configure(app); } }
            if (config.UseSession) { app.UseCookiePolicy(); app.UseSession(); } //使用session
            else if (config.UseCookie) { app.UseCookiePolicy(); }//使用cookie
            if (config.UseCors) { app.UseCors(); }

            app.UseEnableRequestRewind();
            ApplicationRegister(app);

            if (config.UseMvc) {
                if (config.UseTheme) {
                    app.UseMiddleware<ThemeMiddleware>();
                }
                app.UseEndpoints(endpoints => {
                    endpoints.MapRazorPages();
                    endpoints.MapControllerRoute(
                        name: "areas",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                     );
                    endpoints.MapControllerRoute(
                       name: "imageDefaultC",
                       pattern: "Image/C{w:int}x{h:int}/{*u}",
                       defaults: new { controller = "Image", action = "C", });
                    endpoints.MapControllerRoute(
                        name: "imageDefaultH",
                        pattern: "Image/H{h:int}/{*u}",
                        defaults: new { controller = "Image", action = "H", });
                    endpoints.MapControllerRoute(
                        name: "imageDefaultW",
                        pattern: "Image/W{w:int}/{*u}",
                        defaults: new { controller = "Image", action = "W", });
                    endpoints.MapControllerRoute(
                        name: "imageDefaultT",
                        pattern: "Image/T{w:int}x{h:int}/{*u}",
                        defaults: new { controller = "Image", action = "T", });
                    endpoints.MapControllerRoute(
                        name: "imageDefaultQR",
                        pattern: "Image/qr{w:int}x{h:int}/{*u}",
                        defaults: new { controller = "Image", action = "QR", });

                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home:exists}/{action=Index}/{id?}");
                });
                 
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract MyConfig GetMyConfig();

        /// <summary>
        /// Mine 注册
        /// </summary>
        /// <param name="provider"></param>
        protected virtual void MineRegister(FileExtensionContentTypeProvider provider) { }

        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        protected virtual void ServiceRegister(IServiceCollection services) { }


        /// <summary>
        /// IOC注册
        /// </summary>
        /// <param name="containerManager"></param>
        protected virtual void IocManagerRegister(ContainerManager containerManager) { }

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes"></param>
        protected virtual void RouteRegister(IRouteBuilder routes) { }

        /// <summary>
        /// app注册 
        /// </summary>
        /// <param name="app"></param>
        protected virtual void ApplicationRegister(IApplicationBuilder app) { }
    }
}
