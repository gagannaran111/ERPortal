using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class Comments : BaseEntity
    {
        // Link to users table
        public virtual UserAccounts LinkedUser { get; set; }

        // Application ID reference
        [InverseProperty("DGHComments")]
        public virtual ICollection<ERApplication> ERAppDGHComments { get; set; }

        [InverseProperty("DGHCommentsForPilot")]
        public virtual ICollection<ERApplication> ERAppDGHCommentsForPilot { get; set; }

        [InverseProperty("ERCComments")]
        public virtual ICollection<ERApplication> ERAppERCComments { get; set; }

        public string Remarks { get; set; }
    }
}
