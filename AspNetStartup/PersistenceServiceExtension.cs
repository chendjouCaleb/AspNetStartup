using Everest.AspNetStartup.Entities;
using Everest.AspNetStartup.Core.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Everest.AspNetStartup.Infrastruture;
using Everest.AspNetStartup.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Everest.AspNetStartup
{
    public static class PersistenceServiceExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {

            //PersistenceContext context = new DesignTimeDbContextFactory().CreateDbContext(new string[] { });
            //services.AddScoped<DbContext>(provider =>
            //{
            //    return new DesignTimeDbContextFactory().CreateDbContext(new string[] { });
            //});


            services.AddTransient<IRepository<User, string>, Repository<User, string>>();
            services.AddTransient<IRepository<Connection, long>, Repository<Connection, long>>();
            services.AddTransient<IRepository<Role, string>, Repository<Role, string>>();
            services.AddTransient<IRepository<UserRole, long>, Repository<UserRole, long>>();


            services.AddTransient<RoleController>();
            services.AddTransient<UserController>();
            services.AddTransient<UserSeedData>();
            services.AddTransient<RoleSeedData>();
        }
    }
}
