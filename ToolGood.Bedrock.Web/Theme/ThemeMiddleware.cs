using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
 
namespace ToolGood.Bedrock.Web.Theme
{
    public class ThemeMiddleware
    {
        private readonly RequestDelegate _next;

        public ThemeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Request.HttpContext.Items[ViewLocationExpander.ThemeKey] = "Default";
            return _next(context);
        }

    }
}
