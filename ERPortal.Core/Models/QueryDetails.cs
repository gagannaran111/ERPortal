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
        public string CommentRefId { get; set; }
        //public virtual Comment CommentRef { get; set; }
        public string QueryParentId { get; set; }
        // public virtual QueryMaster QueryParent { get; set; }
        public int QuerySeq { get; set; }
        public string FileRefId { get; set; }
        public string Status { get; set; }
        public string CustQueryId { get; set; }
    }

    public class QueryMaster : BaseEntity
    {
        public string ERApplicationId { get; set; }
        public string Subject { get; set; }      
        public string QueryParentId { get; set; }
        public string CustQueryId { get; set; }

    }
    public class QueryUser : BaseEntity
    {
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public string QueryId { get; set; }
        // public virtual QueryDetails Query{ get; set; }

    }
}
