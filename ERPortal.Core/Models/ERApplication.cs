using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class ERApplication : BaseEntity
    {
        // Operator Section
        public string OrganisationId { get; set; }
        [Display(Name = "Operator Name")]
        [Required(ErrorMessage = "Valid Operator Name is required")]
        public virtual Organisation Organisation { get; set; }

        public string FieldTypeId { get; set; }
        [Display(Name = "Field Type")]
        [Required(ErrorMessage = "Valid Field Type is required")]
        public virtual FieldType FieldType { get; set; }

        [Display(Name = "Field Name")]
        public string FieldName { get; set; }

        [Display(Name = "Date of Discovery")]
        [DataType(DataType.Date)]
        public DateTime DateOfDiscovery { get; set; }

        [Display(Name = "Date of Commencement of Commercial Production")]
        [DataType(DataType.Date)]
        public DateTime? DateOfInitialCommercialProduction { get; set; }

        [Display(Name = "Date of most recent Commercial Production")]
        [DataType(DataType.Date)]
        public DateTime? DateOfLastCommercialProduction { get; set; }

        [Display(Name = "Presently Under Production")]
        public Boolean? PresentlyUnderProduction { get; set; }

        [Display(Name = "ER Screening Carried out")]
        public Boolean? ERScreeningStatus { get; set; }

        public string ERScreeningDetailId { get; set; }
        [Display(Name = "Screening report")]
        public virtual ERScreeningDetail ERScreeningDetail { get; set; }

        [Display(Name = "Field OIIP")]
        public int? FieldOIIP { get; set; }

        [Display(Name = "Field GIIP")]
        public int? FieldGIIP { get; set; }

        [Display(Name = "Pilot Design carried out")]
        public Boolean? PilotDesign { get; set; }

        [Display(Name = "Pilot Production Profile")]
        public ProductionProfile? PilotProductionProfile { get; set; }

        [Display(Name = "Technically Compatible")]
        public Boolean? TechnicallyCompatible { get; set; }

        [Display(Name = "Economic Viability")]
        public Boolean? EconomicViability { get; set; }

        [Display(Name = "Any Additional remarks")]
        public string AdditonalRemarks { get; set; }
        public DateTime? SubmissionDate { get; set; }

        // DGH Section
        public Boolean? EligibleForFiscalIncentive { get; set; }
        public Boolean? DGHApprovalStatus { get; set; }
        public DateTime? DGHApprovalDate { get; set; }
        public string DGHFileAttachment { get; set; }
        public Boolean? PilotMandatory { get; set; }
        public Boolean? PilotReportApprovalStatus { get; set; }
        public string DGHFileAttachmentForPilot { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        // ER Committee Section
        // Add fields

        public Boolean? FinalApprovalStatus { get; set; }
    }

    public enum ProductionProfile
    {
        Incremental,
        BAU,
        Decreasing
    }
}
