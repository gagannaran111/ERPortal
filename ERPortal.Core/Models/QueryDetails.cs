using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ERPortal.Core.Models
{
    public class QueryDetails : BaseEntity
    {
        public string ERApplicationId { get; set; }
        
        public string Subject { get; set; }
        public string Text { get; set; }
        public string QueryParent { get; set; }
        public int QuerySeq { get; set; }
        public string FileRefId { get; set; }
       
        public string UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public Boolean Is_Active { get; set; }

    }

    public class QueryMaster :BaseEntity
    {
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public Boolean Is_Active { get; set; }
        public string QueryRefId { get; set; }
        public virtual QueryDetails QueryRef { get; set; }
       // public string ERApplicationId  { get; set; }
    }
}
