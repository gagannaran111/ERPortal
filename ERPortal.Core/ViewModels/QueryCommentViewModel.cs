using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPortal.Core.Models;
using System.ComponentModel.DataAnnotations;
namespace ERPortal.Core.ViewModels
{
    public class QueryCommentViewModel
    {
        public QueryDetails QueryDetails { get; set; }
        public QueryMaster QueryMaster { get; set; }
        public Comment Comment { get; set; }
        public IEnumerable<ListItemData> ReciverId { get; set; }
        [Display(Name = "Forward To")]
        public string[] ReciverIdSelectList { get; set; }
        [Display(Name = "Query Type")]
        public QueryStatus QueryStatus { get; set; }
        public QueryUser QueryUser { get; set; }
    }
    public enum QueryStatus
    {
        [Display(Name = "Query Raised")]
        Raised,
        [Display(Name = "Query Forward")]
        Forward,
        [Display(Name = "Query Reply")]
        Reply,
        [Display(Name = "Query Resolved")]
        Resolved

    }
}
