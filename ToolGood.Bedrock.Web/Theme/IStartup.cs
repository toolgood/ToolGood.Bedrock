using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Bedrock.Dependency;

namespace ToolGood.Bedrock.Web.Theme
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration);
        void Configure(IApplicationBuilder builder);
        void IocManagerRegister(ContainerManager containerManager);

    }
}
