using Everest.AspNetStartup.Controllers;
using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Entities;
using Everest.AspNetStartup.Models;
using Everest.IdentityTest;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Everest.AspNetStartupTests.Controlleurs
{
    public class RoleControllerTest
    {
        private RoleController controller;
        private IRepository<User, string> userRepository;
        private IRepository<Role, string> roleRepository;
        private IRepository<UserRole, long> userRoleRepository;
        private User user;

        [SetUp]
        public void BeforeEach()
        {
            IServiceCollection serviceCollection = ServiceConfiguration.InitServiceCollection();
            IServiceProvider serviceProvider = ServiceConfiguration.BuildServiceProvider();


            controller = serviceProvider.GetRequiredService<RoleController>();
            userRepository = serviceProvider.GetRequiredService<IRepository<User, string>>();
            roleRepository = serviceProvider.GetRequiredService<IRepository<Role, string>>();
            userRoleRepository = serviceProvider.GetRequiredService<IRepository<UserRole, long>>();

            var model = new AdduserModel
            {
                Email = "chendjou@email.com",
                Name = "caleb",
                Surname = "deGrace",
                Password = "password"
            };
            var userController = serviceProvider.GetRequiredService<UserController>();
            user = userController.Create(model);
        }

        [Test]
        public void AddRole()
        {
            Role roleModel = new Role { Name = "role name", Description = "description role" };

            Role role = controller.Add(roleModel);

            roleRepository.Refresh(role);
            Assert.AreEqual(roleModel.Name, role.Name);
            Assert.AreEqual(roleModel.Description, role.Description);
        }

        [Test]
        public void TryAdd_TwoRole_WithSameName()
        {
            Role roleModel = new Role { Name = "role name", Description = "description role" };

            controller.Add(roleModel);

            Exception ex = Assert.Throws<InvalidOperationException>(() => controller.Add(roleModel));
            Assert.AreEqual("Vous ne pouvez ajouter le même role deux fois", ex.Message);
        }

        [Test]
        public void AddUserRole()
        {
            Role roleModel = new Role { Name = "role name", Description = "description role" };
            Role role = controller.Add(roleModel);

            UserRole userRole = controller.AddUserRole(role, user);

            userRoleRepository.Refresh(userRole);

            Assert.AreEqual(role, userRole.Role);
            Assert.AreEqual(user, userRole.User);
        }

        [Test]
        public void TryAddSameUserRoleTwoTime()
        {
            Role roleModel = new Role { Name = "role name", Description = "description role" };
            Role role = controller.Add(roleModel);

            controller.AddUserRole(role, user);
            Exception ex = Assert.Throws<InvalidOperationException>(() => controller.AddUserRole(role, user));
            Assert.AreEqual("Vous ne pouvez ajouter le même role deux fois à un même utilisateur", ex.Message);
        }

        [Test]
        public void DeleteUserRole()
        {
            Role roleModel = new Role { Name = "role name", Description = "description role" };
            Role role = controller.Add(roleModel);

            UserRole userRole = controller.AddUserRole(role, user);

            controller.DeleteUserRole(userRole.Id);

            Assert.False(userRoleRepository.Exists(userRole));
        }
    }
}
