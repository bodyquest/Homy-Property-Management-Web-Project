namespace RPM.Services.Tests
{
    using Data;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using RPM.Data.Models;
    using RPM.Services.Tests.Mocks;

    public class BaseServiceTest
    {
        protected ApplicationDbContext Context;
        protected Mock<UserManager<User>> UserManager;

        protected BaseServiceTest()
        {
            this.Context = InMemoryDatabase.Get();
            this.UserManager = UserManagerMock.New;
        }
    }
}
