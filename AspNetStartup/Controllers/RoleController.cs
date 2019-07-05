using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Entities;
using Everest.AspNetStartup.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Everest.AspNetStartup.Controllers
{
    [Route("api/roles")]
    public class RoleController:Controller
    {
        private IRepository<User, string> userRepository;
        private IRepository<Role, string> roleRepository;
        private IRepository<UserRole, long> userRoleRepository;

        public RoleController(IRepository<User, string> userRepository, IRepository<Role, string> roleRepository, 
            IRepository<UserRole, long> userRoleRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
        }

        [HttpGet]
        private IEnumerable<Role> List()
        {
            return roleRepository.List();
        }


        [HttpGet("{roleId}")]
        public Role Find(string roleId) => roleRepository.Find(roleId);

      
        public Role Add(Role role)
        {
            if(roleRepository.Exists(r => r.Name == role.Name))
            {
                throw new InvalidOperationException("Vous ne pouvez ajouter le même role deux fois");
            }

            return roleRepository.Save(role);
        }


        [HttpPost("users")]
        public UserRole AddUserRole([FromBody] AddUserRoleModel model)
        {
            Role role = roleRepository.Find(model.RoleId);
            User user = userRepository.Find(model.UserId);

            return AddUserRole(role, user);
        }

        public UserRole AddUserRole(Role role, User user)
        {
            if(userRoleRepository.Exists(ur => ur.Role.Equals(role)&& ur.User.Equals(user)))
            {
                throw new InvalidOperationException("Vous ne pouvez ajouter le même role deux fois à un même utilisateur");
            }
            UserRole userRole = new UserRole { Role = role, User = user };

            userRole = userRoleRepository.Save(userRole);

            return userRole;
        }

        [HttpDelete("users")]
        public StatusCodeResult DeleteUserRole(long userRoleId)
        {
            UserRole userRole = userRoleRepository.Find(userRoleId);

            userRoleRepository.Delete(userRole);

            return NoContent();
        }
    }
}
