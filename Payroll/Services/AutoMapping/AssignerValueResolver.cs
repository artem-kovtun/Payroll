using AutoMapper;
using Payroll.Models;
using Payroll.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.AutoMapping
{
    public class AssignerValueResolver : IValueResolver<DocCreateFormViewModel, ActGenerationViewModel, AssignerViewModel>
    {
        private PayrollDbContext _db { get; set; }
        private IMapper _mapper { get; set; }

        public AssignerValueResolver(PayrollDbContext dbContext, IMapper mapper)
        {
            _db = dbContext;
            _mapper = mapper;
        }

        public AssignerViewModel Resolve(DocCreateFormViewModel source, ActGenerationViewModel destination, AssignerViewModel destMember, ResolutionContext context)
        {
            return _mapper.Map<AssignerViewModel>(_db.Assigners.FirstOrDefault(assigner => assigner.AssignerId == source.ChosenAssignerId));
        }
    }
}
