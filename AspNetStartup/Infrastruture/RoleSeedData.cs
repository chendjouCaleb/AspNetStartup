using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Entities;

namespace Everest.AspNetStartup.Infrastruture
{
    public class RoleSeedData
    {
        private IRepository<Role, string> roleRepository;

        public RoleSeedData(IRepository<Role, string> roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public void Seed()
        {
            foreach (var role in Roles)
            {
                if (!roleRepository.Exists(c => c.Name == role.Name))
                {
                    roleRepository.Save(role);
                }
            }
        }

        public Role[] Roles = new Role[]
        {
            new Role { Name = "ADMIN", Description = "Le super administrateur de l'application"}
        };
    }
}
