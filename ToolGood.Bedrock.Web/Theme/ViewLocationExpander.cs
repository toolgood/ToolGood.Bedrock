using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Bedrock.Web.Theme
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public const string ThemeKey = "Theme";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            string theme = "Default";
            if (context.ActionContext.HttpContext.Items.ContainsKey(ThemeKey)) {
                theme = context.ActionContext.HttpContext.Items[ThemeKey].ToString();
            }
            context.Values[ThemeKey] = theme;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string theme;

            if (context.Values.TryGetValue(ThemeKey, out theme)) {
                viewLocations = new[]
                {
                    $"/Themes/{theme}/{{1}}/{{0}}.cshtml",
                    $"/Themes/{theme}/Shared/{{0}}.cshtml",
                    $"/Themes/{theme}/{{2}}/{{1}}/{{0}}.cshtml",
                    $"/Themes/{theme}/{{2}}/Shared/{{0}}.cshtml",
                }
                .Concat(viewLocations);
            }

            return viewLocations;
        }
    }

}
