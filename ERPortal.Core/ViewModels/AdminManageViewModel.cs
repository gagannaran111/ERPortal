using ERPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.ViewModels
{
    public class AdminManageViewModel
    {
        public List<UserAccount> UserAccounts { get; set; }
        public List<FieldType> FieldTypes { get; set; }
        public List<Organisation> Organisations { get; set; }
        public List<UHCProductionMethod> UHCProductionMethods { get; set; }
        public List<ERScreeningInstitute> ERScreeningInstitutes { get; set; }
        public List<StatusMaster> StatusMasters { get; set; }

    }
}
