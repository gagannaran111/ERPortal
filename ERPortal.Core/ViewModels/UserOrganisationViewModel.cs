using ERPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.ViewModels
{
    public class UserOrganisationViewModel
    { 
        public UserAccount user { get; set; }
        public IEnumerable<Organisation> organisations { get; set; }
    }
}
