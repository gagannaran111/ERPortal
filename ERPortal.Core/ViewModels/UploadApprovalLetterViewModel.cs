using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPortal.Core.Models;
namespace ERPortal.Core.ViewModels
{
   public class UploadApprovalLetterViewModel
    {
        public  Comment Comment{ get; set; }
        public UploadFile UploadFile { get; set; }
        public ForwardApplication ForwardApplication { get; set; }
    }
}
