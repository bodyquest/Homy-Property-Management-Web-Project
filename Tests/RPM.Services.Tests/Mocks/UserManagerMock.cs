namespace RPM.Services.Tests.Mocks
{
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using RPM.Data.Models;

    public class UserManagerMock
    {
        public static Mock<UserManager<User>> New
            => new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
    }
}
