using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPortal.Core.Models;
using System.ComponentModel.DataAnnotations;
namespace ERPortal.Core.ViewModels
{
    public class ForwardAppViewModel
    {
        public Comment Comment { get; set; }
        public ForwardApplication ForwardApplication { get; set; }
        [Display(Name = "Forward To")]
        public string[] ReciverIdSelectList { get; set; }
      
        //public IEnumerable<UserAccount> ReciverId { get; set; }
    }
}
