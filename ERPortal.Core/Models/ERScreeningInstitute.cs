using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ERPortal.Core.Models
{
    public class ERScreeningInstitute: BaseEntity
    {
        [Display(Name ="Institute Name")]
        public string InstituteName { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Email Id")]
        public string EmailID { get; set; }
    }
}
