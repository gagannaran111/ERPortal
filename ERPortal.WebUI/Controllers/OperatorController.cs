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
namespace ERPortal.WebUI.Controllers
{
    public class OperatorController : Controller
    {
        IRepository<ERApplication> ERApplicationContext;
        IRepository<FieldType> FieldTypeContext;
        IRepository<UHCProductionMethod> UHCProductionMethodContext;
        IRepository<UploadFile> UploadFileContext;
        IRepository<ERScreeningDetail> ERScreeningDetailContext;
        IRepository<ERScreeningInstitute> ERScreeningInstituteContext;
        IRepository<Organisation> OrganisationContext;
        public OperatorController(IRepository<ERApplication> _ERApplicationContext, IRepository<FieldType> _FieldTypeContext, IRepository<UHCProductionMethod> _UHCProductionMethodContext, IRepository<UploadFile> _UploadFileContext, IRepository<ERScreeningDetail> _ERScreeningDetailContext, IRepository<ERScreeningInstitute> _ERScreeningInstituteContext, IRepository<Organisation> _OrganisationContext)
        {
            ERApplicationContext = _ERApplicationContext;
            FieldTypeContext = _FieldTypeContext;
            UHCProductionMethodContext = _UHCProductionMethodContext;
            UploadFileContext = _UploadFileContext;
            ERScreeningDetailContext = _ERScreeningDetailContext;
            ERScreeningInstituteContext = _ERScreeningInstituteContext;
            OrganisationContext = _OrganisationContext;
        }

        // GET: Operator
        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

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

            if (!ModelState.IsValid)
            {
                return View(_ERApplication);
            }

            else
            {
               // var appid = ERApplicationContext.Collection().OrderByDescending(x => x.AppId).FirstOrDefault();
               
                    string dt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("/","").Replace(":","").Replace(" ","");               
                    _ERApplication.ERApplications.AppId = "ERAPPID" + dt;              
               

                ERApplicationContext.Insert(_ERApplication.ERApplications);
                ERApplicationContext.Commit();
            }

            return Json("Application Ref No : "+_ERApplication.ERApplications.AppId,JsonRequestBehavior.AllowGet);
          //  return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult LoadUploadFile(HttpPostedFileBase file, string RefId)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Content/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                    //string FileRefId = RefId != null && RefId != "" ? RefId : Guid.NewGuid().ToString();
                    UploadFile uploadFile = new UploadFile() { FileName = _FileName, FilePath = _path, FIleRef = RefId };

                    UploadFileContext.Insert(uploadFile);
                    UploadFileContext.Commit();
                    return Json("File upload Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("File upload failed!!", JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }
        [HttpPost]
        public JsonResult GetUploadFiles(string RefId)
        {
            return Json(UploadFileContext.Collection().Where(x => x.FIleRef == RefId).ToList<UploadFile>());
        }
        [HttpPost]
        public JsonResult RemoveUploadFile(string FileId)
        {
            UploadFileContext.Delete(FileId);
            UploadFileContext.Commit();
            return Json("File Removed Success", JsonRequestBehavior.AllowGet);
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
               
                return View(targetPage, viewModel);
            }

        }
        public ActionResult AjaxViewDetails(string targetPage,string RefId)
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

    }
}