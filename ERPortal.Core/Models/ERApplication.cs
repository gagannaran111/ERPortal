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
        public virtual Operator OperatorName { get; set; }
        public virtual FieldType FieldTypes { get; set; }
        public string FieldName { get; set; }
        public DateTime DateOfDiscovery { get; set; }
        public DateTime? DateOfCommercialProduction { get; set; }
        public Boolean? PresentlyUnderProduction { get; set; }
        public Boolean? ERScreeningStatus { get; set; }
        public virtual ERScreeningDetails ScreeningReport { get; set; }
        public int? FieldOIIP { get; set; }
        public int? FieldGIIP { get; set; }
        public Boolean? PilotDesign { get; set; }
        public string PilotProductionProfile { get; set; }
        public Boolean? TechnicallyCompatible { get; set; }
        public Boolean? EconomicViability { get; set; }
        public string AdditonalRemarks { get; set; }
        public DateTime? SubmissionDate { get; set; }

        // DGH Section
        public Boolean? EligibleForFiscalIncentive { get; set; }
        public Boolean? DGHApprovalStatus { get; set; }
        public DateTime? DGHApprovalDate { get; set; }
        public virtual Comments DGHComments { get; set; }
        public string DGHFileAttachment { get; set; }
        public Boolean? PilotMandatory { get; set; }
        public Boolean? PilotReportApprovalStatus { get; set; }
        public virtual Comments DGHCommentsForPilot { get; set; }
        public string DGHFileAttachmentForPilot { get; set; }

        // ER Committee Section
        public virtual Comments ERCComments { get; set; }

        public Boolean? FinalApprovalStatus { get; set; }
    }
}
