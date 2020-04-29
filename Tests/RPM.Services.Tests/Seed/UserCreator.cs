namespace RPM.Services.Tests.Seed
{
    using System;
    using RPM.Data.Models;

    public class UserCreator
    {
        public static User Create(string firstName , string lastName, string userName, string email)
        {
            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                Birthdate = DateTime.Now.AddYears(-20),
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
            };
        }
    }
}
