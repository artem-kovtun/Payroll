using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Payroll.Models;
using Payroll.Models.ServiceResponses;
using Payroll.Models.Views;
using Payroll.Services.Authorization;

namespace Payroll.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private IAuthorizationService _authorizationService { get; set; }
        private IMapper _mapper { get; set; }
        private PayrollDbContext _db { get; set; }
        
        public UserController(IAuthorizationService authorization, IMapper mapper, PayrollDbContext context)
        {
            _authorizationService = authorization;
            _mapper = mapper;
            _db = context;
        }

        private string CurrentUser
        {
            get
            {
                return HttpContext.User.Identity.Name;
            }
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var credentials = _mapper.Map<AuthorizationViewModel>(model);
                var response = await _authorizationService.IsExistedUserAsync(credentials);

                if (response.Status == ServiceResponseStatus.Success)
                {
                    await AuthenticateAsync(credentials.Username);

                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    ViewData["ErrorMessage"] = response.Message;
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(value => value.Errors).Select(e => e.ErrorMessage).LastOrDefault();
                ViewData["ErrorMessage"] = error;
            }
            return View();
        }

        [HttpGet("signup")]
        public IActionResult Signup()
        {
            return View();
        }
        
        [HttpPost("signup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var credentials = _mapper.Map<AuthorizationViewModel>(model);
                var response = await _authorizationService.AddNewUserAsync(credentials);
                
                if (response.Status == ServiceResponseStatus.Success)
                {
                    if (model.AuthorizeAfter)
                    {
                        await AuthenticateAsync(credentials.Username);

                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect(returnUrl);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }
                }
                else
                {
                    ViewData["ErrorMessage"] = response.Message;
                }
            }
            else
            {
                var error = ModelState.Values.SelectMany(value => value.Errors).Select(e => e.ErrorMessage).LastOrDefault();
                ViewData["ErrorMessage"] = error;
            }
            return View();
        }

        [HttpPost("signout")]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var userProfile = _db.UserProfiles.Where(profile => profile.User.Username == CurrentUser).FirstOrDefault();

            if (userProfile != null)
            {
                var userProfileViewModel = _mapper.Map<UserProfileViewModel>(userProfile);
                return View(userProfileViewModel);
            }
            return View();
        } 

        [HttpPost("profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userProfile = _mapper.Map<UserProfile>(model);
                userProfile.User = _db.Users.FirstOrDefault(user => user.Username == CurrentUser);

                var exist = _db.UserProfiles.Any(x => x.VAT == userProfile.VAT);
                if (exist)
                {
                    _db.UserProfiles.Update(userProfile);
                }
                else
                {
                    _db.UserProfiles.Add(userProfile);
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var error = ModelState.Values.SelectMany(value => value.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
                ViewData["ErrorMessage"] = error;
            }
            return View();
        }

        private async Task AuthenticateAsync(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}