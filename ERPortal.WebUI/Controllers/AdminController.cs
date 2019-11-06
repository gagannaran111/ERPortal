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
    public class AdminController : Controller
    {
        IRepository<FieldType> FieldTypeContext;
        IRepository<Organisation> OrganisationContext;
        IRepository<UserAccount> UserAccountContext;
        IRepository<UHCProductionMethod> UHCProductionMethodContext;

        public AdminController(IRepository<FieldType> _FieldTypeContext, IRepository<Organisation> _OrganisationContext, IRepository<UserAccount> _UserAccountContext, IRepository<UHCProductionMethod> _UHCProductionMethodContext)
        {
            FieldTypeContext = _FieldTypeContext;
            OrganisationContext = _OrganisationContext;
            UserAccountContext = _UserAccountContext;
            UHCProductionMethodContext = _UHCProductionMethodContext;

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

            return View(adminManageViewModel);
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

