﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPortal.Core.Models;
using System.ComponentModel.DataAnnotations;
namespace ERPortal.Core.ViewModels
{
   public class ERCViewModel
    {
        // Operator UI
        public ERApplication ERApplications { get; set; }
        public IEnumerable<UHCProductionMethod> UHCProductionMethods { get; set; }
        public IEnumerable<FieldType> FieldTypes { get; set; }
        public IEnumerable<Organisation> organisationTypes { get; set; }
        // ER Screening Details
        public ERScreeningDetail eRScreeningDetail { get; set; }
        [Display(Name = "ER Screening Institute")]
        public IEnumerable<ERScreeningInstitute> eRScreeningInstitutes { get; set; }
        public IEnumerable<Comment> comment { get; set; }

    }
}
