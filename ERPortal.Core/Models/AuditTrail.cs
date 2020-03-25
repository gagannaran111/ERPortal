using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ERPortal.Core.Models
{
    public class AuditTrails : BaseEntity
    {
        public string ERApplicationId { get; set; }

        public string FileRefId { get; set; }

        public string StatusId { get; set; }
       // public virtual StatusMaster Status { get; set; }
        public string QueryDetailsId { get; set; }
        public string SenderId { get; set; }
        public virtual UserAccount Sender { get; set; }
        public string ReceiverId { get; set; }
        public virtual UserAccount Receiver { get; set; }       
    }
}
