using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPortal.WebUI.Models;
using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.Core.ViewModels;
namespace ERPortal.WebUI.Controllers
{
    public class DGHController : Controller
    {
        IRepository<ERApplication> ERApplicationContext;
        IRepository<FieldType> FieldTypeContext;
        IRepository<UHCProductionMethod> UHCProductionMethodContext;
        IRepository<UploadFile> UploadFileContext;
        IRepository<ERScreeningDetail> ERScreeningDetailContext;
        IRepository<ERScreeningInstitute> ERScreeningInstituteContext;
        IRepository<Comment> CommentContext;

        public DGHController(IRepository<ERApplication> _ERApplicationContext, IRepository<FieldType> _FieldTypeContext, IRepository<UHCProductionMethod> _UHCProductionMethodContext, IRepository<UploadFile> _UploadFileContext, IRepository<ERScreeningDetail> _ERScreeningDetailContext, IRepository<ERScreeningInstitute> _ERScreeningInstituteContext, IRepository<Comment> _CommentContext)
        {
            ERApplicationContext = _ERApplicationContext;
            FieldTypeContext = _FieldTypeContext;
            UHCProductionMethodContext = _UHCProductionMethodContext;
            UploadFileContext = _UploadFileContext;
            ERScreeningDetailContext = _ERScreeningDetailContext;
            ERScreeningInstituteContext = _ERScreeningInstituteContext;
            CommentContext = _CommentContext;
        }



        // GET: DGH
        public ActionResult Index()
        {
            ViewBag.ApplicationData = ERApplicationContext.Collection();
            return View();
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
                    
                    ViewBag.ERFiles = UploadFileContext.Collection().Where(y => y.FIleRef == erapp.ERScreeningDetail.ReportDocumentPath).ToList();
                }
                viewModel.FieldTypes = FieldTypeContext.Collection();
                viewModel.UHCProductionMethods = UHCProductionMethodContext.Collection();
                viewModel.comment = CommentContext.Collection().Where(x => x.ERApplicationId == erapp.Id);

                viewModel.ERApplications.DGHFileAttachment = erapp.DGHFileAttachment == null? Guid.NewGuid().ToString(): erapp.DGHFileAttachment;
                viewModel.ERApplications.DGHFileAttachmentForPilot = erapp.DGHFileAttachmentForPilot==null? Guid.NewGuid().ToString(): erapp.DGHFileAttachmentForPilot;
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index");
                //viewModel.ERApplications = new ERApplication();
            }
           
            //viewModel.UploadFiles = UploadFileContext.Collection();

           

        }

        [HttpPost]
        public JsonResult DGHFormSubmit(DGHERProposalViewModel dGHERProposalViewModel)
        {
           // ERApplication erapp = new ERApplication();
            ERApplication erapp = ERApplicationContext.Collection().Where(x=>x.AppId == dGHERProposalViewModel.ERApplications.AppId).FirstOrDefault();

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
            ERApplicationContext.Update(erapp);
            ERApplicationContext.Commit();

            return Json(erapp, JsonRequestBehavior.AllowGet);
        }
    }
}