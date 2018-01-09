namespace MovieCatalog.Web.Infrastructure.Extensions
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using MovieCatalog.Common;
    using MovieCatalog.Data;
    using MovieCatalog.Data.Models;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MyDbInitializer(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MovieCatalogDbContext>();

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                //context.Database.Migrate();

                context.Database
                    .ExecuteSqlCommand(File.ReadAllText(GlobalConstants.CountriesSqlFilePath));
                context.Database
                    .ExecuteSqlCommand(File.ReadAllText(GlobalConstants.ColorsSqlFilePath));
                context.Database
                    .ExecuteSqlCommand(File.ReadAllText(GlobalConstants.GenresSqlFilePath));
                context.Database
                    .ExecuteSqlCommand(File.ReadAllText(GlobalConstants.GoldenGlobesSqlFilePath));
                context.Database
                    .ExecuteSqlCommand(File.ReadAllText(GlobalConstants.LanguagesSqlFilePath));
                context.Database
                    .ExecuteSqlCommand(File.ReadAllText(GlobalConstants.LocationsSqlFilePath));
                context.Database
                    .ExecuteSqlCommand(File.ReadAllText(GlobalConstants.OscarsSqlFilePath));

                // On Startup create/seed default admin user if doesn't exist
                Task.Run(async () =>
                {
                    string adminRole = GlobalConstants.AdministratorRole;
                    string[] roles =
                    {
                        GlobalConstants.AdministratorRole,
                        GlobalConstants.ModeratorRole
                    };

                    foreach (string role in roles)
                    {
                        bool roleExists = await roleManager.RoleExistsAsync(role);

                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole()
                            {
                                Name = role
                            });
                        }
                    }

                    User adminUser = await userManager.FindByNameAsync(GlobalConstants.AdminUsername);

                    if (adminUser == null)
                    {
                        adminUser = new User()
                        {
                            Email = GlobalConstants.AdminEmail,
                            UserName = GlobalConstants.AdminUsername
                        };

                        IdentityResult resultCreate = await userManager
                            .CreateAsync(adminUser, GlobalConstants.AdminPassword);

                        IdentityResult resultAddRole = await userManager
                            .AddToRoleAsync(adminUser, adminRole);
                    }
                })
                .GetAwaiter()
                .GetResult();
            }

            return app;
        }
    }
}
