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
            ViewBag.Application = ERApplicationContext.Collection().Select(x => new
            {
                value = x.Id,
                text = x.AppId
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
                   // x.Status,
                    x.QueryDetailsId,
                    x.Is_Active,
                    x.FileRefId,
                    x.Id
                }).OrderByDescending(d => d.CreatedAt).ToList();
            return Json(AuditTrailList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxAdd(string targetPage, string Id)
        {
            object _genericObject;
            switch (targetPage)
            {
                case "FieldType":
                    _genericObject = Id != null ? FieldTypeContext.Collection().Where(x => x.Id == Id).FirstOrDefault() : new FieldType();
                    break;
                case "Organisation":
                    _genericObject = Id != null ? OrganisationContext.Collection().Where(x => x.Id == Id).FirstOrDefault() : new Organisation();
                    break;
                case "User":
                    _genericObject = new UserOrganisationViewModel()
                    {
                        user = new UserAccount(),
                        organisations = OrganisationContext.Collection()
                    };
                    break;
                case "UHCProductionMethod":
                    _genericObject = Id != null ? UHCProductionMethodContext.Collection().Where(x => x.Id == Id).FirstOrDefault() : new UHCProductionMethod();
                    break;
                case "ERScreeningInstitute":
                    _genericObject = Id != null ? ERScreeningInstituteContext.Collection().Where(x => x.Id == Id).FirstOrDefault() : new ERScreeningInstitute();
                    break;
                case "StatusMaster":
                    _genericObject = Id != null ? StatusMasterContext.Collection().Where(x => x.Id == Id).FirstOrDefault() : new StatusMaster();
                    break;
                case "Department":
                    _genericObject = Id != null ? DepartmentContext.Collection().Where(x => x.Id == Id).FirstOrDefault() : new DepartmentType();
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
            string id = collection["Id"];
            string msg = "";
            bool modelIsValid = false;

            switch (targetPage)
            {
                case "FieldType":
                    var FieldType = FieldTypeContext.Collection().Where(x => x.Id == id).FirstOrDefault();

                    if (FieldType != null)
                    {
                        FieldType.Type = collection["Type"];

                    }
                    else
                    {
                        FieldType fieldType = new FieldType()
                        {
                            Type = collection["Type"]
                        };
                        modelIsValid = TryValidateModel(fieldType);
                        if (modelIsValid)
                        {
                            FieldTypeContext.Insert(fieldType);
                           
                        }
                        else
                        {
                            _genericObject = fieldType;
                        }
                    }
                    FieldTypeContext.Commit();
                    break;
                case "Organisation":
                    var Organisation = OrganisationContext.Collection().Where(x => x.Id == id).FirstOrDefault();

                    if (Organisation != null)
                    {
                        switch (collection["Type"])
                        {
                            case "0":
                                Organisation.Type = OrganisationType.Operator;
                                break;
                            case "1":
                                Organisation.Type = OrganisationType.DGH;
                                break;
                            case "2":
                                Organisation.Type = OrganisationType.ERCommittee;
                                break;
                            default:
                                Organisation.Type = OrganisationType.Others;
                                break;
                        }
                        Organisation.Name = collection["Name"];

                    }
                    else
                    {

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
                    }
                    OrganisationContext.Commit();
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
                    var UHCProductionMethod = UHCProductionMethodContext.Collection().Where(x => x.Id == id).FirstOrDefault();

                    if (UHCProductionMethod != null)
                    {
                        UHCProductionMethod.Name = collection["Name"];
                        UHCProductionMethod.Description = collection["Description"];
                    }

                    else
                    {
                        UHCProductionMethod uHCProductionMethod = new UHCProductionMethod()
                        {
                            Name = collection["Name"],
                            Description = collection["Description"]
                        };
                        modelIsValid = TryValidateModel(uHCProductionMethod);
                        if (modelIsValid)
                        {
                            UHCProductionMethodContext.Insert(uHCProductionMethod);
                           
                        }
                        else
                        {
                            _genericObject = uHCProductionMethod;
                        }
                    }
                    UHCProductionMethodContext.Commit();
                    break;
                case "ERScreeningInstitute":

                    var ERScreeningInstitute = ERScreeningInstituteContext.Collection().Where(x => x.Id == id).FirstOrDefault();

                    if (ERScreeningInstitute != null)
                    {
                        ERScreeningInstitute.InstituteName = collection["InstituteName"];
                        ERScreeningInstitute.ContactPerson = collection["ContactPerson"];
                        ERScreeningInstitute.Address = collection["Address"];
                        ERScreeningInstitute.EmailID = collection["EmailID"];
                    }
                    else
                    {
                        ERScreeningInstitute ERScreeningInstitute1 = new ERScreeningInstitute()
                        {
                            InstituteName = collection["InstituteName"],
                            ContactPerson = collection["ContactPerson"],
                            Address = collection["Address"],
                            EmailID = collection["EmailID"]
                        };
                        modelIsValid = TryValidateModel(ERScreeningInstitute1);
                        if (modelIsValid)
                        {
                            ERScreeningInstituteContext.Insert(ERScreeningInstitute1);
                           
                        }
                        else
                        {
                            _genericObject = ERScreeningInstitute1;
                        }
                    }
                    ERScreeningInstituteContext.Commit();
                    break;

                case "StatusMaster":
                   
                    var Status = StatusMasterContext.Collection().Where(x => x.Id == id).FirstOrDefault();

                    if (Status != null)
                    {
                        Status.CustStatusId = collection["CustStatusId"];
                        Status.Status = collection["Status"];
                    }

                    else
                    {
                        StatusMaster statusMaster = new StatusMaster()
                        {
                            CustStatusId = collection["CustStatusId"],
                            Status = collection["Status"],
                            Is_Active = true
                        };
                        modelIsValid = TryValidateModel(statusMaster);
                        if (modelIsValid)
                        {
                            StatusMasterContext.Insert(statusMaster);
                          
                        }
                        else
                        {
                            _genericObject = statusMaster;
                        }
                    }
                    StatusMasterContext.Commit();
                    break;

                case "Department":
                
                    var Dept = DepartmentContext.Collection().Where(x => x.Id == id).FirstOrDefault();

                    if (Dept != null)
                    {
                        Dept.DeptName = collection["DeptName"];
                        Dept.SubDeptName = collection["SubDeptName"];
                        DepartmentContext.Update(Dept);
                    }
                    else
                    {
                        DepartmentType department = new DepartmentType()
                        {
                            DeptName = collection["DeptName"],
                            SubDeptName = collection["SubDeptName"],
                            Is_Active = true
                        };
                        modelIsValid = TryValidateModel(department);
                        if (modelIsValid)
                        {

                            DepartmentContext.Insert(department);
                        }
                        else
                        {
                            _genericObject = department;
                        }
                    }
                    DepartmentContext.Commit();

                    break;
                default:
                    return Content("Error");
            }
            if (_genericObject==null)
            {
                return Content("Success");
            }
            else
            {
                return View(targetPage, _genericObject);
            }

        }


        [HttpPost]
        public JsonResult DeleteData(string targetdata, string Id)
        {
            string msg = "Id : " + Id + " Delete Successfully";
            switch (targetdata)
            {
                case "FieldType":

                    FieldTypeContext.Delete(Id);
                    FieldTypeContext.Commit();
                    break;
                case "Organisation":

                    OrganisationContext.Delete(Id);
                    OrganisationContext.Commit();
                    break;
                case "User":

                    UserAccountContext.Delete(Id);
                    UserAccountContext.Commit();
                    break;
                case "UHCProductionMethod":

                    UHCProductionMethodContext.Delete(Id);
                    UHCProductionMethodContext.Commit();
                    break;
                case "ERScreeningInstitute":

                    ERScreeningInstituteContext.Delete(Id);
                    ERScreeningInstituteContext.Commit();
                    break;
                case "StatusMaster":

                    StatusMasterContext.Delete(Id);
                    StatusMasterContext.Commit();
                    break;
                case "Department":
                    DepartmentContext.Delete(Id);
                    DepartmentContext.Commit();
                    break;
                default:
                    msg = "Something Went Wrong. Try Again";
                    break;
            }

            return Json(msg);
        }
    }
}

