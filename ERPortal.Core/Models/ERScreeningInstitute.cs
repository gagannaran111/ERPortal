using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class ERScreeningInstitute: BaseEntity
    {
        public string InstituteName { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string EmailID { get; set; }
    }
}
