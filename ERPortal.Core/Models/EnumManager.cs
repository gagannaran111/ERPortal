using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ERPortal.Core.Models
{
    public static class EnumManger
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()?
                            .GetMember(enumValue.ToString())?
                            .First()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name;
        }
    }
    public enum FileStatus
    {
        [Display(Name = "Forward")]
        Forward,
        [Display(Name = "Recommended")]
        Recommended,
        [Display(Name = "Return With Comment Back")]
        CommentBack,
        [Display(Name = "Comment Resolved Review Application Again.")]
        ReviewAgain,
        [Display(Name = "Approval Letter")]
        ApprovalLetter
    }
    public enum UserRoleType
    {     
        [Display(Name = "Consultant Enhanced Recovery")]        
        ConsultantEnhancedRecovery,
        Operator,
        Coordinator,
        Hod,
        DG,
        ADG
    }
    public enum ProductionProfile
    {
        Incremental,
        BAU,
        Decreasing
    }
    public enum HydrocarbonType
    {
        Oil,
        Gas,
        UHC
    }
    public enum ImplementaionType
    {
        [Display(Name = "EOR Method")]
        EORMethod,
        [Display(Name = "IOR Recovery")]
        IORRecoveryMethod,
        [Display(Name = "EGR Method")]
        EGRMethod,
        [Display(Name = "IGR Recovery")]
        IGRRecoveryMethod,
        [Display(Name = "UHC Extraction")]
        UHCMethod
    }
    public enum OrganisationType
    {
        Operator,
        DGH,
        [Display(Name = "ER Committee")]
        ERCommittee,
        Others
    }
    public enum ErrorStatus
    {
        Resolved,
        Issued,
        Pending,
    }
    public enum AppStatus
    {
        [Display(Name ="Pending With Me")]
        PWM,
        [Display(Name = "Under Proccessing")]
        UP,
        [Display(Name = "Rejected")]
        RJ,
        [Display(Name = "Approved")]
        AP,
        [Display(Name = "New Application")]
        NA
    }   
    public enum ButtonStatus
    {
        [Display(Name = "Button Hide")]
        Hide,
        [Display(Name = "Button Show")]
        Show
    }
}
