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
        IRepository<QueryMaster> QueryMasterContext;
        IRepository<QueryDetails> QueryDetailsContext;
        IRepository<QueryUser> QueryUserContext;
        public CommentController(IRepository<Comment> _commentContext, IRepository<ERApplication> _eRApplicationContext, IRepository<UploadFile> _UploadFileContext, IRepository<UserAccount> _UserAccountContext, IRepository<ForwardApplication> _ForwardApplicationContext, IRepository<AuditTrails> _AuditTrailsContext, IRepository<ERAppActiveUsers> _ERAppActiveUsersContext, IRepository<StatusMaster> _StatusMasterContext, IRepository<QueryMaster> _QueryMasterContext, IRepository<QueryDetails> _QueryDetailsContext, IRepository<QueryUser> _QueryUserContext)
        {

            eRApplicationContext = _eRApplicationContext;
            commentContext = _commentContext;
            UploadFileContext = _UploadFileContext;
            UserAccountContext = _UserAccountContext;
            ForwardApplicationContext = _ForwardApplicationContext;
            AuditTrailsContext = _AuditTrailsContext;
            ERAppActiveUsersContext = _ERAppActiveUsersContext;
            StatusMasterContext = _StatusMasterContext;
            QueryMasterContext = _QueryMasterContext;
            QueryDetailsContext = _QueryDetailsContext;
            QueryUserContext = _QueryUserContext;
        }

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForwardApplication(string appid)
        {
            string[] arr = Session["UserData"] as string[];
            string userid = arr[0];
            ForwardAppViewModel forwardAppViewModel = new ForwardAppViewModel();
            ForwardApplication forwardApplication = new ForwardApplication();
            //if(arr[2]=="HoD")

            forwardAppViewModel.ReciverId = UserAccountContext.Collection().Where(x => (x.UserRole == "Hod" || x.UserRole == "nodal" || x.UserRole == "ADG") && x.Id != userid).ToList();
            forwardApplication.FileRef = Guid.NewGuid().ToString();
            forwardAppViewModel.ForwardApplication = forwardApplication;
            ViewBag.appid = appid;
            return View(forwardAppViewModel);

        }
        [HttpPost]
        public JsonResult ForwardApplicationSubmit(ForwardAppViewModel forwardAppViewModel, string appid)
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

            string[] reciverlist = forwardAppViewModel.ReciverIdSelectList != null ? forwardAppViewModel.ReciverIdSelectList : null;
            if (reciverlist != null)
            {
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
            }

            string st = "";
            string userid = arr[0];
            string senderid = "";
            switch (forwardAppViewModel.ForwardApplication.FileStatus.ToString())
            {
                case "Forward":
                    st = "Application Forward";
                    break;

                case "Recommended":
                    st = "Application Recommended";
                    senderid = ForwardApplicationContext.Collection().Where(x => x.Reciever == userid && x.ERApplicationId == appid && x.FileStatus == 0).FirstOrDefault().Sender;
                    reciverlist = new string[] { senderid };
                    break;
                default: return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
            string auditstatus = StatusMasterContext.Collection().Where(status => status.Status == st).FirstOrDefault().Id;
            foreach (string x in reciverlist)
            {
                forwardApplication = new ForwardApplication()
                {
                    Reciever = x,
                    Sender = arr[0],
                    FileRef = forwardAppViewModel.ForwardApplication.FileRef,
                    CommentRefId = com.Id,
                    // Subject = forwardAppViewModel.ForwardApplication.Subject,
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
                    return Json("Successfully Forward Application To Selected Users", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult QueryComment(string appid)
        {
            string[] arr = Session["UserData"] as string[];
            string userid = arr[0];
            QueryCommentViewModel queryCommentViewModel = new QueryCommentViewModel();
            QueryDetails queryDetails = new QueryDetails()
            {
                FileRefId = Guid.NewGuid().ToString(),
            };

            if (arr[2] == "coordinator")
            {
                ViewBag.ReciverList = ERAppActiveUsersContext.Collection().Where(x => x.ERApplicationId == appid && x.UserAccountId != userid)
                     .Select(d => new { ListItemKey = d.UserAccountId, ListItemValue = d.UserAccount.FirstName + " " + d.UserAccount.LastName + " (" + d.UserAccount.UserRole + ")" }).ToList();
                //  queryCommentViewModel.ReciverId = (IEnumerable<ListItemData>)dt;
            }
            else if (arr[2] == "operator")
            {
                ViewBag.ReciverList = ERAppActiveUsersContext.Collection().Where(x => x.ERApplicationId == appid && x.UserAccountId != userid && x.UserAccount.UserRole == "coordinator")
                     .Select(d => new { ListItemKey = d.UserAccountId, ListItemValue = d.UserAccount.FirstName + " " + d.UserAccount.LastName + " (" + d.UserAccount.UserRole + ")" }).ToList();
                // queryCommentViewModel.ReciverId = (IEnumerable<ListItemData>)dt;
            }
            else
            {
                ViewBag.ReciverList = ERAppActiveUsersContext.Collection().Where(x => x.ERApplicationId == appid && x.UserAccountId != userid && x.UserAccount.UserRole != "operator")
                    .Select(d => new { ListItemKey = d.UserAccount.Id, ListItemValue = d.UserAccount.FirstName + " " + d.UserAccount.LastName + " (" + d.UserAccount.UserRole + ")" }).ToList();
                //queryCommentViewModel.ReciverId = (IEnumerable<ListItemData>)dt;
            }

            ViewBag.appid = appid;
            queryCommentViewModel.QueryDetails = queryDetails;
            return View(queryCommentViewModel);

        }
        [HttpPost]
        public JsonResult QueryCommentSubmit(string appid, QueryCommentViewModel queryCommentViewModel)
        {
            string[] arr = Session["UserData"] as string[];

            queryCommentViewModel.Comment.ERApplicationId = appid;
            queryCommentViewModel.Comment.UserAccountId = arr[0];

            queryCommentViewModel.QueryMaster.ERApplicationId = appid;
            queryCommentViewModel.QueryMaster.Is_Active = true;
            queryCommentViewModel.QueryMaster.QueryParentId = null;

            queryCommentViewModel.QueryDetails.CommentRefId = queryCommentViewModel.Comment.Id;
            queryCommentViewModel.QueryDetails.QueryParentId = queryCommentViewModel.QueryMaster.Id;
            queryCommentViewModel.QueryDetails.Status = StatusMasterContext.Collection().Where(x => x.Status == "Query Rasied").FirstOrDefault().Id;
            queryCommentViewModel.QueryDetails.Is_Active = true;

            QueryUser queryUser = new QueryUser()
            {
                SenderId = arr[0],
                RecieverId = queryCommentViewModel.ReciverIdSelectList[0],
                QueryId = queryCommentViewModel.QueryDetails.Id,
            };
            AuditTrails auditTrails = new AuditTrails()
            {
                ERApplicationId = appid,
                FileRefId = queryCommentViewModel.QueryDetails.FileRefId,
                StatusId = queryCommentViewModel.QueryDetails.Status,
                QueryDetailsId = queryCommentViewModel.QueryDetails.Id,
                SenderId = arr[0],
                ReceiverId = queryCommentViewModel.ReciverIdSelectList[0],
                Is_Active = true,
            };

            AuditTrailsContext.Insert(auditTrails);
            QueryDetailsContext.Insert(queryCommentViewModel.QueryDetails);
            QueryMasterContext.Insert(queryCommentViewModel.QueryMaster);
            QueryUserContext.Insert(queryUser);
            commentContext.Insert(queryCommentViewModel.Comment);

            if (TryValidateModel(queryCommentViewModel)) {
                using (TransactionScope scope = new TransactionScope())
                {
                    commentContext.Commit();
                    AuditTrailsContext.Commit();
                    QueryDetailsContext.Commit();
                    QueryMasterContext.Commit();
                    QueryUserContext.Commit();
                    scope.Complete();
                    return Json("Successfully Query Rasied To Selected Users", JsonRequestBehavior.AllowGet);
                } }
            else
            {
              return Json("Something Went Wrong! Try Again Later");
            }

        }
        public JsonResult GrantApplication()
        {
            return Json("", JsonRequestBehavior.AllowGet);

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
                //x.Subject,
                x.FileRef,
                comments = commentContext.Collection().Where(c => c.Id == x.CommentRefId)
               .Select(m => m.Text).FirstOrDefault(),
                Files = UploadFileContext.Collection().Where(f => f.FIleRef == x.FileRef && x.Is_active == true).ToList()
            }).OrderByDescending(x => x.CreatedAt).ToList();

            return Json(ForwardSummaryData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BtnCheckStatus(string appid, string btnType)
        {
            string[] arr = Session["UserData"] as string[];
            string userid = arr[0];
            string statuscheck = "Hide";
            int recievecheck = 0;
            int approvedcount = 0;
            switch (btnType)
            {
                case "btnForward":
                    recievecheck = ERAppActiveUsersContext.Collection()
                        .Where(x => x.ERApplicationId == appid && x.UserAccountId == userid && x.Is_Active == true).Count();
                    if (recievecheck > 0)
                    {
                        statuscheck = "Show";
                        approvedcount = ForwardApplicationContext.Collection()
                            .Where(x => x.Sender == userid && (x.FileStatus == FileStatus.Recommended) && x.ERApplicationId == appid && x.Is_active == true).Count();
                        statuscheck = approvedcount > 0 ? "Hide" : "Show";
                    }
                    else
                    {
                        statuscheck = "Hide";
                    }

                    break;
                default:
                    return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
            return Json(statuscheck, JsonRequestBehavior.AllowGet);
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