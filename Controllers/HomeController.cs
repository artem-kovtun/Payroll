using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                Services = _mapper.Map<List<ServiceViewModel>>(_db.Services.Where(service => service.User == CurrentUser && service.IsFinished == false)),
                Assigners = _db.Assigners.Select(assigner => new SelectListItem()
                {
                    Value = assigner.AssignerId.ToString(),
                    Text = $"{assigner.Lastname} {assigner.Firstname[0]}. {assigner.Middlename[0]}."
                }).ToList()
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

                    var filename = _actGenerator.Generate(actGenerationViewModel);
                    SaveDocumentCreationInfo(actGenerationViewModel);

                    return RedirectToAction("Download", "Home", new { filename = filename });
                }
            }

            return View(model);
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery(Name = "filename")]string filename)
        {
            if (filename == null)
            {
                return Content("filename not present");
            }

            var filepath = Path.Combine(
               Directory.GetCurrentDirectory(),
               "wwwroot\\acts", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            System.IO.File.Delete(filepath);
            return File(memory, GetContentType(filepath), filename);
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

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        private void SaveDocumentCreationInfo(ActGenerationViewModel model)
        {
            var document = new Models.Document()
            {
                CreationDate = DateTime.Now,
                WorkCompletionDate = model.WorkCompletionDate,
                Assigner = _mapper.Map<Assigner>(model.Assigner),
                Services = new List<DocumentService>(),
                Creator = CurrentUser
            };
            foreach (var service in model.Services)
            {
                document.Services.Add(new DocumentService()
                {
                    Document = document,
                    Service = service
                });
            }

            _db.Documents.Add(document);
            _db.SaveChanges();
        }

    }
}