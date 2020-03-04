using ERPortal.Core.Contracts;
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
        public Organisation()
        {

        }

        public Organisation(string _Name, string _TypeId)
        {
            this.Name = _Name;
            OrganisationType _type;
            switch (_TypeId)
            {
                case "0": _type= OrganisationType.Operator;
                    break;
                case "1": _type= OrganisationType.DGH;
                    break;
                case "2": _type=OrganisationType.ERCommittee;
                    break;
                default: _type= OrganisationType.Others;
                    break;
            }
            this.Type = _type;
        }
        public string Name { get; set; }

        [Required(ErrorMessage = "Valid Organisation Type is required")]
        public OrganisationType Type { get; set; }
    }

    
}
