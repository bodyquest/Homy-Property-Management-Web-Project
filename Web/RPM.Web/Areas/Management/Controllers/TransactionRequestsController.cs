namespace RPM.Web.Areas.Management.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hangfire;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Common;
    using RPM.Services.Management;
    using RPM.Web.Areas.Management.Models.Requests;
    using RPM.Web.Areas.Management.Models.TransactionRequests;
    using RPM.Web.Infrastructure.Extensions;
    using static RPM.Common.GlobalConstants;

    [Authorize(Roles = OwnerRoleName)]
    public class TransactionRequestsController : ManagementController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IOwnerRentalService rentalService;
        private readonly IOwnerListingService listingService;
        private readonly IOwnerTransactionRequestService transactionRequestService;
        private readonly IPaymentCommonService paymentService;

        public TransactionRequestsController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IOwnerRentalService rentalService,
            IOwnerListingService listingService,
            IOwnerTransactionRequestService transactionRequestService,
            IPaymentCommonService paymentService)
        {
            this.context = context;
            this.userManager = userManager;
            this.rentalService = rentalService;
            this.listingService = listingService;
            this.transactionRequestService = transactionRequestService;
            this.paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(this.User);

            var model = await this.transactionRequestService
                .GetAllTransactionRequestsAsync(userId);

            var viewModel = new OwnerTransactionRequestsWithDetailsViewModel
            {
                TransactionRequests = model,
            };

            return this.View(viewModel);
        }

        [ActionName("SchedulePaymentFrom")]
        public async Task<IActionResult> CreateAsync()
        {
            var userId = this.userManager.GetUserId(this.User);

            var rentals = await this.rentalService.GetTransactionRentalsAsync(userId);
            var managedHomes = await this.listingService.GetManagedHomesAsync(userId);

            var model = new OwnerTransactionRequestsCreateInputModel
            {
                RentalsList = rentals,
            };

            return this.View(model);
        }

        [HttpPost]
        [ActionName("CreateFrom")]
        public async Task<IActionResult> CreateFromAsync(OwnerTransactionRequestsCreateInputModel model)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (this.ModelState.IsValid)
            {
                var modelForDb = new OwnerTransactionRequestsCreateInputServiceModel
                {
                    Reason = model.Reason,
                    RecurringSchedule = model.RecurringSchedule,
                    IsRecurring = model.IsRecurring,
                    RentalId = model.RentalId,
                };

                var isCreatedId = await this.transactionRequestService.CreateAsync(userId, modelForDb);

                if (isCreatedId == null)
                {
                    var rentals = await this.rentalService.GetTransactionRentalsAsync(userId);

                    var viewModel = new OwnerTransactionRequestsCreateInputModel
                    {
                        RentalsList = rentals,
                    };

                    return this.View(viewModel);
                }

                var schedule = model.RecurringSchedule;

                var cronType = this.GetCronFromRecurringType(schedule);

                RecurringJob.AddOrUpdate(
                    isCreatedId,
                    () => this.paymentService.AddPaymentRequestToUserAsync(
                        userId,
                        isCreatedId), cronType);

                return this.RedirectToAction("Index", "TransactionRequests", new { area = ManagementArea }).WithSuccess(string.Empty, RecordCreatedSuccessfully);
            }

            return this.View(model);
        }

        [ActionName("SchedulePaymentTo")]
        public async Task<IActionResult> CreateToAsync()
        {
            var userId = this.userManager.GetUserId(this.User);

            var managedHomes = await this.listingService.GetManagedHomesAsync(userId);

            var model = new OwnerTransactionPaymentsCreateInputModel
            {
                ManagedHomesList = managedHomes,
            };

            return this.View(model);
        }

        [HttpPost]
        [ActionName("SchedulePaymentTo")]
        public async Task<IActionResult> CreateToAsync(OwnerTransactionPaymentsCreateInputModel model)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (this.ModelState.IsValid)
            {
                var modelForDb = new OwnerTransactionToRequestsCreateInputServiceModel
                {
                    Reason = model.Reason,
                    Amount = model.Amount,
                    RecurringSchedule = model.RecurringSchedule,
                    IsRecurring = model.IsRecurring,
                    HomeId = model.HomeId,
                };

                var isCreatedId = await this.transactionRequestService.CreateToAsync(userId, modelForDb);

                if (isCreatedId == null)
                {
                    var managedHomes = await this.listingService.GetManagedHomesAsync(userId);

                    var viewModel = new OwnerTransactionPaymentsCreateInputModel
                    {
                        ManagedHomesList = managedHomes,
                    };

                    return this.View(viewModel);
                }

                // TEST the service. Put Breakpoint on Line 172 and when hit, press F11
                // await this.paymentService.AddPaymentRequestToUserAsync(
                //        userId,
                //        isCreatedId);

                var schedule = model.RecurringSchedule;

                var cronType = this.GetCronFromRecurringType(schedule);

                // pending payment will appear in the payments dashboard of the user
                RecurringJob.AddOrUpdate(
                    isCreatedId,
                    () => this.paymentService.AddPaymentRequestToUserAsync(
                        userId,
                        isCreatedId), cronType);

                return this.RedirectToAction("Index", "TransactionRequests", new { area = ManagementArea })
                    .WithSuccess(string.Empty, RecordCreatedSuccessfully);
            }

            return this.View(model);
        }

        public async Task<IActionResult> RemoveRecurring(string id)
        {
            var transactionRequestForDelete = await this.transactionRequestService.FindByIdAsync(id);

            if (transactionRequestForDelete != null)
            {
                transactionRequestForDelete.Status = TransactionRequestStatus.Removed;
                bool isSuccessful = await this.transactionRequestService.UpdateAsync(transactionRequestForDelete);

                RecurringJob.RemoveIfExists(transactionRequestForDelete.Id);

                return this.RedirectToAction("Index", "Dashboard", new { area = ManagementArea })
                .WithSuccess(string.Empty, RecordUpdatedSuccessfully);
            }

            return this.RedirectToAction("Index", "TransactionRequests", new { area = ManagementArea })
                .WithWarning(string.Empty, CouldNotFind);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var transactionRequestForEdit = await this.transactionRequestService.FindByIdAsync(id);

            return this.View(transactionRequestForEdit);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(OwnerTransactionRequestsCreateInputModel modelForEdit)
        {
            var userId = this.userManager.GetUserId(this.User);

            var transactionRequestForEdit = await this.transactionRequestService.FindByIdAsync(modelForEdit.Id);

            if (this.ModelState.IsValid)
            {
                bool isSuccessful = await this.transactionRequestService.UpdateAsync(transactionRequestForEdit);

                var cronType = this.GetCronFromRecurringType(modelForEdit.RecurringSchedule);

                RecurringJob.AddOrUpdate(
                    modelForEdit.Id,
                    () => this.paymentService.AddPaymentRequestToUserAsync(
                        userId,
                        modelForEdit.Id), cronType);

                return this.RedirectToAction("Index", "Dashboard", new { area = ManagementArea })
                .WithSuccess(string.Empty, RecordUpdatedSuccessfully);
            }

            return this.View(modelForEdit);
        }

        private Func<string> GetCronFromRecurringType(
            RecurringScheduleType recurringSchedule)
        {
            if (recurringSchedule.ToString() == "Monthly")
            {
                return CustomCron.Monthly;
            }
            else if (recurringSchedule.ToString() == "Yearly")
            {
                return Cron.Yearly;
            }
            else if (recurringSchedule.ToString() == "Weekly")
            {
                return Cron.Weekly;
            }
            else if (recurringSchedule.ToString() == "Hourly")
            {
                return Cron.Hourly;
            }
            else if (recurringSchedule.ToString() == "Minutely")
            {
                return CustomCron.TenMinutely;
            }
            else
            {
                return Cron.Monthly;
            }
        }
    }

    internal static class CustomCron
    {
        public static string TenMinutely()
        {
            return TenMinute;
        }

        public static string Monthly()
        {
            var secondLastDayOfMonth = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month) - 1;
            var expression = string.Format(MonthlyOneDayBeforeEoM, secondLastDayOfMonth);

            return expression;
        }
    }
}
