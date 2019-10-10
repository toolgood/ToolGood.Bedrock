using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
#if NETCOREAPP2_2
using ToolGood.Bedrock.Dependency;

#endif

namespace ToolGood.Bedrock.Web.Theme
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration);
        void Configure(IApplicationBuilder builder);
#if NETCOREAPP2_2
        void IocManagerRegister(ContainerManager containerManager);

#endif
    }
}
