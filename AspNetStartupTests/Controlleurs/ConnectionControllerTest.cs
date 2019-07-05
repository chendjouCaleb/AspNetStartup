using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using Microsoft.AspNetCore.Identity;
using Everest.AspNetStartup.Controllers;
using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Entities;
using Everest.AspNetStartup.Core;
using Everest.AspNetStartup.Models;

namespace Everest.IdentityTest.Controlleurs
{
    public class ConnectionControllerTest
    {
        private ConnectionController controller;
        private IRepository<Connection, long> connectionRepository;
        private IRepository<User, string> accountRepository;
        private LoginModel model;
        private User account;

        [SetUp]
        public void BeforeEach()
        {
            IServiceCollection serviceCollection = ServiceConfiguration.InitServiceCollection();
            IServiceProvider serviceProvider = ServiceConfiguration.BuildServiceProvider();


            controller = serviceProvider.GetRequiredService<ConnectionController>();
            accountRepository = serviceProvider.GetRequiredService<IRepository<User, string>>();
            connectionRepository = serviceProvider.GetRequiredService<IRepository<Connection, long>>();
            IPasswordHasher<User> passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<User>>();

            account = new User
            {
                Email = "account@email.com"
            };
            account.Password = passwordHasher.HashPassword(account, "password123");
            account = accountRepository.Save(account);

            model = new LoginModel
            {
                Email = "account@email.com",
                Password = "password123",
                OS = "Windows",
                Browser = "Firefox"
            };
        }

        [Test]
        public void CreateConnection()
        {
            Connection connection = controller.Login(model);

            Assert.True(connectionRepository.Exists(connection));
            
            Assert.AreEqual(model.OS, connection.OS);
            Assert.AreEqual(model.Browser, connection.Browser);
            Assert.AreEqual(model.RemoteAddress, connection.RemoteAddress);
            Assert.AreEqual(model.IsPersisted, connection.IsPersistent);


            Assert.AreEqual(account, connection.User);
            
            Assert.NotNull(connection.BeginDate);
            Assert.Null(connection.EndDate);
            Assert.False(connection.IsClosed);
        }

        [Test]
        public void Try_CreateConnection_WithUnusedEmail()
        {
            model.Email = "unusedEmail.com";

            Exception ex = Assert.Throws<EntityNotFoundException>(() => controller.Login(model));
        }

        [Test]
        public void Try_CreateConnection_WithNonAccountPassword()
        {
            model.Password = "fakePassword";

            Assert.Throws<InvalidValueException>(() => controller.Login(model));
        }

        [Test]
        public void CloseConnection()
        {
            Connection connection = controller.Login(model);

            controller.Logout(connection);
            Assert.True(connection.IsClosed);
            Assert.NotNull(connection.BeginDate);
            Assert.NotNull(connection.EndDate);
        }

        [Test]
        public void Try_CloseClosedConnection()
        {
            Connection connection = controller.Login(model);

            controller.Logout(connection);

            Assert.Throws<InvalidOperationException>(() => controller.Logout(connection));
        }
    }
}
