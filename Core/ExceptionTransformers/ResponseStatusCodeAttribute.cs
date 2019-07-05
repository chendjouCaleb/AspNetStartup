using System;

namespace Everest.AspNetStartup.Core.ExceptionTransformers
{
    public class ResponseStatusCodeAttribute: Attribute
    {
        public int StatusCode { get; set; }
    }
}
