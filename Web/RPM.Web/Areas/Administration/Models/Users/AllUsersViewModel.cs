namespace RPM.Web.Areas.Administration.Models.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using RPM.Services.Admin.Models;

    using static RPM.Common.GlobalConstants;

    public class AllUsersViewModel
    {
        public IEnumerable<AdminUserListingServiceModel> Users { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        public int Total { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage - 1;

        public int NextPage => this.CurrentPage + 1;

        public bool PreviousDisabled => this.CurrentPage == 1;

        public bool NextDisabled
        {
            get
            {
                var maxPage = Math.Ceiling(((double)this.Total) / PageSize);

                return maxPage == this.NextPage;
            }
        }
    }
}
