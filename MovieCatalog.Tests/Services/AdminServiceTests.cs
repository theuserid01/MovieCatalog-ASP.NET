namespace MovieCatalog.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using MovieCatalog.Data;
    using MovieCatalog.Data.Models;
    using MovieCatalog.Services.Data.Contracts;
    using MovieCatalog.Services.Data.Implementations;
    using MovieCatalog.Services.Data.Models.Users;
    using Xunit;

    public class AdminServiceTests
    {
        [Fact]
        public async Task GetAllUsers_ShouldReturnCorrectResultWithFilterAndOrder()
        {
            // Arrange
            MovieCatalogDbContext context = this.GetDbContext();
            IAdminService adminService = new AdminService(context);

            User user1 = new User() { Id = "1", Email = "user1@gamail.com", UserName = "FirstUser" };
            User user2 = new User() { Id = "2", Email = "user2@gamail.com", UserName = "SecondUser" };
            User user3 = new User() { Id = "3", Email = "user3@gamail.com", UserName = "ThirdUser" };

            context.AddRange(user1, user2, user3);
            await context.SaveChangesAsync();

            // Act
            IEnumerable<UserBaseServiceModel> result = adminService.GetAllUsers(1, "first");

            // Assert
            result
                .Should().Match(r => r.First().Id == "1")
                .And
                .HaveCount(1);
        }

        private MovieCatalogDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MovieCatalogDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MovieCatalogDbContext(options);
        }
    }
}
