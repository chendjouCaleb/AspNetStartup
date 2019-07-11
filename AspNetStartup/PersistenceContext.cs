
using Everest.AspNetStartup.Entities;
using Microsoft.EntityFrameworkCore;

namespace Everest.AspNetStartup
{
    public class PersistenceContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Connection> Connections { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public PersistenceContext(DbContextOptions<PersistenceContext> options):base(options)
        {

        }

        public PersistenceContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
