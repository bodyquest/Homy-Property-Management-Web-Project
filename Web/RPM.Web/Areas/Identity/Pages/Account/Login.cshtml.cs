namespace RPM.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using RPM.Data.Models;
    using RPM.Services.Common.Implementations;

    using static RPM.Common.GlobalConstants;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly ReCaptchaService recaptchaService;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<LoginModel> logger;

        public LoginModel(
            SignInManager<User> signInManager,
            ILogger<LoginModel> logger,
            UserManager<User> userManager,
            ReCaptchaService recaptchaService)
        {
            this.userManager = userManager;
            this.recaptchaService = recaptchaService;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                await this.signInManager.SignOutAsync();
                this.HttpContext.Session.Clear();
                this.HttpContext.User = new System.Security
                    .Claims.ClaimsPrincipal(new ClaimsIdentity(string.Empty));
            }

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            // var url = Request.HttpContext.Request.Query["ReturnUrl"];
            // var uri = String.Format("http://example.com?page={0}", url);
            // Response.Redirect(uri);
            returnUrl = returnUrl ?? this.Url.Content("~/");

            // Google ReCaptcha
            var recaptcha = this.recaptchaService.ValidateResponse(this.Input.Token);
            if (!recaptcha.Result.Success && recaptcha.Result.Score <= 0.5)
            {
                this.ModelState.AddModelError(string.Empty, "You are possibly using fake account!");

                this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                return this.Page();
            }

            if (this.ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await this.signInManager
                    .PasswordSignInAsync(this.Input.Username, this.Input.Password, this.Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = this.userManager.FindByNameAsync(this.Input.Username).GetAwaiter().GetResult();
                    var roles = this.userManager.GetRolesAsync(user).GetAwaiter().GetResult();

                    if (roles.Contains(AdministratorRoleName))
                    {
                        this.logger.LogInformation("Admin logged in.");
                        return this.LocalRedirect("~/Administration/Dashboard/Index");
                    }
                    else if (roles.Contains(OwnerRoleName))
                    {
                        this.logger.LogInformation("Owner logged in.");
                        return this.LocalRedirect("~/Management/Dashboard/Index");
                    }
                    else if (roles.Contains(ManagerRoleName))
                    {
                        this.logger.LogInformation("Owner logged in.");
                        return this.LocalRedirect("~/Dashboard/Index");
                    }
                    else
                    {
                        this.logger.LogInformation("User logged in.");
                        return this.LocalRedirect(returnUrl);
                    }
                }

                if (result.RequiresTwoFactor)
                {
                    return this.RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = this.Input.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("User account locked out.");
                    return this.RedirectToPage("./Lockout");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                    return this.Page();
                }
            }

            // If we got this far, something failed, redisplay form
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return this.Page();
        }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            //[Required]
            public string Token { get; set; }
        }
    }
}
