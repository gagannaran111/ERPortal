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
        public string EmailID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string OrganisationId { get; set; }
        [Display(Name = "Organisation")]
        public virtual Organisation Organisation { get; set; }
    }
}
