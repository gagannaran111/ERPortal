using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.Core.ViewModels;
namespace ERPortal.WebUI.Controllers
{
    public class ERCommitteeController : Controller
    {
        IRepository<ERApplication> ERApplicationContext;
        IRepository<FieldType> FieldTypeContext;
        IRepository<UHCProductionMethod> UHCProductionMethodContext;
        IRepository<UploadFile> UploadFileContext;
        IRepository<ERScreeningDetail> ERScreeningDetailContext;
        IRepository<ERScreeningInstitute> ERScreeningInstituteContext;
        IRepository<Comment> CommentContext;
        public ERCommitteeController(IRepository<ERApplication> _ERApplicationContext, IRepository<FieldType> _FieldTypeContext, IRepository<UHCProductionMethod> _UHCProductionMethodContext, IRepository<UploadFile> _UploadFileContext, IRepository<ERScreeningDetail> _ERScreeningDetailContext, IRepository<ERScreeningInstitute> _ERScreeningInstituteContext, IRepository<Comment> _CommentContext)
        {
            ERApplicationContext = _ERApplicationContext;
            FieldTypeContext = _FieldTypeContext;
            UHCProductionMethodContext = _UHCProductionMethodContext;
            UploadFileContext = _UploadFileContext;
            ERScreeningDetailContext = _ERScreeningDetailContext;
            ERScreeningInstituteContext = _ERScreeningInstituteContext;
            CommentContext = _CommentContext;
        }

        // GET: ERCommittee
        public ActionResult Index()
        {
            ViewBag.ApplicationData = ERApplicationContext.Collection().Where(x => x.DGHApprovalStatus != null).ToList(); ;            
            return View();
        }      
        public ActionResult AppRecDGHToERC(string appid)
        {
            //ViewBag.Title = "Submit Proposal";

            ERCViewModel viewModel = new ERCViewModel();

            if (!string.IsNullOrEmpty(appid))
            {
                ERApplication erapp = ERApplicationContext.Collection().Where(x => x.AppId == appid).FirstOrDefault();
                viewModel.ERApplications = erapp;
                if (!string.IsNullOrEmpty(erapp.ERScreeningDetailId))
                {

                    ViewBag.ERFiles = UploadFileContext.Collection().Where(y => y.FIleRef == erapp.ERScreeningDetail.ReportDocumentPath).ToList();
                }
                viewModel.FieldTypes = FieldTypeContext.Collection().ToList();
                viewModel.UHCProductionMethods = UHCProductionMethodContext.Collection().ToList();
                viewModel.comment = CommentContext.Collection().Where(x => x.ERApplicationId == erapp.Id);

                viewModel.ERApplications.DGHFileAttachment = erapp.DGHFileAttachment == null ? Guid.NewGuid().ToString() : erapp.DGHFileAttachment;
                viewModel.ERApplications.DGHFileAttachmentForPilot = erapp.DGHFileAttachmentForPilot == null ? Guid.NewGuid().ToString() : erapp.DGHFileAttachmentForPilot;
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
        public ActionResult ERCFormSubmit(ERCViewModel eRCViewModel)
        {
            if (eRCViewModel.ERApplications.FinalApprovalStatus != null)
            {
                ERApplication erapp = ERApplicationContext.Collection().Where(x => x.AppId == eRCViewModel.ERApplications.AppId).FirstOrDefault();
                erapp.FinalApprovalStatus = eRCViewModel.ERApplications.FinalApprovalStatus;
                ERApplicationContext.Update(erapp);
                ERApplicationContext.Commit();
                return Json("Submit To DGH Successfully", JsonRequestBehavior.AllowGet);
            }
            else {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}
