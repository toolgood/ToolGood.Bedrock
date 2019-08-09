using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToolGood.Bedrock.Dependency;

namespace ToolGood.Bedrock.Web.Test
{
    public class Startup : StartupCore
    {
        public Startup(IHostingEnvironment env) : base(env)
        {
        }

        protected override MyConfig GetMyConfig()
        {
            return new MyConfig() {
                //UseRsaDecrypt=true,

            };
        }
    }
}
