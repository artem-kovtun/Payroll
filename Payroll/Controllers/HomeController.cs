using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Payroll.Models;
using Payroll.Models.Views;
using Payroll.Services.Currency;

namespace Payroll.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IMapper _mapper { get; set; }
        private PayrollDbContext _db { get; set; }
        private ICurrencyHandler _currencyHandler { get; set; }

        public HomeController(IMapper mapper, PayrollDbContext database, ICurrencyHandler currencyHandler)
        {
            _mapper = mapper;
            _db = database;
            _currencyHandler = currencyHandler;
        }

        private User CurrentUser
        {
            get
            {
                return _db.Users.FirstOrDefault(user => user.Username == HttpContext.User.Identity.Name);
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            var docCreateViewModel = new DocCreateViewModel()
            {           
                CustomUSDRate = _mapper.Map<UserProfileViewModel>(_db.UserProfiles.FirstOrDefault(profile => profile.User == CurrentUser))?.USDRate,
                WorkCompletionDate = DateTime.Now,
                Services = _mapper.Map<List<ServiceViewModel>>(_db.Services.Where(service => service.User == CurrentUser && service.IsFinished == false))
            };
            return View(docCreateViewModel);
        }

        [HttpPost]
        public IActionResult Index(DocCreateViewModel model)
        {
            var allSelectedServices = model.Services?.Where(service => service.IsChecked == true);
            if (allSelectedServices == null || allSelectedServices.Count() == 0)
            {
                ViewData["ErrorMessage"] = "Оберіть щонайменше одну послугу";
            }
            else
            {

                return RedirectToAction("Index", "Home");
            }

            model.Services = new List<ServiceViewModel>();
            return View(model);
        }

        [HttpGet("usdhighestexchangerate")]
        public async Task<JsonResult> BestUSDExchangeRate()
        {
            return Json(await _currencyHandler.GetHighestMonthRate());
        }

    }
}