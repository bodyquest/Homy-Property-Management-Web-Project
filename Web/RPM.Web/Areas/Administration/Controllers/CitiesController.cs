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
                    this.StatusMessage = RecordAlreadyExists;

                    modelVM.City = model.City;
                    modelVM.StatusMessage = this.StatusMessage;

                    return this.View(modelVM);
                }

                return this.RedirectToAction(nameof(this.Index))
                    .WithSuccess(string.Empty, RecordCreatedSuccessfully);
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
        [ActionName("Edit")]
        public async Task<IActionResult> EditPostAsync(int? id, AdminCityEditDeleteServiceModel model)
        {
            if (this.ModelState.IsValid)
            {
                bool isEdited = await this.adminCityService.UpdateAsync(id, model.City.Name);

                if (!isEdited)
                {
                    this.StatusMessage = CouldNotUpdateRecord;
                    var modelVM = await this.adminCityService.GetUpdateAsync(id);
                    modelVM.StatusMessage = this.StatusMessage;

                    return this.View(modelVM);
                }

                return this.RedirectToAction(nameof(this.Index))
                    .WithSuccess(string.Empty, RecordUpdatedSuccessfully);
            }

            return this.RedirectToAction(nameof(this.Index))
                        .WithWarning(string.Empty, RecordIdIsInvalid);
        }

        // GET - Delete
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = await this.adminCityService.GetByIdAsync(id);

            return this.View(model);
        }

        // POST - Delete
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePostAsync(int? id)
        {
            if (this.ModelState.IsValid)
            {
                bool isDeleted = await this.adminCityService.DeleteAsync(id);

                if (!isDeleted)
                {
                    this.StatusMessage = CouldNotUpdateRecord;
                    var modelVM = await this.adminCityService.GetByIdAsync(id);
                    modelVM.StatusMessage = this.StatusMessage;

                    return this.View(modelVM);
                }

                return this.RedirectToAction(nameof(this.Index))
                    .WithSuccess(string.Empty, RecordDeletedSuccessfully);
            }

            return this.RedirectToAction(nameof(this.Index))
                        .WithWarning(string.Empty, RecordIdIsInvalid);
        }
    }
}