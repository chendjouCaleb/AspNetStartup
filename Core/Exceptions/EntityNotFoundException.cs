using Everest.AspNetStartup.Core.ExceptionTransformers;
using System;

namespace Everest.AspNetStartup.Core
{
    [ResponseStatusCode(StatusCode = 404)]
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException() : base("L'élément demandé est introuvable")
        { }
        public EntityNotFoundException(string message) : base(message)
        { }
    }
}
