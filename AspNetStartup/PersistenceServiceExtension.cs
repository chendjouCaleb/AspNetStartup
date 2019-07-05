using Everest.AspNetStartup.Entities;
using Everest.AspNetStartup.Core.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Everest.AspNetStartup
{
    public static class PersistenceServiceExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRepository<User, string>, Repository<User, string>>();
            services.AddTransient<IRepository<Connection, long>, Repository<Connection, long>>();
            services.AddTransient<IRepository<Role, string>, Repository<Role, string>>();
            services.AddTransient<IRepository<UserRole, long>, Repository<UserRole, long>>();
        }
    }
}
