using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AuthSamples.Cookies.Controllers
{
    /// <summary>
    /// Controller that override user's authorization flow
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IConfiguration Configuration;
        private static string ConfPath = "AppSettings:User:"; // path to settings in conf file

        public AccountController(IConfiguration configuration) => this.Configuration = configuration;

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Validate userName and password with values from config file
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>True if both, userName and password, are valid</returns>
        private bool ValidateLogin(string userName, string password) => this.Configuration.GetValue<string>(ConfPath + "UserName") == userName && this.Configuration.GetValue<string>(ConfPath + "Password") == password;

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // Normally Identity handles sign in, but you can do it directly
            if (ValidateLogin(userName, password))
            {
                var claims = new List<Claim>
                {
                    new Claim("user", userName),
                    new Claim("role", "Member")
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

                // redirect back, where login was needed
                Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "/");
            }

            return View();
        }

        public IActionResult AccessDenied(string returnUrl = null) => View();

        /// <summary>
        /// Logout user and redirect to Home
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}