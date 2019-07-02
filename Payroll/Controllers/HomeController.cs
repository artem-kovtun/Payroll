using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payroll.Models;
using Payroll.Models.ServiceResponses;
using Payroll.Models.Views;
using Payroll.Services.ActGeneration;
using Payroll.Services.Currency;

namespace Payroll.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IMapper _mapper { get; set; }
        private PayrollDbContext _db { get; set; }
        private ICurrencyHandler _currencyHandler { get; set; }
        private IActGenerator _actGenerator { get; set; }

        public HomeController(IMapper mapper, PayrollDbContext database, ICurrencyHandler currencyHandler, IActGenerator actGenerator)
        {
            _mapper = mapper;
            _db = database;
            _currencyHandler = currencyHandler;
            _actGenerator = actGenerator;
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
            var docCreateViewModel = new DocCreateFormViewModel()
            {           
                CustomUSDRate = _mapper.Map<UserProfileViewModel>(_db.UserProfiles.FirstOrDefault(profile => profile.User == CurrentUser))?.USDRate,
                WorkCompletionDate = DateTime.Now,
                Services = _mapper.Map<List<ServiceViewModel>>(_db.Services.Where(service => service.User == CurrentUser && service.IsFinished == false))
            };
            return View(docCreateViewModel);
        }

        [HttpPost]
        public IActionResult Index(DocCreateFormViewModel model)
        {
            var actGenerationViewModel = _mapper.Map<ActGenerationViewModel>(model);

            if (actGenerationViewModel.Services == null || actGenerationViewModel.Services.Count() == 0)
            {
                ViewData["ErrorMessage"] = "Оберіть щонайменше одну послугу";
                model.Services = model.Services ?? new List<ServiceViewModel>();
            }
            else
            {
                var userProfile = _db.UserProfiles.FirstOrDefault(profile => profile.User == CurrentUser);
                if (userProfile == null)
                {
                    ViewData["ErrorMessage"] = "Для створення документу необхідно заповнити профіль";
                }
                else
                {
                    actGenerationViewModel.Profile = _mapper.Map<UserProfileViewModel>(userProfile);

                    _actGenerator.Generate(actGenerationViewModel);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet("usdhighestexchangerate")]
        public async Task<JsonResult> BestUSDExchangeRate()
        {
            var response = await _currencyHandler.GetHighestMonthRate();
            if (response.Status == ServiceResponseStatus.Error)
            {
                // LOG ERROR response.Message
            }
            return Json(response.Data);
        }

    }
}