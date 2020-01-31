using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.Core.ViewModels;
using System.Transactions;
using System.Data.Entity;


namespace ERPortal.WebUI.Controllers
{
    [CustomAuthenticationFilter]
    public class CommentController : Controller
    {
        IRepository<Comment> commentContext;
        IRepository<ERApplication> eRApplicationContext;
        IRepository<UploadFile> UploadFileContext;
        IRepository<UserAccount> UserAccountContext;
        IRepository<ForwardApplication> ForwardApplicationContext;
        IRepository<AuditTrails> AuditTrailsContext;
        IRepository<ERAppActiveUsers> ERAppActiveUsersContext;
        IRepository<StatusMaster> StatusMasterContext;
        public CommentController(IRepository<Comment> _commentContext, IRepository<ERApplication> _eRApplicationContext, IRepository<UploadFile> _UploadFileContext, IRepository<UserAccount> _UserAccountContext, IRepository<ForwardApplication> _ForwardApplicationContext, IRepository<AuditTrails> _AuditTrailsContext, IRepository<ERAppActiveUsers> _ERAppActiveUsersContext, IRepository<StatusMaster> _StatusMasterContext)
        {

            eRApplicationContext = _eRApplicationContext;
            commentContext = _commentContext;
            UploadFileContext = _UploadFileContext;
            UserAccountContext = _UserAccountContext;
            ForwardApplicationContext = _ForwardApplicationContext;
            AuditTrailsContext = _AuditTrailsContext;
            ERAppActiveUsersContext = _ERAppActiveUsersContext;
            StatusMasterContext = _StatusMasterContext;
        }

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Comment(string appid)
        {
            ForwardAppViewModel forwardAppViewModel = new ForwardAppViewModel();
            ForwardApplication forwardApplication = new ForwardApplication();
            forwardAppViewModel.ReciverId = UserAccountContext.Collection().Where(x => x.UserRole == "Hod" || x.UserRole == "nodal").ToList();
            forwardApplication.FileRef = Guid.NewGuid().ToString();
            forwardAppViewModel.ForwardApplication = forwardApplication;
            ViewBag.appid = appid;
            return View(forwardAppViewModel);

        }
        [HttpPost]
        public JsonResult CommentSubmit(ForwardAppViewModel forwardAppViewModel, string appid)
        {
            ForwardApplication forwardApplication;
            ERAppActiveUsers eRAppActiveUsers;
            string[] arr = Session["UserData"] as string[];
            bool modelIsValid = false;
            Comment com = new Comment()
            {
                ERApplicationId = appid,
                UserAccountId = arr[0],
                Text = forwardAppViewModel.Comment.Text

            };

            var userlist = forwardAppViewModel.ReciverIdSelectList.Except(ERAppActiveUsersContext.Collection().Where(x => x.ERApplicationId == appid).Select(y => y.UserAccountId).ToList());
            foreach (string user in userlist)
            {
                eRAppActiveUsers = new ERAppActiveUsers()
                {
                    ERApplicationId = appid,
                    UserAccountId = user,
                    Dept_Id = null,
                    Is_Active = true,
                    Status = null
                };
                ERAppActiveUsersContext.Insert(eRAppActiveUsers);
            }

            string st = "";
            switch (forwardAppViewModel.ForwardApplication.FileStatus.ToString())
            {
                case "0":
                    st = "Application Forward";
                    break;
                case "1":
                    st = "Application Approved";
                    break;
                case "2":
                    st = "Application Revert Back";
                    break;
                default: return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
            string auditstatus = StatusMasterContext.Collection().Where(status => status.Status == st).FirstOrDefault().Id;
            foreach (string x in forwardAppViewModel.ReciverIdSelectList)
            {
                forwardApplication = new ForwardApplication()
                {
                    Reciever = x,
                    Sender = arr[0],
                    FileRef = forwardAppViewModel.ForwardApplication.FileRef,
                    CommentRefId = com.Id,
                    Subject = forwardAppViewModel.ForwardApplication.Subject,
                    ERApplicationId = appid,
                    Is_active = true,
                    FileStatus = forwardAppViewModel.ForwardApplication.FileStatus
                };
                ForwardApplicationContext.Insert(forwardApplication);

                AuditTrails auditTrails = new AuditTrails()
                {
                    ERApplicationId = appid,
                    FileRefId = forwardAppViewModel.ForwardApplication.FileRef,
                    StatusId = auditstatus,
                    // QueryDetailsId = null,
                    SenderId = arr[0],
                    ReceiverId = x,
                    Is_Active = true,
                };

                AuditTrailsContext.Insert(auditTrails);
            }

            modelIsValid = TryValidateModel(com);
            if (modelIsValid)
            {
                commentContext.Insert(com);

                using (TransactionScope scope = new TransactionScope())
                {
                    commentContext.Commit();
                    ERAppActiveUsersContext.Commit();
                    ForwardApplicationContext.Commit();
                    AuditTrailsContext.Commit();

                    scope.Complete();
                    return Json("Successfully Forward Application To Selected Users ", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }



        }
        public ActionResult ApplicationSummary(string appid)
        {
            var forapptbl = ForwardApplicationContext.Collection().Where(forApp => forApp.ERApplicationId == appid).ToList();
            var userlist = UserAccountContext.Collection().ToList();
            var ForwardSummaryData = forapptbl.Select(x => new
            {
                recivername = userlist.Where(c => c.Id == x.Reciever)
                .Select(m => m.FirstName + " " + m.LastName + " (" + m.UserRole + ")").FirstOrDefault(),
                Sendername = userlist.Where(c => c.Id == x.Sender)
               .Select(m => m.FirstName + " " + m.LastName + " (" + m.UserRole + ")").FirstOrDefault(),
                x.FileStatus,
                x.Id,
                x.CreatedAt,
                x.ERApplicationId,
                x.Subject,
                x.FileRef,
                comments = commentContext.Collection().Where(c => c.Id == x.CommentRefId)
               .Select(m => m.Text).FirstOrDefault(),
                Files = UploadFileContext.Collection().Where(f => f.FIleRef == x.FileRef).ToList()
            }).GroupBy(x=>x.FileRef);

            return Json(ForwardSummaryData, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult GetComment(string[] commentrefid)
        //{
        //    commentContext.Collection();
        //    return Json(commentContext.Collection().ToList(), JsonRequestBehavior.AllowGet);

        //}

        #region Upload,Delete,Get Files
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
        #endregion
    }
}