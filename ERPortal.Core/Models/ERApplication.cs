using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPortal.Core.CustomValidation;
namespace ERPortal.Core.Models
{
    public class ERApplication : BaseEntity
    {
        #region  // Operator Section  
        [Display(Name = "Application Ref. No.")]
        public string AppId { get; set; }
        public string OrganisationId { get; set; }
        [Display(Name = "Operator Name")]
        [Required(ErrorMessage = "Valid Operator Name is required")]
        public virtual Organisation Organisation { get; set; }
        [Required(ErrorMessage = "Choose Field Type")]
      
        public string FieldTypeId { get; set; }
        [Display(Name = "Field Type")]
        public virtual FieldType FieldType { get; set; }

        [Display(Name = "Field Name")]
        [Required(ErrorMessage = "Valid Field Type is required")]       
        [CustomValidationClass("/.,!@#$%",ErrorMessage ="Invalid Data")]
        public string FieldName { get; set; }

        [Display(Name ="Type of Hydrocarbon")]
        [Required(ErrorMessage = "Valid Hydrocarbon Type is required")]
        public HydrocarbonType HydrocarbonType { get; set; }

        public string UHCProductionMethodId { get; set; }
        [Display(Name = "UHC Production Method")]
        public virtual UHCProductionMethod UHCProductionMethod { get; set; }

        [Display(Name = "Incentive Sought for Implementation")]
        [Required(ErrorMessage = "Valid Implementaion Type is required")]
        public ImplementaionType ImplementaionType { get; set; }
      
        [Display(Name = "Date of Discovery")]
        [Required(ErrorMessage = "Valid Date Of Discovery is required")]      
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}",ApplyFormatInEditMode = true)]
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
        #endregion
        #region   
        // DGH Section
        [Display(Name = "Eligible For Fiscal Incentive")]
        public Boolean? EligibleForFiscalIncentive { get; set; }
        [Display(Name = "DGH Approval Status")]
        public Boolean? DGHApprovalStatus { get; set; }
        [Display(Name = "DGH Approval Date")]
        public DateTime? DGHApprovalDate { get; set; }
        [Display(Name = "DGH File Attachment")]
        public string DGHFileAttachment { get; set; }
        [Display(Name = "Pilot Mandatory")]
        public Boolean? PilotMandatory { get; set; }
        [Display(Name = "Pilot Report Approval Status")]
        public Boolean? PilotReportApprovalStatus { get; set; }
        [Display(Name = "DGH File Attachment For Pilot")]
        public string DGHFileAttachmentForPilot { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        #endregion
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

    public enum HydrocarbonType
    {
        Oil,
        Gas,
        UHC
    }

    public enum ImplementaionType
    {
        [Display(Name = "EOR Method")]
        EORMethod,
        [Display(Name = "EGR Method")]
        EGRMethod,
        [Display(Name = "UHC Extraction")]
        UHCMethod
    }

}
