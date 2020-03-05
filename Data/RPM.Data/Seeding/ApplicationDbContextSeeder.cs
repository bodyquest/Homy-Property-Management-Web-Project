namespace RPM.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using RPM.Common;
    using RPM.Data.Models;
    using static RPM.Common.GlobalConstants;

    public class ApplicationDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ApplicationDbContextSeeder));

            var seeders = new List<ISeeder>
                          {
                              new RolesSeeder(),
                              new SettingsSeeder(),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }

            var userManager = serviceProvider.GetService<UserManager<User>>();

            await CreateAdmin(userManager, AdminUserName, AdminEmail, AdminFirstName, AdminLastName, DefaultAdminPassword, AdministratorRoleName);

            await CreateOwner(userManager, OwnerUserName, OwnerEmail, OwnerFirstName, OwnerLastName, DefaultOwnerPassword, OwnerRoleName);

            await CreateManager(userManager, ManagerUserName, ManagerEmail, ManagerFirstName, ManagerLastName, DefaultManagerPassword, ManagerRoleName);
        }

        private static async Task CreateAdmin(UserManager<User> userManager, string username, string email, string firstName, string lastName, string defaultPassword, string role)
        {
            var adminUser = await userManager.FindByEmailAsync(email);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Birthdate = DateTime.UtcNow.AddYears(-20),
                    EmailConfirmed = true,
                    SecurityStamp = "S0m3R4nd0mV4lu3",
                };

                var result = await userManager.CreateAsync(adminUser, defaultPassword);
                await userManager.AddToRoleAsync(adminUser, role);
            }
        }

        private static async Task CreateOwner(UserManager<User> userManager, string username, string email, string firstName, string lastName, string defaultPassword, string role)
        {
            var ownerUser = await userManager.FindByEmailAsync(email);

            if (ownerUser == null)
            {
                ownerUser = new User
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Birthdate = DateTime.UtcNow.AddYears(-40),
                    EmailConfirmed = true,
                    SecurityStamp = "S0m3R4nd0mV4lu3",
                };

                var result = await userManager.CreateAsync(ownerUser, defaultPassword);
                await userManager.AddToRoleAsync(ownerUser, role);
            }
        }

        private static async Task CreateManager(UserManager<User> userManager, string username, string email, string firstName, string lastName, string defaultPassword, string role)
        {
            var managerUser = await userManager.FindByEmailAsync(email);

            if (managerUser == null)
            {
                managerUser = new User
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Birthdate = DateTime.UtcNow.AddYears(-40),
                    EmailConfirmed = true,
                    SecurityStamp = "S0m3R4nd0mV4lu3",
                };

                var result = await userManager.CreateAsync(managerUser, defaultPassword);
                await userManager.AddToRoleAsync(managerUser, role);
            }
        }
    }
}
