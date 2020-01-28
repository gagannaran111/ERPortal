using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPortal.WebUI.Models;
using System.Transactions;

namespace ERPortal.WebUI.Controllers
{


    [CustomAuthenticationFilter]
    public class OperatorController : Controller
    {
        IRepository<ERApplication> ERApplicationContext;
        IRepository<FieldType> FieldTypeContext;
        IRepository<UHCProductionMethod> UHCProductionMethodContext;
        IRepository<UploadFile> UploadFileContext;
        IRepository<ERScreeningDetail> ERScreeningDetailContext;
        IRepository<ERScreeningInstitute> ERScreeningInstituteContext;
        IRepository<Organisation> OrganisationContext;
        IRepository<ForwardApplication> ForwardApplicationContext;
        IRepository<AuditTrails> AuditTrailContext;
        IRepository<ERAppActiveUsers> ERAppActiveUsersContext;

        public OperatorController(IRepository<ERApplication> _ERApplicationContext, IRepository<FieldType> _FieldTypeContext, IRepository<UHCProductionMethod> _UHCProductionMethodContext, IRepository<UploadFile> _UploadFileContext, IRepository<ERScreeningDetail> _ERScreeningDetailContext, IRepository<ERScreeningInstitute> _ERScreeningInstituteContext, IRepository<Organisation> _OrganisationContext, IRepository<ForwardApplication> _ForwardApplicationContext, IRepository<AuditTrails> _AuditTrailContext, IRepository<ERAppActiveUsers> _ERAppActiveUsersContext)
        {
            ERApplicationContext = _ERApplicationContext;
            FieldTypeContext = _FieldTypeContext;
            UHCProductionMethodContext = _UHCProductionMethodContext;
            UploadFileContext = _UploadFileContext;
            ERScreeningDetailContext = _ERScreeningDetailContext;
            ERScreeningInstituteContext = _ERScreeningInstituteContext;
            OrganisationContext = _OrganisationContext;
            ForwardApplicationContext = _ForwardApplicationContext;
            AuditTrailContext = _AuditTrailContext;
            ERAppActiveUsersContext = _ERAppActiveUsersContext;
        }

        // GET: Operator

        [CustomAuthorize("operator")]
        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        [CustomAuthorize("operator")]
        public ActionResult SubmitERProposal(string appid)
        {
            ViewBag.Title = "Submit Proposal";
            OperatorERProposalViewModel viewModel = new OperatorERProposalViewModel();

            if (!string.IsNullOrEmpty(appid))
            {
                ERApplication erapp = ERApplicationContext.Collection().Where(x => x.AppId == appid).FirstOrDefault();
                viewModel.ERApplications = erapp;
            }
            else
            {
                viewModel.ERApplications = new ERApplication();
            }
            viewModel.FieldTypes = FieldTypeContext.Collection().ToList();
            viewModel.UHCProductionMethods = UHCProductionMethodContext.Collection().ToList();
            viewModel.organisationTypes = OrganisationContext.Collection().ToList();
            
            //viewModel.UploadFiles = UploadFileContext.Collection();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SubmitERProposal(OperatorERProposalViewModel _ERApplication)
        {
            ViewBag.Title = "Submit Proposal";
            string[] userdata = Session["UserData"] as string[];

            if (!ModelState.IsValid)
            {
                return View(_ERApplication);
            }

            else
            {
                string dt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("/", "").Replace(":", "").Replace(" ", "");
                _ERApplication.ERApplications.AppId = "ERAPPID" + dt;

                ERApplicationContext.Insert(_ERApplication.ERApplications);
                AuditTrails auditTrails = new AuditTrails()
                {
                    ERApplicationId = _ERApplication.ERApplications.AppId,
                   // FileRefId = null,
                    StatusId = "14e8e42d-aa81-4e91-876a-95651048f6d6",
                   // QueryDetailsId = null,
                    SenderId = userdata[0],
                    ReceiverId = "aaf04b39-83b4-4870-84bb-20d2acac2e87",
                    Is_Active = true,
                };
                List<string> lst = new List<string>() {
                    userdata[0],
                    "aaf04b39-83b4-4870-84bb-20d2acac2e87"
                };
                foreach (string x in lst)
                {
                    ERAppActiveUsers eRAppActiveUsers = new ERAppActiveUsers()
                    {
                        ERApplicationId = _ERApplication.ERApplications.AppId,
                        UserAccountId = x, 
                        Dept_Id = null,
                        Is_Active = true,
                        Status = null
                    };

                    ERAppActiveUsersContext.Insert(eRAppActiveUsers);
                }
                AuditTrailContext.Insert(auditTrails);

                using (TransactionScope scope = new TransactionScope())
                {
                    ERApplicationContext.Commit();
                    ERAppActiveUsersContext.Commit();
                    AuditTrailContext.Commit();
                    scope.Complete();
                }

            }

            return Json("Application Ref No : " + _ERApplication.ERApplications.AppId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxAdd(string targetPage)
        {
            object _genericObject;
            switch (targetPage)
            {
                case "ERScreeningDetail":
                    ERScreeningDetailViewModel viewModel = new ERScreeningDetailViewModel();
                    viewModel.eRScreeningDetail = new ERScreeningDetail();
                    viewModel.eRScreeningInstitutes = ERScreeningInstituteContext.Collection().ToList();
                    ViewBag.RefId = Guid.NewGuid().ToString();
                    _genericObject = viewModel;
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
        public ActionResult AjaxAdd(string targetPage, string FileRef, FormCollection collection)
        {
            object _genericObject = null;
            string id = "";
            bool modelIsValid = false;
            switch (targetPage)
            {
                case "ERScreeningDetail":
                    ERScreeningDetail eRScreeningDetail = new ERScreeningDetail()
                    {
                        ERScreeningInstituteId = collection["eRScreeningDetail.ERScreeningInstituteId"],
                        ReportDocumentPath = FileRef,
                        FirstOrderScreening = Convert.ToBoolean(collection["eRScreeningDetail.FirstOrderScreening"]),
                        SecondOrderScreening = Convert.ToBoolean(collection["eRScreeningDetail.SecondOrderScreening"]),
                        ThirdOrderScreening = Convert.ToBoolean(collection["eRScreeningDetail.ThirdOrderScreening"]),
                    };
                    id = eRScreeningDetail.Id;

                    modelIsValid = TryValidateModel(eRScreeningDetail);
                    if (modelIsValid && ModelState.IsValid)
                    {
                        ERScreeningDetailContext.Insert(eRScreeningDetail);
                        ERScreeningDetailContext.Commit();
                    }
                    else
                    {
                        _genericObject = eRScreeningDetail;
                    }
                    break;
                default:
                    return Content("Error");
            }
            if (modelIsValid && ModelState.IsValid)
            {
                return Content("Success," + id + "," + targetPage);
            }
            else
            {
                ERScreeningDetailViewModel viewModel = new ERScreeningDetailViewModel();
                viewModel.eRScreeningInstitutes = ERScreeningInstituteContext.Collection().ToList();
                viewModel.eRScreeningDetail = (ERScreeningDetail)_genericObject;
                ViewBag.RefId = Guid.NewGuid().ToString();
                return View(targetPage, viewModel);
            }

        }
        public ActionResult AjaxViewDetails(string targetPage, string RefId)
        {
            object _genericObject;
            switch (targetPage)
            {
                case "ERScreeningDetailView":
                    ERScreeningDetailViewModel viewModel = new ERScreeningDetailViewModel();
                    viewModel.eRScreeningDetail = ERScreeningDetailContext.Find(RefId);
                    _genericObject = viewModel;
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
       
        //public ActionResult SubmitERScreeningReport()
        //{
        //    ERScreeningDetailViewModel viewModel = new ERScreeningDetailViewModel();
        //    viewModel.eRScreeningDetail = new ERScreeningDetail();
        //    viewModel.eRScreeningInstitutes = ERScreeningInstituteContext.Collection().ToList();
        //    ViewBag.RefId = Guid.NewGuid().ToString();
        //    return View(viewModel);
        
        //}

    }
}