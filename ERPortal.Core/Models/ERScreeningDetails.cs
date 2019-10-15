using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class ERScreeningDetails: BaseEntity
    {
        // Operator Section
        public virtual ERScreeningInstitute IssuingInstitute { get; set; }
        public string ReportDocumentPath { get; set; }
        public Boolean? FirstOrderScreening { get; set; }
        public Boolean? SecondOrderScreening { get; set; }
        public Boolean? ThirdOrderScreening { get; set; }

        // DGH Section
        public Boolean? ApprovalStatus { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public DateTime? DateOfLastApproval { get; set; }
    }
}
