using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class Organisation : BaseEntity
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Valid Organisation Type is required")]
        public OrganisationType Type { get; set; }
    }

    public enum OrganisationType
    {
        Operator,
        DGH,
        ERCommittee,
        Others
    }
}
