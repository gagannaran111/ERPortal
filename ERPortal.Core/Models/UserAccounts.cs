using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class UserAccounts : BaseEntity
    {
        public string EmailID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }  
        public virtual Operator OperatorName { get; set; }
    }
}
