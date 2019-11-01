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

        public AdminController(IRepository<FieldType> _FieldTypeContext, IRepository<Organisation> _OrganisationContext, IRepository<UserAccount> _UserAccountContext)
        {
            FieldTypeContext = _FieldTypeContext;
            OrganisationContext = _OrganisationContext;
            UserAccountContext = _UserAccountContext;

        }
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Title = "Manage";

            AdminManageViewModel adminManageViewModel = new AdminManageViewModel();
            adminManageViewModel.FieldTypes = FieldTypeContext.Collection().ToList();
            adminManageViewModel.UserAccounts = UserAccountContext.Collection().ToList();
            adminManageViewModel.Organisations = OrganisationContext.Collection().ToList();

            return View(adminManageViewModel);
        }

        public ActionResult AjaxAddFieldType()
        {
            FieldType fieldType = new FieldType();
            return View(fieldType);
        }

        [HttpPost]
        public ActionResult AjaxAddFieldType(FieldType fieldType)
        {
            if (!ModelState.IsValid)
            {
                return View(fieldType);
            }
            else
            {
                FieldTypeContext.Insert(fieldType);
                FieldTypeContext.Commit();

                return Content("Success");
            }
        }

        public ActionResult AjaxAddUser()
        {
            UserOrganisationViewModel userViewModel = new UserOrganisationViewModel();

            userViewModel.user = new UserAccount();
            userViewModel.organisations = OrganisationContext.Collection();

            return View(userViewModel);
        }

        [HttpPost]
        public ActionResult AjaxAddUser(UserOrganisationViewModel userAccount)
        {
            if (userAccount.OrganisationID != null)
            {
                userAccount.user.OrganisationId = OrganisationContext.Find(userAccount.OrganisationID).Id;
            }

            if (!ModelState.IsValid)
            {
                return View(userAccount);
            }
            else
            {
                UserAccountContext.Insert(userAccount.user);
                UserAccountContext.Commit();

                return Content("Success");
            }
        }

        public ActionResult AjaxAddOrganisation()
        {
            Organisation organisation = new Organisation();
            return View();
        }

        [HttpPost]
        public ActionResult AjaxAddOrganisation(Organisation organisation)
        {
            if (!ModelState.IsValid)
            {
                return View(organisation);
            }
            else
            {
                OrganisationContext.Insert(organisation);
                OrganisationContext.Commit();

                return Content("Success");
            }
        }

    }
}