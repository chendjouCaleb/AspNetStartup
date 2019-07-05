using Everest.AspNetStartup.Core.Extensions;
using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Everest.AspNetStartup.Filters
{
    public class LoadConnectionAttribute : Attribute, IResourceFilter
    {
        public string ItemName { get; set; } = "connection";
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            IRepository<Connection, long> repository =
                context.HttpContext.RequestServices.GetRequiredService<IRepository<Connection, long>>();
            string id = context.GetParameter("connectionId");

            Connection connection = repository.Find(long.Parse(id));

            context.HttpContext.Items[ItemName] = connection;

        }

      
    }
}
