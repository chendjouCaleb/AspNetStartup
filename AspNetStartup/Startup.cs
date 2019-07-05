using Everest.AspNetStartup;
using Everest.AspNetStartup.Core.ExceptionTransformers;
using Everest.AspNetStartup.Infrastruture;
using Everest.AspNetStartup.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetStartup
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<DbContext, PersistenceContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(Configuration["Data:ConnectionStrings:Database"]);
            });

            services.AddExceptionTransformerFactory();

            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddTransient<AccessTokenValidator>();

            services.AddRepositories();

            services.AddCors(options =>
            {
                options.AddPolicy("corsPolicy", policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.Build();
                });
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var roleSeed = app.ApplicationServices.GetRequiredService<RoleSeedData>();
            roleSeed.Seed();

            var userSeed = app.ApplicationServices.GetRequiredService<UserSeedData>();
            userSeed.Seed();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionTransformer();

            app.UseCors("corsPolicy");

            app.UseRouting();

            app.UseMvc();
        }
    }
}
