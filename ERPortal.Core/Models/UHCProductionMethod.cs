using ERPortal.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class UHCProductionMethod : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
