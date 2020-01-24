using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class StatusMaster : BaseEntity
    {
        public string Status { get; set; }
        public Boolean Is_Active { get; set; }
    }
}
