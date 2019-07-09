using Everest.AspNetStartup;
using Everest.AspNetStartup.Controllers;
using Everest.AspNetStartup.Infrastruture;
using Everest.AspNetStartup.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Everest.IdentityTest
{
    public class ServiceConfiguration
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static IServiceCollection ServiceCollection { get; set; }

        public static IServiceCollection InitServiceCollection()
        {
            ServiceCollection = new ServiceCollection();

            ServiceCollection.AddDbContext<DbContext, PersistenceContext>(options => {
                options.UseLazyLoadingProxies();
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            ServiceCollection.AddRepositories();


            ServiceCollection.AddTransient<UserController>();
            ServiceCollection.AddTransient<ConnectionController>();
            ServiceCollection.AddTransient<RoleController>();
            ServiceCollection.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            ServiceCollection.AddTransient<AccessTokenValidator>();
            ServiceCollection.AddSingleton<IConfiguration, TestConfiguration>();

            if (!Directory.Exists(Constant.USER_IMAGE_FOLDER))
            {
                Directory.CreateDirectory(Constant.USER_IMAGE_FOLDER);
            }


            return ServiceCollection;
        }

        public static IServiceProvider BuildServiceProvider()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();

            return ServiceProvider;
        }

        
    }
}
