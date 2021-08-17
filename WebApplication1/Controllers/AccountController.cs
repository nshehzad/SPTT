using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
       // private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private readonly ILogger _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        //public AccountController(SignInManager<AppUser> signinMgr)
        //{
        //    //userManager = userMgr;
        //    signInManager = signinMgr;
        //}


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {



            ViewData["ReturnUrl"] = returnUrl;
            var redirectUrl = Url.Content("~/");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl });//,
               // WsFederationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignedOut), "Account", values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View();
        }


        [Authorize]
        public IActionResult Logout()
        {

            var callbackUrl = Url.Action(nameof(SignedOut), "Account", values: null, protocol: Request.Scheme);
            return  SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme);

            //ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            //string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };

            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //AppUser user = new AppUser
            //{

            //    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
            //    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
            //};

            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //HttpContext.Session.Clear();
            ////HttpContext.Session.Clear();
            //Response.Cookies.

            ////await signInManager.SignOutAsync();
            //// var redirectUrl = Url.Content("~/");
            ////return View(
            ////   new AuthenticationProperties { RedirectUri = redirectUrl });

            ////,
            ////WsFederationDefaults.AuthenticationScheme);
            //return View();
        }
    }
}