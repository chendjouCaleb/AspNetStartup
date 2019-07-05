using Microsoft.AspNetCore.Mvc;

namespace Everest.AspNetStartup.Core.Extensions
{
    public static class ActionContextExtensions
    {
        public static string GetParameter(this ActionContext actionContext, string name)
        {
            string parameter = actionContext.RouteData.Values[name] as string;

            if (parameter == null)
            {
                parameter = actionContext.HttpContext.Request.Query[name];
            }

            if(parameter == null)
            {
                parameter = actionContext.HttpContext.Request.Form[name];
            }

            return parameter;
        }
    }
}
