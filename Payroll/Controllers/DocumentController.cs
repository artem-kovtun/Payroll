using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll.Models;
using Payroll.Models.Views;

namespace Payroll.Controllers
{
    [Route("document")]
    public class DocumentController : Controller
    {
        private PayrollDbContext _db { get; set; }
        private IMapper _mapper { get; set; }

        public DocumentController(PayrollDbContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        private User CurrentUser
        {
            get
            {
                return _db.Users.FirstOrDefault(user => user.Username == HttpContext.User.Identity.Name);
            }
        }

        [HttpGet("history")]
        public IActionResult UserHistory()
        {
            var documents = _mapper.Map<List<DocumentViewModel>>(_db.Documents.Where(doc => doc.Creator == CurrentUser).Include(doc => doc.Services).Include(doc => doc.Assigner).ToList());
            foreach(var document in documents)
            {
                document.Services = _mapper.Map<List<ServiceViewModel>>(_db.DocumentService.Where(doc => doc.DocumentId == document.DocumentId).Select(doc => doc.Service)).ToList();
            }
            return View(documents);
        }
    }
}