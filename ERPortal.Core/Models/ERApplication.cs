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
        [Display(Name = "Application Ref. No.")]
        public string AppId { get; set; }
        [Required(ErrorMessage = "Valid Operator Name is required")]
        public string OrganisationId { get; set; }
        [Display(Name = "Operator Name")]

        public virtual Organisation Organisation { get; set; }
        [Required(ErrorMessage = "Choose Field Type")]

        public string FieldTypeId { get; set; }
        [Display(Name = "Field Type")]
        public virtual FieldType FieldType { get; set; }

        [Display(Name = "Field Name")]
        [Required(ErrorMessage = "Valid Field Type is required")]
        [CustomValidationClass("/.,!@#$%", ErrorMessage = "Invalid Data")]
        public string FieldName { get; set; }

        [Display(Name = "Type of Hydrocarbon")]
        [Required(ErrorMessage = "Valid Hydrocarbon Type is required")]
        public HydrocarbonType HydrocarbonType { get; set; }

        [Display(Name = "Type of Hydrocarbon Method Propose")]
        [Required(ErrorMessage = "Valid Hydrocarbon Type Method Propose is required")]
        public HydrocarbonMethod HydrocarbonMethod { get; set; }

        public string UHCProductionMethodId { get; set; }
        [Display(Name = "UHC Production Method")]
        public virtual UHCProductionMethod UHCProductionMethod { get; set; }

        [Display(Name = "Incentive Sought for Implementation")]
        [Required(ErrorMessage = "Valid Implementaion Type is required")]
        public ImplementaionType ImplementaionType { get; set; }

        [Display(Name = "Date of Discovery")]
        [Required(ErrorMessage = "Valid Date Of Discovery is required")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateOfDiscovery { get; set; }

        [Display(Name = "Date of Commencement of Commercial Production")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfInitialCommercialProduction { get; set; }

        [Display(Name = "Date of most recent Commercial Production")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]

        public DateTime? DateOfLastCommercialProduction { get; set; }

        [Display(Name = "Presently Under Production")]
        public YesNo? PresentlyUnderProduction { get; set; }

        public string ERScreeningDetailId { get; set; }
        [Display(Name = "Screening report")]
        public virtual ERScreeningDetail ERScreeningDetail { get; set; }

        [Display(Name = "Field OIIP")]
        public int? FieldOIIP { get; set; }

        [Display(Name = "Field GIIP")]
        public int? FieldGIIP { get; set; }

        [Display(Name = "ER Pilot Id")]
        public string ERPilotId { get; set; }
        [Display(Name = "ER Pilot")]
        public virtual ERPilot ERPilot { get; set; }
        [Display(Name = "ER Techniques Id")]
        public string? ERTechniquesId { get; set; }
        [Display(Name = "ER Techniques (Conventional)")]
        public virtual ERTechniques ERTechniques { get; set; }

        [Display(Name = "Any Additional remarks")]
        public string AdditonalRemarks { get; set; }
        public DateTime? SubmissionDate { get; set; }
      

    }
}
