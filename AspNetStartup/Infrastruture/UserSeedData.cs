using Everest.AspNetStartup.Controllers;
using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Entities;
using Everest.AspNetStartup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Everest.AspNetStartup.Infrastruture
{
    public class UserSeedData
    {
        private IRepository<User, string> userRepository;
        private IRepository<Role, string> roleRepository;
        private UserController userController;
        private RoleController roleController;



        public void Seed()
        {
            if(userRepository.Exists(u => u.Email == "chendjou2016@outlook.fr")){
                return;
            }
            Role admin = roleRepository.First(r => r.Name == "ADMIN");
            AdduserModel model = new AdduserModel
            {
                Email = "chendjou2016@outlook.fr",
                Name = "Chendjou",
                Surname = "Caleb",
                Password = "123456"
            };

            User user = userController.Create(model);

            roleController.AddUserRole(admin, user);

        }
    }
}
