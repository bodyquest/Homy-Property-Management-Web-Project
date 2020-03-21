namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class ListingsController : ManagementController
    {
        public ListingsController()
        {

        }

        public async Task<IActionResult> Index()
        {
            return this.View();
        }

        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync()
        {
            return this.View();
        }

        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(string id)
        {
            return this.View();
        }

        [ActionName("All")]
        public async Task<IActionResult> AllAsync(string id)
        {
            return this.View();
        }
    }
}
