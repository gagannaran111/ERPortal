using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class ERCMemberComments : BaseEntity
    {
        // Link to users table
        // Member ID reference
        public virtual ERApplication LinkedApplication { get; set; }
        public string Remarks { get; set; }
    }
}
