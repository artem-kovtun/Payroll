using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Payroll.Models;
using Payroll.Models.Views;

namespace Payroll.Controllers
{
    [Route("services")]
    public class ServiceController : Controller
    {
        private PayrollDbContext _db { get; set; }
        private IMapper _mapper { get; set; }

        public ServiceController(PayrollDbContext database, IMapper mapper)
        {
            _db = database;
            _mapper = mapper;
        }

        private User CurrentUser
        {
            get
            {
                return _db.Users.FirstOrDefault(user => user.Username == HttpContext.User.Identity.Name);
            }
        }

        [Route("all")]
        public IActionResult All()
        {
            var services = _db.Services.Where(service => service.User == CurrentUser && service.IsFinished == false);
            var servicesViewModels = _mapper.Map<IEnumerable<ServiceViewModel>>(services);
            return View(servicesViewModels);
        }

        [HttpPost("finish/<id:int>")]
        public async Task<IActionResult> Finish(int id)
        {
            var currentService = _db.Services.FirstOrDefault(service => service.User == CurrentUser && service.ServiceId == id);
            if (currentService != null)
            {
                currentService.IsFinished = true;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("All", "Service");
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save(ServiceViewModel model) 
        {
            var service = _mapper.Map<Service>(model);
            service.User = CurrentUser;
            if (service.ServiceId == 0)
            {
                await _db.Services.AddAsync(service);           
            }
            else
            {
                _db.Services.Update(service);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("All", "Service");
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public JsonResult GetService(int id)
        {
            var service = _db.Services.FirstOrDefault(s => s.ServiceId == id);
            var serviceViewModel = _mapper.Map<ServiceViewModel>(service);
            return Json(serviceViewModel);
        }
    }
}