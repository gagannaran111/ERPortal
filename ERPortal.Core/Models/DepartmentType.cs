using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
  public  class DepartmentType:BaseEntity
    {
        public string  DeptName { get; set; }
        public string SubDeptName { get; set; }
        public Boolean Is_Active { get; set; }
    }
}
