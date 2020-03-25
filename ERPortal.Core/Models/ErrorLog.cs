using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class ErrorLog : BaseEntity
    {
        public string ERApplicationId { get; set; }
        //public virtual ERApplication ERApplication { get; set; }
        public string PageUrl { get; set; }
        public string ErrorText { get; set; }
        public string UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
      
        public virtual ErrorStatus ErrorStatus { get; set; }
    }
    

}
