namespace RPM.Web.Areas.Administration.Models.Users
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using RPM.Services.Admin.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AdminUsersListingViewModel
    {
        public IEnumerable<AdminUserListingServiceModel> Users { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
