using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ERPortal.Core.Models
{
    public class StatusMaster : BaseEntity
    {
        [Display(Name = "Status")]
        public string CustStatusId { get; set; }

        [Display(Name = "Description")]
        public string Status { get; set; }       
    }

}
