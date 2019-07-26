using System;
using System.Collections.Generic;
using System.Text;

namespace Everest.AspNetStartup.Core
{
    public class UnsupportedOperationException:ApplicationException
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public object RouteValues { get; set; }

        public UnsupportedOperationException(string actionName, string controllerName, object routeValues)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            RouteValues = routeValues;
        }

        public UnsupportedOperationException(string actionName)
        {
            ActionName = actionName;
        }

        public UnsupportedOperationException(string actionName, string controllerName)
        {
            ActionName = actionName;
            ControllerName = controllerName;
        }

        public UnsupportedOperationException(string actionName, object routeValues)
        {
            ActionName = actionName;
            RouteValues = routeValues;
        }

        public UnsupportedOperationException() : base("Cette opération n'est pas supportée sur cette URL")
        { }
        
    }
}
