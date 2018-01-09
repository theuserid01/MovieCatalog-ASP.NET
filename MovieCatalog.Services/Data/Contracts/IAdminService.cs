namespace MovieCatalog.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MovieCatalog.Services.Data.Models.Users;

    public interface IAdminService
    {
        Task<int> CountUsers(string search);

        IEnumerable<UserBaseServiceModel> GetAllUsers(int page = 1, string search = null);
    }
}
