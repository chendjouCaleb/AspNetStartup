using Everest.AspNetStartup.Core.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Everest.AspNetStartup.Entities
{
    public class Role : Entity<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public virtual List<UserRole> UserRoles { get; set; }
    }
}
