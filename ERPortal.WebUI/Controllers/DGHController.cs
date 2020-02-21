using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPortal.WebUI.Models;
using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.Core.ViewModels;
using System.Transactions;

namespace ERPortal.WebUI.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("HoD", "ADG", "coordinator", "Consultant Enhanced Recovery", "DG")]
    public class DGHController : Controller
    {
        IRepository<ERApplication> ERApplicationContext;
        IRepository<FieldType> FieldTypeContext;
        IRepository<UHCProductionMethod> UHCProductionMethodContext;
        IRepository<UploadFile> UploadFileContext;
        IRepository<ERScreeningDetail> ERScreeningDetailContext;
        IRepository<ERScreeningInstitute> ERScreeningInstituteContext;
        IRepository<Comment> CommentContext;
        IRepository<ForwardApplication> ForwardApplicationContext;
        IRepository<AuditTrails> AuditTrailContext;
        IRepository<ERAppActiveUsers> ERAppActiveUsersContext;

        public DGHController(IRepository<ERApplication> _ERApplicationContext, IRepository<FieldType> _FieldTypeContext, IRepository<UHCProductionMethod> _UHCProductionMethodContext, IRepository<UploadFile> _UploadFileContext, IRepository<ERScreeningDetail> _ERScreeningDetailContext, IRepository<ERScreeningInstitute> _ERScreeningInstituteContext, IRepository<Comment> _CommentContext, IRepository<ERAppActiveUsers> _ERAppActiveUsersContext, IRepository<ForwardApplication> _ForwardApplicationContext, IRepository<AuditTrails> _AuditTrailContext)
        {
            ERApplicationContext = _ERApplicationContext;
            FieldTypeContext = _FieldTypeContext;
            UHCProductionMethodContext = _UHCProductionMethodContext;
            UploadFileContext = _UploadFileContext;
            ERScreeningDetailContext = _ERScreeningDetailContext;
            ERScreeningInstituteContext = _ERScreeningInstituteContext;
            CommentContext = _CommentContext;
            ForwardApplicationContext = _ForwardApplicationContext;
            AuditTrailContext = _AuditTrailContext;
            ERAppActiveUsersContext = _ERAppActiveUsersContext;
        }



        // GET: DGH
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult DGHDashboard()
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
        public ActionResult AppRecToDgh(string appid)
        {
            //ViewBag.Title = "Submit Proposal";

            DGHERProposalViewModel viewModel = new DGHERProposalViewModel();

            if (!string.IsNullOrEmpty(appid))
            {
                ERApplication erapp = ERApplicationContext.Collection().Where(x => x.AppId == appid).FirstOrDefault();
                viewModel.ERApplications = erapp;
                if (!string.IsNullOrEmpty(erapp.ERScreeningDetailId))
                {

                    ViewBag.ERFiles = UploadFileContext.Collection()
                        .Where(y => y.FIleRef == erapp.ERScreeningDetail.ReportDocumentPath).ToList();
                }
                else
                {
                    ViewBag.ERFiles = null;
                }
                viewModel.FieldTypes = FieldTypeContext.Collection();
                viewModel.UHCProductionMethods = UHCProductionMethodContext.Collection();
                viewModel.comment = CommentContext.Collection().Where(x => x.ERApplicationId == erapp.Id);


                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index");
                //viewModel.ERApplications = new ERApplication();
            }

            //viewModel.UploadFiles = UploadFileContext.Collection();



        }


        #region // Currently Not Used
        [HttpPost]
        public JsonResult DGHFormSubmit(DGHERProposalViewModel dGHERProposalViewModel)
        {
            // ERApplication erapp = new ERApplication();
            string[] userdata = Session["UserData"] as string[];
            ERApplication erapp = ERApplicationContext.Collection().Where(x => x.AppId == dGHERProposalViewModel.ERApplications.AppId).FirstOrDefault();

            if (erapp.ERScreeningDetailId != null)
            {
                erapp.ERScreeningDetail.ApprovalStatus = dGHERProposalViewModel.ERApplications.ERScreeningDetail.ApprovalStatus;
                erapp.ERScreeningDetail.DateOfSubmission = dGHERProposalViewModel.ERApplications.ERScreeningDetail.DateOfSubmission;
                erapp.ERScreeningDetail.DateOfLastApproval = dGHERProposalViewModel.ERApplications.ERScreeningDetail.DateOfLastApproval;
            }
            erapp.PilotMandatory = dGHERProposalViewModel.ERApplications.PilotMandatory;
            erapp.PilotReportApprovalStatus = dGHERProposalViewModel.ERApplications.PilotReportApprovalStatus;
            erapp.DGHFileAttachmentForPilot = dGHERProposalViewModel.ERApplications.DGHFileAttachmentForPilot;
            erapp.DGHFileAttachment = dGHERProposalViewModel.ERApplications.DGHFileAttachment;
            erapp.FinalApprovalStatus = dGHERProposalViewModel.ERApplications.FinalApprovalStatus;
            erapp.EligibleForFiscalIncentive = dGHERProposalViewModel.ERApplications.EligibleForFiscalIncentive;
            erapp.DGHApprovalStatus = dGHERProposalViewModel.ERApplications.DGHApprovalStatus;
            erapp.DGHApprovalDate = DateTime.Now; //dGHERProposalViewModel.ERApplications.DGHApprovalDate;
            if (erapp.DGHApprovalStatus != null)
            {
                ERApplicationContext.Update(erapp);

                AuditTrails auditTrails = new AuditTrails()
                {
                    ERApplicationId = erapp.AppId,
                    // FileRefId = null,
                    StatusId = "34fcb2d5-14d2-4fc2-9db8-a71dcbd6ffc8",
                    // QueryDetailsId = null,
                    SenderId = userdata[0],
                    ReceiverId = "c40a19d6-8af7-4813-aa5c-2dc7ca6449e3",
                    Is_Active = true,
                };
                List<string> lst = new List<string>() {
                    "c40a19d6-8af7-4813-aa5c-2dc7ca6449e3"
                };
                foreach (string x in lst)
                {
                    ERAppActiveUsers eRAppActiveUsers = new ERAppActiveUsers()
                    {
                        ERApplicationId = erapp.AppId,
                        UserAccountId = x,
                        // Dept_Id = null,
                        Is_Active = true,
                        // Status = null
                    };

                    ERAppActiveUsersContext.Insert(eRAppActiveUsers);
                }

                AuditTrailContext.Insert(auditTrails);
                using (TransactionScope scope = new TransactionScope())
                {
                    ERApplicationContext.Commit();
                    AuditTrailContext.Commit();
                    ERAppActiveUsersContext.Commit();
                    scope.Complete();
                }
                return Json("Submit To ER Committee Successfully", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Not Submitted. DGH Approval Mandatory", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion 

    }
}