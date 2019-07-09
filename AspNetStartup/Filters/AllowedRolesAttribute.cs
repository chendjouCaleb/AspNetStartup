using Everest.AspNetStartup.Core.Exceptions;
using Everest.AspNetStartup.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Everest.AspNetStartup.Filters
{
    public class AllowedRolesAttribute : Attribute, IActionFilter
    {
        public string[] Roles { get; set; }

        public AllowedRolesAttribute(string[] roles)
        {
            Roles = roles;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            User user = context.HttpContext.Items["user"] as User;

            bool hasRole = false;

            foreach(string role in Roles)
            {
                if (user.HasRole(role))
                {
                    hasRole = true;
                }
            }

            if (!hasRole)
            {
                throw new UnauthorizedException("Vous n'avez pas le role pour effectuer cette action");
            }
        }
    }
}
