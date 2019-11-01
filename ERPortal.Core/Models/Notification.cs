using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class Notification : BaseEntity
    {
        public string UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }

        public string ERApplicationId { get; set; }
        public virtual ERApplication ERApplication { get; set; }

        public string Text { get; set; }
        public Boolean IsRead { get; set; }
    }
}
