namespace RPM.Web
{
    using System.Reflection;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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
    using RPM.Services.Data;
    using RPM.Services.Management;
    using RPM.Services.Management.Implementations;
    using RPM.Services.Mapping;
    using RPM.Services.Messaging;

    using RPM.Web.ViewModels;

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
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddSingleton(this.configuration);

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


            Account account = new Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:ApiSecret"]);

            var cloudUtility = new Cloudinary(account);
            services.AddSingleton(cloudUtility);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender("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"));
            //services.AddSingleton<IEmailSender, SendGridEmailSender>();
            //services.Configure<EmailOptions>(this.configuration);

            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IAdminUserService, AdminUserService>();
            services.AddTransient<IAdminCountryService, AdminCountryService>();
            services.AddTransient<IAdminCityService, AdminCityService>();
            services.AddTransient<IAdminListingService, AdminListingService>();

            services.AddTransient<ICloudImageService, CloudImageService>();
            services.AddTransient<IImageDbService, ImageDbService>();
            //services.AddTransient<ICloudImageService, CloudImageService>();
            services.AddTransient<IListingService, ListingService>();

            services.AddTransient<IOwnerListingService, OwnerListingService>();
            services.AddTransient<IOwnerRequestService, OwnerRequestService>();
            services.AddTransient<IOwnerRentalService, OwnerRentalService>();
            services.AddTransient<IRequestService, RequestService>();

            services.AddMvc(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddRouting(option =>
            {
                option.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                option.LowercaseUrls = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly
                );

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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute(
                    "areaRoute",
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller:slugify}/{action:slugify}/{id:slugify?}",
                    defaults: new { controller = "Home", action = "Index" });

                    endpoints.MapRazorPages();
                });
        }
    }
}
