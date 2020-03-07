namespace RPM.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using RPM.Services.Admin;
    using RPM.Web.Areas.Administration.Models.Countries;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CountriesController : AdministrationController
    {
        private readonly IAdminCountryService adminCountryService;

        public CountriesController(IAdminCountryService adminCountryService)
        {
            this.adminCountryService = adminCountryService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.adminCountryService.AllCountriesAsync();

            return this.View(model);
        }

        // GET - Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST - Create
        [HttpPost]
        public async Task<IActionResult> Create(AdminCountryCreateInputViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.adminCountryService.CreateAsync(model.Name, model.Code);

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(model);
        }
    }
}
