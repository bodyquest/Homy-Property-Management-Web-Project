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
    using RPM.Web.Areas.Administration.Models.Users;
    using Infrastructure.Extensions;

    using static RPM.Common.GlobalConstants;

    public class UsersController : AdministrationController
    {
        private readonly IAdminUserService adminUserService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UsersController(IAdminUserService adminUserService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.adminUserService = adminUserService;
            this.roleManager = roleManager;
            this.userManager = userManager;
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
                .ToListAsync();

            return this.View(new AdminUsersListingViewModel
            {
                Users = users,
                Roles = roles,
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddToRoleAsync(AddUserToRoleFormModel model)
        {
            var roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            var user = await this.userManager.FindByIdAsync(model.UserId);
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
