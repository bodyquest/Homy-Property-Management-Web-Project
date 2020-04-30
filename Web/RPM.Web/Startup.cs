namespace RPM.Web
{
    using System;
    using System.Reflection;

    using CloudinaryDotNet;
    using Hangfire;
    using Hangfire.Dashboard;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using RPM.Common.PaymentGateways.Stripe;
    using RPM.Data;
    using RPM.Data.Common;
    using RPM.Data.Common.Repositories;
    using RPM.Data.Models;
    using RPM.Data.Repositories;
    using RPM.Data.Seeding;
    using RPM.Services.Admin;
    using RPM.Services.Admin.Implementations;
    using RPM.Services.Common;
    using RPM.Services.Common.Implementations;
    using RPM.Services.Common.Models.ReCAPTHASettings;
    using RPM.Services.Data;
    using RPM.Services.Management;
    using RPM.Services.Management.Implementations;
    using RPM.Services.Mapping;
    using RPM.Services.Messaging;
    using RPM.Web.Filters;
    using RPM.Web.ViewModels;
    using Stripe;

    using static RPM.Common.GlobalConstants;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<User>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => false;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddSingleton(this.configuration);

            // services.AddCors(options =>
            // {
            //    options.AddPolicy(
            //        "AllowAllOrigins",
            //        builder => builder
            //        .AllowAnyOrigin());
            // });
            services.Configure<IdentityOptions>(options =>
            {
                options.Password = new PasswordOptions()
                {
                    RequiredLength = 6,
                    RequireUppercase = false,
                    RequireLowercase = false,
                    RequireDigit = false,
                    RequiredUniqueChars = 0,
                    RequireNonAlphanumeric = false,
                };

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            var account = new CloudinaryDotNet.Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:ApiSecret"]);

            var cloudUtility = new Cloudinary(account);
            services.AddSingleton(cloudUtility);

            services.AddHangfire(x => x.UseSqlServerStorage(this.configuration.GetConnectionString("HangfireConnection")));
            services.AddHangfireServer();

            services.Configure<StripeSettings>(this.configuration.GetSection("Stripe"));

            services.Configure<ReCaptchaSettings>(this.configuration.GetSection("GooglereCAPTCHA"));

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // services.AddScoped<ISeeder, ApplicationDbContextSeeder>(); !!!

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(SendGridKey));
            //services.AddSingleton<IEmailSender, SendGridEmailSender>();
            //services.Configure<EmailOptions>(this.configuration);

            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IAdminUserService, AdminUserService>();
            services.AddTransient<IAdminCountryService, AdminCountryService>();
            services.AddTransient<IAdminCityService, AdminCityService>();
            services.AddTransient<IAdminListingService, AdminListingService>();
            services.AddTransient<IAdminRentalService, AdminRentalService>();

            services.AddTransient<ICloudImageService, CloudImageService>();
            services.AddTransient<IImageDbService, ImageDbService>();
            services.AddTransient<IListingService, ListingService>();

            services.AddTransient<IOwnerListingService, OwnerListingService>(); // Tests
            services.AddTransient<IOwnerRequestService, OwnerRequestService>(); // 
            services.AddTransient<IOwnerRentalService, OwnerRentalService>(); // Tests
            services.AddTransient<IOwnerPaymentService, OwnerPaymentService>(); // Tests
            services.AddTransient<IOwnerContractService, OwnerContractService>(); // 
            services.AddTransient<IOwnerTransactionRequestService,
                OwnerTransactionRequestService>(); // 

            services.AddTransient<IPaymentCommonService, PaymentCommonService>(); // 
            services.AddTransient<IRentalService, RentalService>(); // 
            services.AddTransient<IRequestService, RequestService>(); // 
            services.AddTransient<ICityService, CityService>(); // 
            services.AddTransient<ICountryService, CountryService>(); // 

            services.AddTransient<ReCaptchaService>(); // 

            // External Authentications
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = FaceBookAppId;
                facebookOptions.AppSecret = FaceBookAppSecret;
            });

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = GoogleClientId;
                options.ClientSecret = GoogleClientSecret;
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddRouting(option =>
            {
                option.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                option.LowercaseUrls = true;
            });

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
            });  // TODO: remove if it does not work as intended !!!!

            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            /*
             app.UseCors(options =>
                options
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
             */

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession(); // TODO: remove if it does not work as intended !!!!

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            StripeConfiguration.ApiKey = this.configuration.GetSection("Stripe")["SecretKey"];

            var options = new DashboardOptions
            {
                Authorization = new[]
                {
                    new HangfireAuthorizationFilter(),
                },

            };

            app.UseHangfireDashboard("/hangfire", options);

            app.UseHangfireServer();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller:slugify}/{action:slugify}/{id:slugify?}",
                    defaults: new { controller = "Home", action = "Index" });

                    endpoints.MapControllerRoute(
                    name: "api",
                    pattern: "{controller}/{action}/{id?}");

                    endpoints.MapRazorPages();
                });
        }
    }
}
