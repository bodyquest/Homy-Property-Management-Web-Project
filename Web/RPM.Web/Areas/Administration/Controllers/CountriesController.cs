namespace RPM.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using RPM.Services.Admin;
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

        //// POST - Create
        //[HttpPost]
        //public async Task<IActionResult> Create(AdminCategoryCreateViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await this.adminCountryService.CreateAsync(model.Name);

        //        return RedirectToAction(nameof(Index));
        //    }

        //    return this.View(model);
        //}
    }
}
