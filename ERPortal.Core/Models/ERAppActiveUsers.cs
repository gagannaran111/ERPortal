using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ERPortal.Core.Models
{
    public class ERAppActiveUsers : BaseEntity
    {
        public string ERApplicationId { get; set; }     
        public string UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
       // public string User_Role { get; set; }
        public string Dept_Id { get; set; }
        public Boolean Is_Active { get; set; }
        public string Status { get; set; }

    }
}
