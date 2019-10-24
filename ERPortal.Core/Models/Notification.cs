using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class Notification : BaseEntity
    {
        public virtual UserAccount UserID { get; set; }
        public virtual ERApplication LinkedERApplication { get; set; }
        public string Text { get; set; }
        public Boolean IsRead { get; set; }
    }
}
