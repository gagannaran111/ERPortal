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
using System.Data.Entity;

namespace ERPortal.WebUI.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("operator")]
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
        IRepository<StatusMaster> StatusMasterContext;
        IRepository<UserAccount> UserAccountContext;
        public OperatorController(IRepository<ERApplication> _ERApplicationContext, IRepository<FieldType> _FieldTypeContext, IRepository<UHCProductionMethod> _UHCProductionMethodContext, IRepository<UploadFile> _UploadFileContext, IRepository<ERScreeningDetail> _ERScreeningDetailContext, IRepository<ERScreeningInstitute> _ERScreeningInstituteContext, IRepository<Organisation> _OrganisationContext, IRepository<ForwardApplication> _ForwardApplicationContext, IRepository<AuditTrails> _AuditTrailContext, IRepository<ERAppActiveUsers> _ERAppActiveUsersContext, IRepository<StatusMaster> _StatusMasterContext, IRepository<UserAccount> _UserAccountContext)
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
            StatusMasterContext = _StatusMasterContext;
            UserAccountContext = _UserAccountContext;

        }

        // GET: Operator


        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult OperatorDashboard()
        {
            string[] userdata = Session["UserData"] as string[];
            string userid = userdata[0];
            List<ERApplication> erapp = ERApplicationContext.Collection().ToList();
            List<ERAppActiveUsers> activeuser = ERAppActiveUsersContext.Collection().Where(x => x.UserAccountId == userid && x.Is_Active == true).Distinct().ToList();
         
            var applicationdata = erapp.Join(activeuser, x => x.Id, y => y.ERApplicationId, (x, y) => new
            {
                x.Id,
                x.AppId,
                x.Organisation,
                x.FieldType,
                x.FieldName,
                x.CreatedAt,
                y.Dept_Id,
                y.UserAccount,
                Status = AuditTrailContext.Collection().Where(w => w.ERApplicationId == x.Id && w.Is_Active == true).Count() == 1 ? "NA"
                 : AuditTrailContext.Collection().Where(w => w.ERApplicationId == x.Id && w.Is_Active == true && w.StatusId == "S116").Count() > 0 ? "AP"
                 : AuditTrailContext.Collection().Where(d => d.ERApplicationId == x.Id && d.Is_Active == true && (d.SenderId == userid || d.ReceiverId == userid))
                 .OrderByDescending(o => o.CreatedAt).Select(s => new { StatusId = s.ReceiverId == userid ? "PWM" : s.SenderId == userid ? "UP" : "" }).FirstOrDefault().StatusId,
                FileRef = AuditTrailContext.Collection().Where(w => w.ERApplicationId == x.Id && w.Is_Active == true && w.StatusId == "S116").Count() == 0 ? null
                : AuditTrailContext.Collection().Where(w => w.ERApplicationId == x.Id && w.Is_Active == true && w.StatusId == "S116").FirstOrDefault().FileRefId
            }).Select(a => new
            {
                a.Id,
                a.AppId,
                a.Organisation,
                a.FieldType,
                a.FieldName,
                a.CreatedAt,
                a.Dept_Id,
                a.UserAccount,
                a.Status,
                ApprovalLetter = a.FileRef != null ? UploadFileContext.Collection().Where(f => f.FIleRef == a.FileRef).ToList() : null
            }).ToList();

            return Json(applicationdata);
        }

        public ActionResult SubmitERProposal(string appid)
        {
            string[] userdata = Session["UserData"] as string[];
            string userid = userdata[0];
            ViewBag.Title = "Submit Proposal";
            ViewBag.RefId = Guid.NewGuid().ToString();
            OperatorERProposalViewModel viewModel = new OperatorERProposalViewModel();

            if (!string.IsNullOrEmpty(appid))
            {
                ERApplication erapp = ERApplicationContext.Collection().Where(x => x.AppId == appid).FirstOrDefault();
                viewModel.ERApplications = erapp;
            }
            else
            {
                viewModel.ERApplications = new ERApplication();
                viewModel.ERApplications.Organisation = UserAccountContext.Collection().Where(x => x.Id == userid).FirstOrDefault().Organisation;
                viewModel.ERApplications.OrganisationId = viewModel.ERApplications.Organisation.Id;
            }
            viewModel.FieldTypes = FieldTypeContext.Collection().ToList();
            viewModel.UHCProductionMethods = UHCProductionMethodContext.Collection().ToList();
          //  viewModel.organisationTypes = OrganisationContext.Collection().ToList();
            viewModel.eRScreeningInstitutes = ERScreeningInstituteContext.Collection().ToList();

            //viewModel.UploadFiles = UploadFileContext.Collection();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SubmitERProposal(OperatorERProposalViewModel _ERApplication, string FileRef)
        {
            ViewBag.Title = "Submit Proposal";
            string[] userdata = Session["UserData"] as string[];

            if (!ModelState.IsValid)
            {
                ViewBag.RefId = Guid.NewGuid().ToString();
                return View(_ERApplication);
            }
            //  _ERApplication.ERApplications.FieldName
            else
            {
                string dt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("/", "").Replace(":", "").Replace(" ", "");
                _ERApplication.ERApplications.AppId = "ERAPPID" + dt;
                _ERApplication.ERApplications.ERScreeningDetail.ReportDocumentPath = FileRef;

                ERApplicationContext.Insert(_ERApplication.ERApplications);
                string auditstatus = StatusMasterContext.Collection().Where(status => status.Status == "Application Submitted").FirstOrDefault().CustStatusId;

                string CER = UserAccountContext.Collection().Where(x => x.UserRole == "Consultant Enhanced Recovery").Select(c => c.Id).FirstOrDefault();

                AuditTrails auditTrails = new AuditTrails()
                {
                    ERApplicationId = _ERApplication.ERApplications.Id,
                    FileRefId = FileRef,
                    StatusId = auditstatus,
                    // QueryDetailsId = null,
                    SenderId = userdata[0],
                    //ReceiverId = CER, // Consultant Enhanced Recovery
                    Is_Active = true,
                };
                List<string> lst = new List<string>() {
                    userdata[0],
                    CER // Consultant Enhanced Recovery
                };
                foreach (string x in lst)
                {
                    ERAppActiveUsers eRAppActiveUsers = new ERAppActiveUsers()
                    {
                        ERApplicationId = _ERApplication.ERApplications.Id,
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

            return Json(_ERApplication.ERApplications.AppId, JsonRequestBehavior.AllowGet);
        }
        #region // Not Used
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
        #endregion

    }
}