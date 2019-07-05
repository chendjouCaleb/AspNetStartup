using Everest.AspNetStartup.Core.Models;

namespace Everest.AspNetStartup.Entities
{
    public class UserRole: Entity<long>
    {
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual Role Role { get; set; }
        public string RoleId { get; set; }
    }
}
