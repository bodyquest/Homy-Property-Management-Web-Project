namespace RPM.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using MimeKit;
    using RPM.Data.Models;
    using RPM.Services.Common.Implementations;
    using RPM.Services.Messaging;

    using static RPM.Common.GlobalConstants;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly ReCaptchaService recaptchaService;

        private IWebHostEnvironment env;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment env,
            ReCaptchaService recaptchaService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.env = env;
            this.recaptchaService = recaptchaService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(UsersNameMax, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = UsersNameMin)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(UsersNameMax, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = UsersNameMin)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(UsersNameMax, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = UsersNameMin)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [DataType(DataType.Date)]
            public DateTime Birthdate { get; set; }

            [Required]
            public string Token { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            // Google ReCaptcha
            var recaptcha = this.recaptchaService.ValidateResponse(this.Input.Token);
            if (!recaptcha.Result.Success && recaptcha.Result.Score <= 0.5)
            {
                this.ModelState.AddModelError(string.Empty, "You are possibly using fake account!");

                this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                return this.Page();
            }

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = this.Input.FirstName,
                    LastName = this.Input.LastName,
                    UserName = this.Input.Username,
                    Email = this.Input.Email,
                    Birthdate = this.Input.Birthdate,
                };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    await this.userManager.AddToRoleAsync(user, ViewerRoleName);

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: this.Request.Scheme);

                    #region Use HTML Email Template

                    var webRoot = this.env.WebRootPath;

                    // Get wwwroot Folder
                    var pathToFile = env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "registerConfirmation.html";

                    var builder = new BodyBuilder();
                    using (StreamReader sourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        builder.HtmlBody = sourceReader.ReadToEnd();
                    }

                    // {0} : callbackURL
                    var body = builder.HtmlBody;
                    string messageBody = string.Format(
                        builder.HtmlBody,
                        callbackUrl);

                    #endregion

                    await this.emailSender.SendEmailAsync(
                        "darko@test.com",
                        "DotNetDari",
                        this.Input.Email,
                        "Confirm your email, bah mu 4udoto i neveroqtnata tehnologiq",
                        messageBody

                        // $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                        );

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email });
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
