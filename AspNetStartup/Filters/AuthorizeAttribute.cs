﻿using Everest.AspNetStartup.Core.Exceptions;
using Everest.Identity.Infrastruture;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Everest.Identity.Filters
{
    /// <summary>
    /// Filtre pour vérifier qu'une requête HTTP contient toutes les autorizations.
    /// </summary>
    /// 

    public class AuthorizeAttribute : Attribute, IActionFilter
    {

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            AuthorizationResult result = context.HttpContext.Items["Authorization"] 
                as AuthorizationResult;

            if (!result.Successed)
            {
                throw new UnauthorizedException(result.Exception.Message);
            }
        }
    }
}
