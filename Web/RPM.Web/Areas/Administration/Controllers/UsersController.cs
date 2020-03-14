namespace RPM.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data.Models;
    using RPM.Services.Admin;
    using RPM.Services.Admin.Models;
    using RPM.Web.Areas.Administration.Models.Users;
    using RPM.Web.Infrastructure.Extensions;
    using RPM.Web.Models.Users;
    using static RPM.Common.GlobalConstants;

    public class UsersController : AdministrationController
    {
        private readonly IAdminUserService adminUserService;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<User> userManager;

        public UsersController(UserManager<User> userManager, RoleManager<ApplicationRole> roleManager, IAdminUserService adminUserService) 
        {
            this.adminUserService = adminUserService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var users = await this.adminUserService.AllAsync(page);
            var roles = await this.roleManager
                .Roles
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name,
                })
                .OrderByDescending(x => x.Value)
                .ToListAsync();

            var totalUsers = this.adminUserService.Total();

            var model = new AllUsersViewModel
            {
                Users = users,
                Roles = roles,
                Total = totalUsers,
                CurrentPage = page,
                PageSize = PageSize,
            };

            return this.View(model);
        }

        [HttpPost]
        [ActionName(AddToRole)]
        public async Task<IActionResult> AddToRoleAsync(AddUserToRoleFormModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.UserId);

            var roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            var userExists = user != null;

            if (!roleExists || !userExists)
            {
                this.ModelState.AddModelError(string.Empty, InvalidIdentityError);
            }

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Index))
                    .WithWarning(string.Empty, CouldNotUpdateRecord);
            }

            var result = await this.userManager.AddToRoleAsync(user, model.Role);

            return this.RedirectToAction(nameof(this.Index))
                .WithSuccess(string.Empty, RecordUpdatedSuccessfully);
        }

        [HttpPost]
        [ActionName(RemoveRole)]
        public async Task<IActionResult> RemoveFromRole(AddUserToRoleFormModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.UserId);

            var roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            var userExists = user != null;

            if (!roleExists || !userExists)
            {
                this.ModelState.AddModelError(string.Empty, InvalidIdentityError);
            }

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Index))
                    .WithWarning(string.Empty, CouldNotUpdateRecord);
            }

            var result = await this.userManager.RemoveFromRoleAsync(user, model.Role);

            return this.RedirectToAction(nameof(this.Index))
                .WithSuccess(string.Empty, RecordUpdatedSuccessfully);
        }

        public async Task<IActionResult> DetailsAsync(string id)
        {
            var model = await this.adminUserService.GetUserDetailsAsync(id);

            var viewModel = new UserProfileViewModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                Birthdate = model.Birthdate,
                About = model.About,
                ProfilePic = model.ProfilePic,
                RegisteredAt = model.RegisteredAt,

                Rentals = model.Rentals,

                Roles = model.Roles,

                Homes = model.Homes,
            };

            return this.View(viewModel);
        }
    }
}
