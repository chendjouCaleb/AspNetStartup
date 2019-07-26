using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Everest.AspNetStartup.Core.Extensions
{
    /// <summary>
    /// Pour exiger que certains paramètres soit présents dans les paramètres de requêtes avant d'éxécuter l'action.
    /// </summary>
    public class RequireQueryParametersAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Les noms des paramètres exigés.
        /// </summary>
        public string[] Parameters { get; set; }

        public RequireQueryParametersAttribute(string[] Parameters)
        {
            this.Parameters = Parameters;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpContext httpContext = context.HttpContext;

            foreach (string parameter in Parameters)
            {
                if (httpContext.Request.Query[parameter] == StringValues.Empty)
                {
                    throw new ElementNotFoundException($"Le parametre {parameter} est absent paramètre de requête");
                }
            }
        }
    }




    /// <summary>
    /// Pour exiger qu'un paramètres soit présent dans les paramètres de requêtes avant d'éxécuter l'action.
    /// </summary>
    public class RequireQueryParameterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Les noms des paramètres exigés.
        /// </summary>
        public string Parameter { get; set; }

        public RequireQueryParameterAttribute(string Parameter)
        {
            this.Parameter = Parameter;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpContext httpContext = context.HttpContext;

            if (httpContext.Request.Query[Parameter] == StringValues.Empty)
            {
                throw new ElementNotFoundException($"Le parametre {Parameter} est absent paramètre de requête");
            }

        }
    }
}
