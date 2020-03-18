namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class RentalsController : ManagementController
    {
        public RentalsController()
        {

        }

        public async Task<IActionResult> Index()
        {
            return this.View();
        }
    }
}
