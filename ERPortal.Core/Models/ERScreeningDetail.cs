﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ERPortal.Core.Models
{
    public class ERScreeningDetail: BaseEntity
    {
        // Operator Section
        [Required(ErrorMessage ="Enter Institute")]
        [Display(Name = "ER Screening Institute Id")]
        public string ERScreeningInstituteId { get; set; }
        
        [Display(Name = "ER Screening Institute")]
        public virtual ERScreeningInstitute ERScreeningInstitute { get; set; }
        [Display(Name = "Report Document Path")]             
        public string ReportDocumentPath { get; set; }
        [Display(Name = "First Order Screening")]
        [Required(ErrorMessage ="Required")]
       
        public Boolean? FirstOrderScreening { get; set; }
        [Display(Name = "Second Order Screening")]
        [Required(ErrorMessage = "Required")]
        public Boolean? SecondOrderScreening { get; set; }
        [Display(Name = "Third Order Screening")]
        [Required(ErrorMessage = "Required")]
        public Boolean? ThirdOrderScreening { get; set; }


        [Display(Name = "Comment for Select First Order Screening : (NO)")]        
        public string FirstOrderScrText { get; set; }
        [Display(Name = "Comment for Select Second Order Screening : (NO)")]        
        public string SecondOrderScrText { get; set; }
        [Display(Name = "Comment for Select Third Order Screening : (NO)")]        
        public string ThirdOrderScrText { get; set; }




        // DGH Section
        [Display(Name = "Approval Status")]
        public Boolean? ApprovalStatus { get; set; }
        [Display(Name = "Date of Submission")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfSubmission { get; set; }

        [Display(Name = "Date of Last Approval")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfLastApproval { get; set; }
    }
}
