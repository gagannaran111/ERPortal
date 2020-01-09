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

        public DGHController(IRepository<ERApplication> _ERApplicationContext, IRepository<FieldType> _FieldTypeContext, IRepository<UHCProductionMethod> _UHCProductionMethodContext, IRepository<UploadFile> _UploadFileContext, IRepository<ERScreeningDetail> _ERScreeningDetailContext, IRepository<ERScreeningInstitute> _ERScreeningInstituteContext)
        {
            ERApplicationContext = _ERApplicationContext;
            FieldTypeContext = _FieldTypeContext;
            UHCProductionMethodContext = _UHCProductionMethodContext;
            UploadFileContext = _UploadFileContext;
            ERScreeningDetailContext = _ERScreeningDetailContext;
            ERScreeningInstituteContext = _ERScreeningInstituteContext;
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
            }
            else
            {
                //viewModel.ERApplications = new ERApplication();
            }
            viewModel.FieldTypes = FieldTypeContext.Collection();
            viewModel.UHCProductionMethods = UHCProductionMethodContext.Collection();
            //viewModel.UploadFiles = UploadFileContext.Collection();

            return View(viewModel);
           
        }
    }
}