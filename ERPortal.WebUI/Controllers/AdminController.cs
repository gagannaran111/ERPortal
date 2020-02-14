using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPortal.WebUI.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("Admin")]
    public class AdminController : Controller
    {
        IRepository<FieldType> FieldTypeContext;
        IRepository<Organisation> OrganisationContext;
        IRepository<UserAccount> UserAccountContext;
        IRepository<UHCProductionMethod> UHCProductionMethodContext;
        IRepository<ERScreeningInstitute> ERScreeningInstituteContext;
        IRepository<StatusMaster> StatusMasterContext;
        IRepository<AuditTrails> AuditTrailContext;
        IRepository<ERApplication> ERApplicationContext;
        IRepository<DepartmentType> DepartmentContext;
        public AdminController(IRepository<FieldType> _FieldTypeContext, IRepository<Organisation> _OrganisationContext, IRepository<UserAccount> _UserAccountContext, IRepository<UHCProductionMethod> _UHCProductionMethodContext, IRepository<ERScreeningInstitute> _ERScreeningInstituteContext, IRepository<StatusMaster> _StatusMasterContext, IRepository<AuditTrails> _AuditTrailContext, IRepository<ERApplication> _ERApplicationContext, IRepository<DepartmentType> _DepartmentContext)
        {
            FieldTypeContext = _FieldTypeContext;
            OrganisationContext = _OrganisationContext;
            UserAccountContext = _UserAccountContext;
            UHCProductionMethodContext = _UHCProductionMethodContext;
            ERScreeningInstituteContext = _ERScreeningInstituteContext;
            StatusMasterContext = _StatusMasterContext;
            AuditTrailContext = _AuditTrailContext;
            ERApplicationContext = _ERApplicationContext;
            DepartmentContext = _DepartmentContext;
        }
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Title = "Manage";

            AdminManageViewModel adminManageViewModel = new AdminManageViewModel();
            adminManageViewModel.FieldTypes = FieldTypeContext.Collection().ToList();
            adminManageViewModel.UserAccounts = UserAccountContext.Collection().ToList();
            adminManageViewModel.Organisations = OrganisationContext.Collection().ToList();
            adminManageViewModel.UHCProductionMethods = UHCProductionMethodContext.Collection().ToList();
            adminManageViewModel.ERScreeningInstitutes = ERScreeningInstituteContext.Collection().ToList();
            adminManageViewModel.StatusMasters = StatusMasterContext.Collection().ToList();
            adminManageViewModel.Departments = DepartmentContext.Collection().ToList();
            return View(adminManageViewModel);
        }
        public ActionResult AuditTrail()
        {
            ViewBag.Application = ERApplicationContext.Collection().Select(x=>new {
                value = x.Id,
                text =x.AppId
            }).ToList();

            return View();

        }
        public JsonResult AuditTrailData(string appid)
        {
            string Application = ERApplicationContext.Collection().Where(x => x.Id == appid).FirstOrDefault().AppId;
            var AuditTrailList = AuditTrailContext.Collection().Where(s => s.ERApplicationId == appid)
                .Select(x => new
                {
                    ERApplicationId = Application,
                    x.CreatedAt,
                    x.Receiver,
                    x.Sender,
                    x.ReceiverId,
                    x.StatusId,
                    x.Status,
                    x.QueryDetailsId,
                    x.Is_Active,
                    x.FileRefId,
                    x.Id
                }).OrderByDescending(d=>d.CreatedAt).ToList();
            return Json(AuditTrailList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxAdd(string targetPage)
        {
            object _genericObject;
            switch (targetPage)
            {
                case "FieldType":
                    _genericObject = new FieldType();
                    break;
                case "Organisation":
                    _genericObject = new Organisation();
                    break;
                case "User":
                    _genericObject = new UserOrganisationViewModel()
                    {
                        user = new UserAccount(),
                        organisations = OrganisationContext.Collection()
                    };
                    break;
                case "UHCProductionMethod":
                    _genericObject = new UHCProductionMethod();
                    break;
                case "ERScreeningInstitute":
                    _genericObject = new ERScreeningInstitute();
                    break;
                case "StatusMaster":
                    _genericObject = new StatusMaster();
                    break;
                case "Department":
                    _genericObject = new DepartmentType();
                    break;
                default:
                    _genericObject = null;
                    break;
            }
            if (null != _genericObject)
            {
                return View(targetPage, _genericObject);
            }
            else
            {

                return Content("<div class=\"alert alert-danger\" role=\"alert\"> An Error has occured </div>");
            }

        }

        [HttpPost]
        public ActionResult AjaxAdd(string targetPage, FormCollection collection)
        {
            object _genericObject = null;
            bool modelIsValid = false;
            switch (targetPage)
            {
                case "FieldType":
                    FieldType fieldType = new FieldType()
                    {
                        Type = collection["Type"]
                    };
                    modelIsValid = TryValidateModel(fieldType);
                    if (modelIsValid)
                    {
                        FieldTypeContext.Insert(fieldType);
                        FieldTypeContext.Commit();
                    }
                    else
                    {
                        _genericObject = fieldType;
                    }
                    break;
                case "Organisation":
                    Organisation organisation = new Organisation(collection["Name"], collection["Type"]);
                    modelIsValid = TryValidateModel(organisation);
                    if (TryValidateModel(modelIsValid))
                    {
                        OrganisationContext.Insert(organisation);
                        OrganisationContext.Commit();
                    }
                    else
                    {
                        _genericObject = organisation;
                    }
                    break;
                case "User":
                    UserAccount userAccount = new UserAccount()
                    {
                        EmailID = collection["user.EmailID"],
                        FirstName = collection["user.FirstName"],
                        LastName = collection["user.LastName"],
                        OrganisationId = collection["user.OrganisationId"]
                    };
                    modelIsValid = TryValidateModel(userAccount);
                    if (modelIsValid && null != collection["user.OrganisationId"])
                    {
                        UserAccountContext.Insert(userAccount);
                        UserAccountContext.Commit();
                    }
                    else
                    {
                        _genericObject = new UserOrganisationViewModel()
                        {
                            user = userAccount,
                            organisations = OrganisationContext.Collection()
                        };
                        modelIsValid = false;
                    }
                    break;
                case "UHCProductionMethod":
                    UHCProductionMethod uHCProductionMethod = new UHCProductionMethod()
                    {
                        Name = collection["Name"],
                        Description = collection["Description"]
                    };
                    modelIsValid = TryValidateModel(uHCProductionMethod);
                    if (modelIsValid)
                    {
                        UHCProductionMethodContext.Insert(uHCProductionMethod);
                        UHCProductionMethodContext.Commit();
                    }
                    else
                    {
                        _genericObject = uHCProductionMethod;
                    }
                    break;
                case "ERScreeningInstitute":
                    ERScreeningInstitute ERScreeningInstitute = new ERScreeningInstitute()
                    {
                        InstituteName = collection["InstituteName"],
                        ContactPerson = collection["ContactPerson"],
                        Address = collection["Address"],
                        EmailID = collection["EmailID"]
                    };
                    modelIsValid = TryValidateModel(ERScreeningInstitute);
                    if (modelIsValid)
                    {
                        ERScreeningInstituteContext.Insert(ERScreeningInstitute);
                        ERScreeningInstituteContext.Commit();
                    }
                    else
                    {
                        _genericObject = ERScreeningInstitute;
                    }
                    break;

                case "StatusMaster":
                    StatusMaster statusMaster = new StatusMaster()
                    {
                        Status = collection["Status"],
                        Is_Active = true
                    };
                    modelIsValid = TryValidateModel(statusMaster);
                    if (modelIsValid)
                    {
                        StatusMasterContext.Insert(statusMaster);
                        StatusMasterContext.Commit();
                    }
                    else
                    {
                        _genericObject = statusMaster;
                    }
                    break;

                case "Department":
                    DepartmentType department = new DepartmentType()
                    {
                        DeptName = collection["DeptName"],
                        SubDeptName=collection["SubDeptName"],
                        Is_Active=true
                    };
                    modelIsValid = TryValidateModel(department);
                    if (modelIsValid)
                    {
                        DepartmentContext.Insert(department);
                        DepartmentContext.Commit();
                    }
                    else
                    {
                        _genericObject = department;
                    }
                    break;
                default:
                    return Content("Error");
            }
            if (modelIsValid)
            {
                return Content("Success");
            }
            else
            {
                return View(targetPage, _genericObject);
            }

        }
    }
}

