using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ERPortal.Core.Models
{
    public class ERPilot : BaseEntity
    {
        [Display(Name = "Pilot Design carried out")]
        public YesNo PilotDesign { get; set; }

        [Display(Name = "Pilot Mandatory")]
        public YesNo? PilotMandatory { get; set; }

        [Display(Name = "Technically Compatible")]
        public TechnicallyCompatible TechnicallyCompatible { get; set; }

        [Display(Name = "Economic Viable")]
        public YesNo PilotEconomicViability { get; set; }

        [Display(Name = "NPV")]
        public PositiveNegative? PilotNPV { get; set; }

        [Display(Name = "IRR")]
        public PositiveNegative? PilotIRR { get; set; }
        [Display(Name = "Pilot Phase Commencement Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? PilotStartDate { get; set; }

        [Display(Name = "Pilot Phase Culmination Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? PilotEndDate { get; set; }

        [Display(Name = "Economic Viable")]
        public YesNo? FullFillEconomicViability { get; set; }

        [Display(Name = "NPV")]
        public PositiveNegative? FullFillNPV { get; set; }

        [Display(Name = "IRR")]
        public PositiveNegative? FullFillIRR { get; set; }

        [Display(Name = "Commencement Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? FullFillStartDate { get; set; }

        [Display(Name = "Culmination Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? FullFillEndDate { get; set; }

        [Display(Name = "Eligible For Fiscal Incentive")]
        public YesNo? EligibleForFiscalIncentive { get; set; }
      

    }
}
