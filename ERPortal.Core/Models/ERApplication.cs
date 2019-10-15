using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class ERApplication : BaseEntity
    {
        // Operator Section
        public virtual ICollection<FieldType> FieldTypes { get; set; }
        public string FieldName { get; set; }
        public DateTime DateOfDiscovery { get; set; }
        public DateTime? DateOfCommercialProduction { get; set; }
        public Boolean? PresentlyUnderProduction { get; set; }
        public Boolean? ERScreeningStatus { get; set; }
        public virtual ERScreeningDetails ScreeningReport { get; set; }
        public Nullable<int> FieldOIIP { get; set; }
        public Nullable<int> FieldGIIP { get; set; }
        public Boolean? PilotDesign { get; set; }
        public string PilotProductionProfile { get; set; }
        public Boolean? TechnicallyCompatible { get; set; }
        public Boolean? EconomicViability { get; set; }
        public string AdditonalRemarks { get; set; }

        // DGH Section
        public Boolean? EligibleForFiscalIncentive { get; set; }
        public DateTime? DGHApprovalStatus { get; set; }
        public string DGHComments { get; set; }
        public string DGHFileAttachment { get; set; }
        public Boolean? PilotMandatory { get; set; }
        public Boolean? PilotReportApprovalStatus { get; set; }
        public string DGHCommentsForPilot { get; set; }
        public string DGHFileAttachmentForPilot { get; set; }

        // ER Committee Section
        public virtual ICollection<ERCMemberComments> ERCComments { get; set; }

        public Boolean? FinalApprovalStatus { get; set; }
    }
}
