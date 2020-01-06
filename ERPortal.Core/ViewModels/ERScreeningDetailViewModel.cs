using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPortal.Core.Models;
using System.ComponentModel.DataAnnotations;
namespace ERPortal.Core.ViewModels
{
  public class ERScreeningDetailViewModel
    {
        public ERScreeningDetail eRScreeningDetail { get; set; }
        [Display(Name="ER Screening Institute")]
        public IEnumerable<ERScreeningInstitute> eRScreeningInstitutes { get; set; }
    }
}
