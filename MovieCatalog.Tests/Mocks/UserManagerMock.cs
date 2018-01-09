namespace MovieCatalog.Tests.Mocks
{
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using MovieCatalog.Data.Models;

    public class UserManagerMock
    {
        public static Mock<UserManager<User>> GetNew()
        {
            IUserStore<User> userStoreMock = Mock.Of<IUserStore<User>>();

            return new Mock<UserManager<User>>(
                userStoreMock, null, null, null, null, null, null, null, null);
        }
    }
}
