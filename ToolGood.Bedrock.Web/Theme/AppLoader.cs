using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

#if NETCOREAPP3_0
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
#elif NETCOREAPP2_2
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#endif



namespace ToolGood.Bedrock.Web.Theme
{
    public class AppLoader
    {
        private static AppLoader _instance;
        private static IHostingEnvironment _hostingEnvironment;
        private readonly IList<string> _loadedAssemblies = new List<string>();
        public IList<Assembly> AppAssemblies = new List<Assembly>();

 
        public static AppLoader Instance(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            return _instance ?? (_instance = new AppLoader());
        }

        private AppLoader()
        {
            var rootFolder = new DirectoryInfo(_hostingEnvironment.ContentRootPath);
            var appsRootFolder = new DirectoryInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "Apps"));

            foreach (var file in rootFolder.GetFiles("*.plugin.dll", SearchOption.TopDirectoryOnly)) {
                if (_loadedAssemblies.FirstOrDefault(x => x == file.Name) == null) {
                    _loadedAssemblies.Add(file.Name);
                }
            }
            foreach (var file in rootFolder.GetFiles("*.app.dll", SearchOption.TopDirectoryOnly)) {
                if (_loadedAssemblies.FirstOrDefault(x => x == file.Name) == null) {
                    _loadedAssemblies.Add(file.Name);
                }
            }


            if (appsRootFolder.Exists) {
                foreach (var appFolder in appsRootFolder.GetDirectories()) {
                    //AppDescriptors.Add(new AppDescriptor(appFolder.Name));

                    foreach (var file in appFolder.GetFiles("*.plugin.dll", SearchOption.TopDirectoryOnly)) {
                        if (_loadedAssemblies.FirstOrDefault(x => x == file.Name) != null)
                            continue;

                        _loadedAssemblies.Add(file.Name);

                        try {
                            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                            AppAssemblies.Add(assembly);
                        } catch (Exception) {
                            // ignored
                        }
                    }

                    foreach (var file in appFolder.GetFiles("*.app.dll", SearchOption.TopDirectoryOnly)) {
                        if (_loadedAssemblies.FirstOrDefault(x => x == file.Name) != null)
                            continue;

                        _loadedAssemblies.Add(file.Name);

                        try {
                            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                            AppAssemblies.Add(assembly);
                        } catch (Exception) {
                            // ignored
                        }
                    }
                }

            }
        }
    }

}
