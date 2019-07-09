using Everest.AspNetStartup.Core.Exceptions;
using Everest.AspNetStartup.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Everest.Identity.Filters
{
    /// <summary>
    /// Filtre pour vérifier que le compte authentifiée d'une requête
    /// HTTP est bien celui du compte présent dans les ressources.
    /// <see cref="Everest.Identity.Models.user"/>
    /// </summary>
    public class RequireuserOwner : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            User authuser = context.HttpContext.Items["Authorization.User"] as User;
            User user = context.HttpContext.Items["user"] as User;
            System.Console.WriteLine($"user Id = {user.Id}");
            System.Console.WriteLine($"Auth ccount Id = {authuser.Id}");

            if (user.Id != authuser.Id)
            {
                throw new UnauthorizedException("Le compte qui essaye d'accéder à la ressource n'est pas celui du compte de la ressource");
            }
        }
    }
}
