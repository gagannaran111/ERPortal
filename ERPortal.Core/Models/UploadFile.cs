using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPortal.Core.Contracts;


namespace ERPortal.Core.Models
{
   public class UploadFile :BaseEntity
    {
        public string FIleRef { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string CreatedBy { get; set; }
        public string NewFileName { get; set; }
    }
}
