using Everest.AspNetStartup.Entities;
using System;

namespace Everest.Identity.Infrastruture
{
    public class AuthorizationResult
    {
        public bool Successed { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public Connection Connection { get; set; }
    }
}
