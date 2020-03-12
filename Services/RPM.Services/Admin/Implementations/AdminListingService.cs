namespace RPM.Services.Admin.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Services.Admin.Models;

    using static RPM.Common.GlobalConstants;

    public class AdminListingService : IAdminListingService
    {
        private readonly ApplicationDbContext context;
        private readonly IAdminCityService adminCityService;
        private readonly IAdminCountryService adminCountryService;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<User> userManager;

        public AdminListingService(ApplicationDbContext context, IAdminCityService adminCityService, IAdminCountryService adminCountryService, RoleManager<ApplicationRole> roleManager, UserManager<User> userManager)
        {
            this.context = context;
            this.adminCityService = adminCityService;
            this.adminCountryService = adminCountryService;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<AdminHomesListingServiceModel>> GetAllListingsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateListingAsync(AdminHomeCreateServiceModel model)
        {
            var city = await this.context.Cities.FirstOrDefaultAsync(x => x.Name == model.City);
            var owner = await this.context.Users.FirstOrDefaultAsync(x => x.UserName == model.Owner);

            Home home = new Home
            {
                Name = model.Name,
                Description = model.Description,
                Address = model.Address,
                Price = model.Price,
                City = city,
                Category = model.Category,
                Status = model.Status,
                Owner = owner,
            };

            home.Images.Add(model.Image);

            this.context.Homes.Add(home);
            var result = await this.context.SaveChangesAsync();

            city.Homes.Add(home);
            await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
