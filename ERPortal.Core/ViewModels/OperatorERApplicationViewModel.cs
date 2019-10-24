using ERPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.ViewModels
{
    public class OperatorERApplicationViewModel
    {
        public ERApplication ERApplications { get; set; }
        public IEnumerable<FieldType> FieldTypes { get; set; }
    }
}
