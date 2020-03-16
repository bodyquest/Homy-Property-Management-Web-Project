namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : ManagementController
    {
        public DashboardController()
        {

        }

        [HttpGet("/Management/Dashboard/Index")]
        public IActionResult Index()
        {
            // 

            return this.View();
        }
    }
}
