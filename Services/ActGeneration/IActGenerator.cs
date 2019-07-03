using Payroll.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.ActGeneration
{
    public interface IActGenerator
    {
        string Generate(ActGenerationViewModel model);
    }
}
