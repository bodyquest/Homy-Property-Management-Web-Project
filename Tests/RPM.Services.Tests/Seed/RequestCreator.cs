namespace RPM.Services.Tests.Seed
{
    using System;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;

    public class RequestCreator
    {
        public static Request Create(Home home)
        {
            var rnd = new Random();

            return new Request
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.UtcNow,
                Type = RequestType.ToRent,
                User = UserCreator.Create("Miroslav", "Shpekov", "shpeka", "shpek@prasmail.com"),
                Home = home,
                Status = RequestStatus.NA,
            };
        }

        public static Request CreateManageApproved(Home home, User user, string id)
        {
            var rnd = new Random();

            return new Request
            {
                Id = id,
                Date = DateTime.UtcNow,
                Type = RequestType.ToManage,
                User = user,
                UserId = user.Id,
                Home = home,
                HomeId = home.Id,
                Status = RequestStatus.Approved,
            };
        }

        public static Request CreateRentApproved(Home home, User user, string id)
        {
            var rnd = new Random();

            return new Request
            {
                Id = id,
                Date = DateTime.UtcNow,
                Type = RequestType.ToRent,
                User = user,
                UserId = user.Id,
                Home = home,
                HomeId = home.Id,
                Status = RequestStatus.Approved,
            };
        }
    }
}
