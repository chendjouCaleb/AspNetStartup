using Everest.AspNetStartup.Core.Extensions;
using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Everest.AspNetStartup.Filters
{
    public class LoadUserAttribute: Attribute, IResourceFilter
    {
        public string ItemName { get; set; } = "user";
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            IRepository<User, string> repository =
                context.HttpContext.RequestServices.GetRequiredService<IRepository<User, string>>();
            string id = context.GetParameter("userId");
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            User user = repository.Find(id);

            context.HttpContext.Items[ItemName] = user;

        }
    }
}
