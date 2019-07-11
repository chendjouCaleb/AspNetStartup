using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Everest.AspNetStartup.Core.Extensions
{
    public static class ActionContextExtensions
    {
        public static string GetParameter(this ActionContext actionContext, string name)
        {
            string parameter = actionContext.RouteData.Values[name] as string;

            HttpRequest request = actionContext.HttpContext.Request;
            if (parameter == null)
            {
                parameter = request.Query[name];
            }

            if(parameter == null && !string.IsNullOrEmpty(request.ContentType) &&
                (request.ContentType.StartsWith("application/x-www-form-urlencoded")
                || request.ContentType.StartsWith("application/form-data")))
            {
                parameter = actionContext.HttpContext.Request.Form[name];
            }

            return parameter;
        }
    }
}
