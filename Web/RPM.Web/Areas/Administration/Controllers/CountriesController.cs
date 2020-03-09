namespace RPM.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RPM.Services.Admin;
    using RPM.Web.Areas.Administration.Models.Countries;
    using RPM.Web.Infrastructure.Extensions;

    using static RPM.Common.GlobalConstants;

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
        public async Task<IActionResult> CreateAsync(AdminCountryCreateInputViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.adminCountryService.CreateAsync(model.Name, model.Code);

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(model);
        }

        // GET - Edit
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = await this.adminCountryService.GetByIdAsync(id);

            return this.View(model);
        }

        // POST - Edit
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPostAsync(int? id, string name, string code)
        {
            if (this.ModelState.IsValid)
            {
                bool isEdited = await this.adminCountryService.UpdateAsync(id, name, code);

                if (!isEdited)
                {
                    return this.RedirectToAction(nameof(this.Index))
                        .WithWarning(string.Empty, CouldNotUpdateEntity);
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(this.Index))
                        .WithWarning(string.Empty, EntityIdIsInvalid);
        }

        // GET - Delete
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = await this.adminCountryService.GetByIdAsync(id);

            return this.View(model);
        }

        // POST - Delete
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePostAsync(int? id)
        {
            if (this.ModelState.IsValid)
            {
                bool isDeleted = await this.adminCountryService.DeleteAsync(id);

                if (!isDeleted)
                {
                    return this.RedirectToAction(nameof(this.Index))
                        .WithWarning(string.Empty, CouldNotDeleteEntity);
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(this.Index))
                        .WithWarning(string.Empty, EntityIdIsInvalid);
        }
    }
}
