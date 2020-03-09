namespace RPM.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Services.Admin;
    using RPM.Services.Admin.Models;
    using RPM.Web.Areas.Administration.Models.Cities;
    using RPM.Web.Infrastructure.Extensions;

    using static RPM.Common.GlobalConstants;

    public class CitiesController : AdministrationController
    {
        private readonly IAdminCityService adminCityService;

        public CitiesController(IAdminCityService adminCityService)
        {
            this.adminCityService = adminCityService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            var model = await this.adminCityService.AllCitiesAsync();

            return this.View(model);
        }

        // GET: Cities/Create
        public async Task<IActionResult> Create()
        {
            var model = await this.adminCityService.GetCreateAsync();

            return this.View(model);
        }

        // POST: Cities/Create
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync(AdminCityAndCountryInputModel model)
        {
            var modelVM = await this.adminCityService.GetCreateAsync();

            if (this.ModelState.IsValid)
            {
                var cityExists = await this.adminCityService.CreateAsync(model.City.Name, model.City.CountryId);

                if (cityExists == null)
                {
                    this.StatusMessage = EntityAlreadyExists;

                    modelVM.City = model.City;
                    modelVM.StatusMessage = this.StatusMessage;

                    return this.View(modelVM);
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(modelVM);
        }

        // GET: Cities/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return this.View();
        }

        // GET: Cities/Edit/5
        public async Task<ActionResult> EditAsync(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = await this.adminCityService.GetUpdateAsync(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        // POST: Cities/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cities/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cities/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}