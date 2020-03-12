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

        public async Task<IActionResult> Index()
        {
            var users = await this.adminUserService.AllAsync();
            var roles = await this.roleManager
                .Roles
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name,
                })
                .OrderByDescending(x => x.Value)
                .ToListAsync();

            var model = new AdminUsersListingViewModel
            {
                Users = users,
                Roles = roles,
            };

            return this.View(model);
        }

        [HttpPost]
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
                return this.RedirectToAction(nameof(this.Index));
            }

            await this.userManager.AddToRoleAsync(user, model.Role);

            this.TempData.AddSuccessMessage(string.Format(UserAddedToRole, user.UserName, model.Role));

            return this.RedirectToAction(nameof(this.Index));
        }

    }
}
