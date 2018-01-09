namespace MovieCatalog.Services.Data.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MovieCatalog.Common;
    using MovieCatalog.Data;
    using MovieCatalog.Data.Models;
    using MovieCatalog.Services.Data.Contracts;
    using MovieCatalog.Services.Data.Models.Users;

    public class AdminService : IAdminService
    {
        private readonly MovieCatalogDbContext context;

        public AdminService(MovieCatalogDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CountUsers(string search)
        {
            var users = GetFilteredUsers(search);

            return await users.CountAsync();
        }

        public IEnumerable<UserBaseServiceModel> GetAllUsers(int page = 1, string search = null)
        {
            var users = this.GetFilteredUsers(search);

            return users
                .Select(u => new UserBaseServiceModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Username = u.UserName
                })
                .OrderBy(u => u.Username)
                .Skip((page - 1) * GlobalConstants.PageSize)
                .Take(GlobalConstants.PageSize)
                .ToList();
        }

        private IQueryable<User> GetFilteredUsers(string search)
        {
            var users = this.context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users
                    .Where(u => u.Email.ToLower().Contains(search.ToLower()) ||
                        u.UserName.ToLower().Contains(search.ToLower()));
            }

            return users;
        }
    }
}
