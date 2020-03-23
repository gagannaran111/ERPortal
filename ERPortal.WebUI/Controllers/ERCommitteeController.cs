using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.Core.ViewModels;
using System.Data.Entity;
namespace ERPortal.WebUI.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("nodal")]
    public class ERCommitteeController : Controller
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
        public ERCommitteeController(IRepository<ERApplication> _ERApplicationContext, IRepository<FieldType> _FieldTypeContext, IRepository<UHCProductionMethod> _UHCProductionMethodContext, IRepository<UploadFile> _UploadFileContext, IRepository<ERScreeningDetail> _ERScreeningDetailContext, IRepository<ERScreeningInstitute> _ERScreeningInstituteContext, IRepository<Comment> _CommentContext, IRepository<ERAppActiveUsers> _ERAppActiveUsersContext, IRepository<ForwardApplication> _ForwardApplicationContext, IRepository<AuditTrails> _AuditTrailContext)
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

        // GET: ERCommittee

        public ActionResult Index()
        {
            string[] userdata = Session["UserData"] as string[];
            string userid = userdata[0];
            var er = ERAppActiveUsersContext.Collection().Where(x=>x.UserAccountId==userid).ToList();
            var results = (from F in er
                           join FT in ERApplicationContext.Collection().ToList() on F.ERApplicationId equals FT.Id
                           where F.UserAccountId == userdata[0]
                           select FT);
            ViewBag.ApplicationData = results;
            return View();
        }

        //public ActionResult AppRecDGHToERC(string appid)
        //{
        //    ERCViewModel viewModel = new ERCViewModel();

        //    if (!string.IsNullOrEmpty(appid))
        //    {
        //        ERApplication erapp = ERApplicationContext.Collection().Where(x => x.AppId == appid).FirstOrDefault();
        //        viewModel.ERApplications = erapp;
        //        if (!string.IsNullOrEmpty(erapp.ERScreeningDetailId))
        //        {

        //            ViewBag.ERFiles = UploadFileContext.Collection().Where(y => y.FIleRef == erapp.ERScreeningDetail.ReportDocumentPath).ToList();
        //        }
        //        viewModel.FieldTypes = FieldTypeContext.Collection().ToList();
        //        viewModel.UHCProductionMethods = UHCProductionMethodContext.Collection().ToList();
        //        viewModel.comment = CommentContext.Collection().Where(x => x.ERApplicationId == erapp.Id);

        //        viewModel.ERApplications.DGHFileAttachment = erapp.DGHFileAttachment == null ? Guid.NewGuid().ToString() : erapp.DGHFileAttachment;
        //        viewModel.ERApplications.DGHFileAttachmentForPilot = erapp.DGHFileAttachmentForPilot == null ? Guid.NewGuid().ToString() : erapp.DGHFileAttachmentForPilot;
        //        return View(viewModel);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");

        //    }
        //}
        //[HttpPost]       
        //public ActionResult ERCFormSubmit(ERCViewModel eRCViewModel)
        //{
        //    string[] userdata = Session["UserData"] as string[];
        //    if (eRCViewModel.ERApplications.FinalApprovalStatus != null)
        //    {
        //        ERApplication erapp = ERApplicationContext.Collection().Where(x => x.AppId == eRCViewModel.ERApplications.AppId).FirstOrDefault();
        //        erapp.FinalApprovalStatus = eRCViewModel.ERApplications.FinalApprovalStatus;
        //        ERApplicationContext.Update(erapp);

        //        AuditTrails auditTrails = new AuditTrails()
        //        {
        //            ERApplicationId = erapp.AppId,
        //            // FileRefId = null,
        //            StatusId = "f62fccfd-4fe5-4001-9f6b-1ba05a62a0ae",
        //            //  QueryDetailsId = null,
        //            SenderId = userdata[0],
        //            // ReceiverId = null,
        //            Is_Active = true,
        //        };

        //        AuditTrailContext.Insert(auditTrails);
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            ERApplicationContext.Commit();
        //            AuditTrailContext.Commit();
        //            scope.Complete();
        //        }
        //        return Json("Submit To DGH Successfully", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("ERROR", JsonRequestBehavior.AllowGet);
        //    }
        //}

    }
}
