using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERPortal.Core.Models
{
    public class ForwardApplication : BaseEntity
    {
        public string ERApplicationId { get; set; }
        public virtual ERApplication ERApplication { get; set; }

        public string Sender { get; set; }
        public string Reciever { get; set; }
        public string Subject { get; set; }
        public string CommentRefId { get; set; }
        // public virtual  Comment Comment { get; set; }
        public string FileRef { get; set; }
        [Display(Name = "File Status")]
        public FileStatus FileStatus { get; set; }      
    }
   
}
