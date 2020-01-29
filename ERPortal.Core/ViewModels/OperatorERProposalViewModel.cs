using ERPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.ViewModels
{
    public class OperatorERProposalViewModel
    {
        
        public ERApplication ERApplications { get; set; }
        public IEnumerable<UHCProductionMethod> UHCProductionMethods { get; set; }
        public IEnumerable<FieldType> FieldTypes { get; set; }

        public IEnumerable<Organisation> organisationTypes { get; set; }
        public IEnumerable<ERScreeningInstitute> eRScreeningInstitutes { get; set; }

        // public IEnumerable<UploadFile> UploadFiles { get; set; }
    }
}
