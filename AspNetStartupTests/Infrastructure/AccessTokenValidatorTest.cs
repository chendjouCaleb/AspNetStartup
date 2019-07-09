using Everest.AspNetStartup.Controllers;
using Everest.AspNetStartup.Core.Exceptions;
using Everest.AspNetStartup.Core.Persistence;
using Everest.AspNetStartup.Entities;
using Everest.AspNetStartup.Infrastruture;
using Everest.AspNetStartup.Models;
using Everest.IdentityTest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Everest.AspNetStartupTests.Infrastructure
{
    public class AccessTokenValidatorTest
    {
        private ConnectionController controller;
        private UserController userController;

        private IRepository<Connection, long> connectionRepository;
        private IRepository<User, string> userRepository;
        private AccessTokenValidator validator;

        private IServiceCollection serviceCollection;
        private IServiceProvider provider;

        private IConfiguration configuration;

        [SetUp]
        public void BeforeEach()
        {
            serviceCollection = ServiceConfiguration.InitServiceCollection();
            provider = ServiceConfiguration.BuildServiceProvider();

            configuration = provider.GetRequiredService<IConfiguration>();
            connectionRepository = provider.GetRequiredService<IRepository<Connection, long>>();
            userRepository = provider.GetRequiredService<IRepository<User, string>>();


            validator = provider.GetRequiredService<AccessTokenValidator>();

            controller = provider.GetRequiredService<ConnectionController>();
            userController = provider.GetRequiredService<UserController>();
        }

        private Connection CreateConnection()
        {
            var addUserModel = new AddUserModel
            {
                Name = "name",
                Surname = "surname",
                Email = "name@gmail.com",
                Password = "password123",
            };

            User user = userController.Create(addUserModel);

            

            LoginModel model = new LoginModel
            {
                Email = "name@gmail.com",
                Password = "password123",
                OS = "Windows",
                Browser = "Firefox"
            };

            Connection connection = controller.Login(model);

            return connection;
        }

        [Test]
        public void Validate_ValidToken()
        {
            String token = CreateConnection().AccessToken;
            validator.Validate(token);
        }

        [Test]
        public void Validate_ModifiedToken()
        {
            String token = CreateConnection().AccessToken;

            Assert.Throws<UnauthorizedException>(() => validator.Validate(token + "3"));
            Assert.Throws<UnauthorizedException>(() => validator.Validate("1" + token));
            Assert.Throws<UnauthorizedException>(() => validator.Validate(token.Substring(2)));
        }

        [Test]
        public void Validate_TokenOfClosedConnection()
        {
            Connection connection = CreateConnection();
            connection.Close();

            connectionRepository.Update(connection);


            string token = connection.AccessToken;

            Assert.Throws<UnauthorizedException>(() => validator.Validate(token));
        }

        [Test]
        public void Validate_NotExistingConnection()
        {
            Connection connection = CreateConnection();
            string token = connection.AccessToken;
            connectionRepository.Delete(connection);

            Assert.Throws<UnauthorizedException>(() => validator.Validate(token));
        }
    }
}
