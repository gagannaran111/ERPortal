using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ERPortal.Core.Models
{
    public class ERTechniques : BaseEntity
    {
        [Display(Name = "Method")]
        [Required]
        public ImplementaionType Method { get; set; }

        [Display(Name = "Type")]
        [Required]
        public string TechniqueType { get; set; }
        [Display(Name = "Name")]
        [Required]
        public string TechniqueName { get; set; }        
        public string Status { get; set; }
      
    }
}
