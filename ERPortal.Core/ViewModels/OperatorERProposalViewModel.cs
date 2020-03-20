using ERPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ERPortal.Core.ViewModels
{
    public class OperatorERProposalViewModel
    {       
        public ERApplication ERApplications { get; set; }
        public IEnumerable<UHCProductionMethod> UHCProductionMethods { get; set; }
        public IEnumerable<FieldType> FieldTypes { get; set; }       
        public IEnumerable<ERScreeningInstitute> eRScreeningInstitutes { get; set; }
    }
}
