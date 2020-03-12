namespace RPM.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using RPM.Data.Models;

    using static RPM.Common.GlobalConstants;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            //if (context.Users.Any(u => u.UserName == "radito" && u.UserName == "bodyquest"))
            //{
            //    return;
            //}

            var users = new List<(
                string FirstName,
                string LastName,
                string UserName,
                string Email)>
            {
                // Bulgaria
                ("FirstNameBGSF1", "LastNameBGSF1", "UserNameBGSF1", "EmailBGSF1@email.com"),
                ("FirstNameBGSF2", "LastNameBGSF2", "UserNameBGSF2", "EmailBGSF2@email.com"),
                ("FirstNameBGSF3", "LastNameBGSF3", "UserNameBGSF3", "EmailBGSF3@email.com"),

                ("FirstNameBGVN1", "LastNameBGVN1", "UserNameBGVN1", "EmailBGVN1@email.com"),
                ("FirstNameBGVN2", "LastNameBGVN2", "UserNameBGVN2", "EmailBGVN2@email.com"),
                ("FirstNameBGVN3", "LastNameBGVN3", "UserNameBGVN3", "EmailBGVN3@email.com"),

                ("FirstNameBGBU1", "LastNameBGBU1", "UserNameBGBU1", "EmailBGBU1@email.com"),
                ("FirstNameBGBU2", "LastNameBGBU2", "UserNameBGBU2", "EmailBGBU2@email.com"),
                ("FirstNameBGBU3", "LastNameBGBU3", "UserNameBGBU3", "EmailBGBU3@email.com"),

                ("FirstNameBGPT1", "LastNameBGPT1", "UserNameBGPT1", "EmailBGPT1@email.com"),
                ("FirstNameBGPT2", "LastNameBGPT2", "UserNameBGPT2", "EmailBGPT2@email.com"),
                ("FirstNameBGPT3", "LastNameBGPT3", "UserNameBGPT3", "EmailBGPT3@email.com"),

                // Germany
                ("FirstNameDEB1", "LastNameDEB1", "UserNameDEB1", "EmailDEB1@email.com"),
                ("FirstNameDEB2", "LastNameDEB2", "UserNameDEB2", "EmailDEB2@email.com"),
                ("FirstNameDEB3", "LastNameDEB3", "UserNameDEB3", "EmailDEB3@email.com"),

                ("FirstNameDELZ1", "LastNameDELZ1", "UserNameDELZ1", "EmailDELZ1@email.com"),
                ("FirstNameDELZ2", "LastNameDELZ2", "UserNameDELZ2", "EmailDELZ2@email.com"),
                ("FirstNameDELZ3", "LastNameDELZ3", "UserNameDELZ3", "EmailDELZ3@email.com"),

                ("FirstNameDEDR1", "LastNameDEDR1", "UserNameDEDR1", "EmailDEDR1@email.com"),
                ("FirstNameDEDR2", "LastNameDEDR2", "UserNameDEDR2", "EmailDEDR2@email.com"),
                ("FirstNameDEDR3", "LastNameDEDR3", "UserNameDEDR3", "EmailDEDR3@email.com"),

                // France
                ("FirstNameFRT1", "LastNameFRT1", "UserNameFRT1", "EmailFRT1@email.com"),
                ("FirstNameFRT2", "LastNameFRT2", "UserNameFRT2", "EmailFRT2@email.com"),
                ("FirstNameFRT3", "LastNameFRT3", "UserNameFRT3", "EmailFRT3@email.com"),

                ("FirstNameFRPA1", "LastNameFRPA1", "UserNameFRPA1", "EmailFRPA1@email.com"),
                ("FirstNameFRPA2", "LastNameFRPA2", "UserNameFRPA2", "EmailFRPA2@email.com"),
                ("FirstNameFRPA3", "LastNameFRPA3", "UserNameFRPA3", "EmailFRPA3@email.com"),

                // Italy
                ("FirstNameITARO1", "LastNameITARO1", "UserNameITARO1", "EmaiITARO1@email.com"),
                ("FirstNameITARO2", "LastNameITARO2", "UserNameITARO2", "EmaiITARO2@email.com"),
                ("FirstNameITARO3", "LastNameITARO3", "UserNameITARO3", "EmaiITARO3@email.com"),

                // Portugal
                ("FirstNamePTLI1", "LastNamePTLI1", "UserNamePTLI1", "EmaiPTLI1@email.com"),
                ("FirstNamePTLI2", "LastNamePTLI2", "UserNamePTLI2", "EmaiPTLI2@email.com"),
                ("FirstNamePTLI3", "LastNamePTLI3", "UserNamePTLI3", "EmaiPTLI3@email.com"),
            };

            Random rnd = new Random();
            var defaultPassword = "user123";

            foreach (var user in users)
            {
                int age = rnd.Next(18, 61);

                var newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Birthdate = DateTime.UtcNow.AddYears(-age),
                    EmailConfirmed = true,
                    SecurityStamp = "S0m3R4nd0mV4lu3",
                };

                var result = await userManager.CreateAsync(newUser, defaultPassword);
                await userManager.AddToRoleAsync(newUser, OwnerRoleName);
            }
        }
    }
}
