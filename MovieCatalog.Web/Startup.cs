namespace MovieCatalog.Web
{
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MovieCatalog.Data;
    using MovieCatalog.Data.Models;
    using MovieCatalog.Web.Infrastructure.Extensions;
    using MovieCatalog.Web.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MovieCatalogDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    // Disable password requirements
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<MovieCatalogDbContext>()
                .AddDefaultTokenProviders();

            //services.AddAuthentication().AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //});

            services.AddMvc(options =>
            {
                // Automatically validating all appropriate actions
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddRouting(routing => routing.LowercaseUrls = true);

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            #region Custom Services

            // Add application services located in project *.Services with Reflection
            // !!! Must be registered before AutomaMapper
            services.AddDomainServices();

            // Map objects with reflection in AutoMapperProfile
            services.AddAutoMapper();

            #endregion Custom Services
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Call context.Database.Migrate()
            // Create/seed roles & default admin user if doesn't exist
            // !!! Enable after applying Initial MIgration
            //app.MyDbInitializer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
