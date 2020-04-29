namespace RPM.Services.Tests
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class InMemoryDatabase
    {
        public static ApplicationDbContext Get()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(dbOptions);
        }
    }
}
