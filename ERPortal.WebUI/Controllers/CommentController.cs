using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.Core.ViewModels;


namespace ERPortal.WebUI.Controllers
{
    public class CommentController : Controller
    {
        IRepository<Comment> commentContext;
        IRepository<ERApplication> eRApplicationContext;
        IRepository<UploadFile> UploadFileContext;
        public CommentController(IRepository<Comment> _commentContext, IRepository<ERApplication> _eRApplicationContext, IRepository<UploadFile> _UploadFileContext)
        {

            eRApplicationContext = _eRApplicationContext;
            commentContext = _commentContext;
            UploadFileContext = _UploadFileContext;
        }

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Comment(string appid)
        {
            Comment obj = new Comment();
            obj.ERApplicationId = appid;

            return View(obj);
        }
        [HttpPost]
        public JsonResult CommentSubmit(Comment comment, string appid)
        {
            string userid = Session["userId"] == null ? null : Session["userId"].ToString();
            if (userid == null)
            {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
            else
            {
                Comment com = new Comment()
                {
                    ERApplicationId = appid,
                    UserAccountId = userid,// "62c5216e-0155-4fe3-8e9d-06dd66f1ad21",
                    Text = comment.Text,
                };

                // comment.UserAccountId=  
                if (com.UserAccountId != null && com.Text != null && com.ERApplicationId != null)
                {
                    commentContext.Insert(com);
                    commentContext.Commit();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("ERROR", JsonRequestBehavior.AllowGet);
                }
            }
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
    }
}