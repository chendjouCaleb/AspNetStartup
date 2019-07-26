using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Everest.AspNetStartup.Core.Extensions
{
    public static class ActionContextExtensions
    {
        public static string GetParameter(this ActionContext actionContext, string name, ParameterSource source = ParameterSource.Route)
        {
            if(source == ParameterSource.Route)
            {
                return actionContext.RouteData.Values[name] as string;
            }

            HttpRequest request = actionContext.HttpContext.Request;

            if (source == ParameterSource.Form)
            {
                if (!string.IsNullOrEmpty(request.ContentType) &&
                (request.ContentType.StartsWith("application/x-www-form-urlencoded")
                || request.ContentType.StartsWith("application/form-data")))
                {
                    return actionContext.HttpContext.Request.Form[name];
                }
                else
                {
                    throw new InvalidOperationException("The request don't have a 'form' content type");
                }
            }

            
            if (source == ParameterSource.Query)
            {
                return request.Query[name];
            }

            if(source == ParameterSource.Header)
            {
                return request.Headers[name];
            }

            return null;
        }

        
    }
}
