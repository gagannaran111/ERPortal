using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class ERScreeningDetail: BaseEntity
    {
        // Operator Section
        public string ERScreeningInstituteId { get; set; }
        public virtual ERScreeningInstitute ERScreeningInstitute { get; set; }

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
