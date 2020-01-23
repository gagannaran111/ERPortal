using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class UserAccount : BaseEntity
    {
        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "Valid Email ID is required")]
        public string EmailID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Valid Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Valid Organisation is required")]
        public string OrganisationId { get; set; }
        [Display(Name = "Organisation")]
        public virtual Organisation Organisation { get; set; }
        public string UserRole { get; set; }
    }
}
