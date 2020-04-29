namespace RPM.Web.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Hangfire.Dashboard;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data.Models;

    using static RPM.Common.GlobalConstants;

    public class HangfireAuthorizationFilter : ControllerBase, IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow Admin to see hangfire dashboard).
            if (httpContext.User.IsInRole(AdministratorRoleName))
            {
                return true;
            }

            return false;
        }
    }
}
