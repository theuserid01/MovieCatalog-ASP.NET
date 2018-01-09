namespace MovieCatalog.Tests.Mocks
{
    using Microsoft.AspNetCore.Identity;
    using Moq;

    public class RoleManagerMock
    {
        public static Mock<RoleManager<IdentityRole>> GetNew()
        {
            IRoleStore<IdentityRole> roleStoreMock = Mock.Of<IRoleStore<IdentityRole>>();

            return new Mock<RoleManager<IdentityRole>>(roleStoreMock, null, null, null, null);
        }
    }
}
