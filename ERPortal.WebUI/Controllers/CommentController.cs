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
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
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

        [HttpPost]
        public async Task<JsonResult> ApplicationActionSubmit(ForwardAppViewModel forwardAppViewModel, string appid)
        {
            ForwardApplication forwardApplication;
            ERAppActiveUsers eRAppActiveUsers;
            string[] arr = Session["UserData"] as string[];
            bool modelIsValid = false;
            string msg = "ERROR";
            forwardAppViewModel.Comment.ERApplicationId = appid;
            forwardAppViewModel.Comment.UserAccountId = arr[0];

            string[] reciverlist = forwardAppViewModel.ReciverIdSelectList != null ? forwardAppViewModel.ReciverIdSelectList : null;

            if (reciverlist != null && forwardAppViewModel.ForwardApplication.FileStatus == FileStatus.Forward)
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

            string userid = arr[0];
            string senderid = "";
            string auditstatus = "";
            switch (forwardAppViewModel.ForwardApplication.FileStatus)
            {
                case FileStatus.Forward:
                    auditstatus = "S102";
                    msg = "Successfully Forward Application To Selected Users";
                    break;

                case FileStatus.Recommended:
                    auditstatus = "S105";
                    senderid = ForwardApplicationContext.Collection().Where(x => x.Reciever == userid && x.ERApplicationId == appid && (x.FileStatus == FileStatus.Forward || x.FileStatus == FileStatus.ReviewAgain) && x.Is_Active == true).FirstOrDefault().Sender;
                    reciverlist = new string[] { senderid };
                    msg = "Successfully Application Recommended";
                    break;
                case FileStatus.CommentBack:
                    auditstatus = "S109";
                    senderid = ForwardApplicationContext.Collection().Where(x => x.Reciever == userid && x.ERApplicationId == appid && (x.FileStatus == FileStatus.Forward || x.FileStatus == FileStatus.ReviewAgain) && x.Is_Active == true).FirstOrDefault().Sender;
                    reciverlist = new string[] { senderid };
                    msg = "Successfully Comment Back";
                    if (arr[2] == UserRoleType.DG.ToString())
                    {
                        await ForwardApplicationContext.Collection().Where(x => x.ERApplicationId == appid && x.Sender == senderid && x.Reciever == userid && (x.FileStatus == FileStatus.Forward || x.FileStatus == FileStatus.ReviewAgain) && x.Is_Active == true).ForEachAsync(x => x.Is_Active = false);
                        await AuditTrailsContext.Collection().Where(x => x.ERApplicationId == appid && x.SenderId == senderid && x.ReceiverId == userid && x.Is_Active == true).ForEachAsync(x => x.Is_Active = false);

                    }
                    else
                    {
                        await ForwardApplicationContext.Collection().Where(x => x.ERApplicationId == appid && x.Sender == senderid && (x.FileStatus == FileStatus.Forward || x.FileStatus == FileStatus.ReviewAgain) && x.Is_Active == true).ForEachAsync(x => x.Is_Active = false);
                        await ForwardApplicationContext.Collection().Where(x => x.ERApplicationId == appid && x.FileStatus == FileStatus.Recommended && x.Reciever == senderid && x.Is_Active == true).ForEachAsync(d => d.Is_Active = false);
                        await AuditTrailsContext.Collection().Where(x => x.ERApplicationId == appid && x.StatusId == "S105" && x.Is_Active == true).ForEachAsync(d => d.Is_Active = false);
                        await AuditTrailsContext.Collection().Where(x => x.ERApplicationId == appid && x.SenderId == senderid && x.Is_Active == true).ForEachAsync(x => x.Is_Active = false);

                    }
                    break;
                case FileStatus.ReviewAgain:
                    string DGID = UserAccountContext.Collection().Where(x => x.UserRole == UserRoleType.DG.ToString()).FirstOrDefault().Id;
                    int countdgid = ERAppActiveUsersContext.Collection().Where(x => x.UserAccountId == DGID && x.ERApplicationId == appid && x.Is_Active == true).Count();
                    if (countdgid > 0)
                    {
                        reciverlist = new string[] { DGID };
                    }
                    else
                    {
                        reciverlist = ForwardApplicationContext.Collection().Where(x => x.ERApplicationId == appid && x.Sender == userid && (x.FileStatus == FileStatus.Forward || x.FileStatus == FileStatus.ReviewAgain) && x.Is_Active == false).Select(d => d.Reciever).Distinct().ToArray();
                        if (reciverlist == null || reciverlist.Length == 0)
                            return Json("NCR", JsonRequestBehavior.AllowGet);

                    }
                    auditstatus = "S106";
                    msg = "Comment Resolved Successfully";
                    break;

                default: return Json(msg, JsonRequestBehavior.AllowGet);
            }

            foreach (string x in reciverlist)
            {
                forwardApplication = new ForwardApplication()
                {
                    Reciever = x,
                    Sender = arr[0],
                    FileRef = forwardAppViewModel.ForwardApplication.FileRef,
                    CommentRefId = forwardAppViewModel.Comment.Id,
                    // Subject = forwardAppViewModel.ForwardApplication.Subject,
                    ERApplicationId = appid,
                    Is_Active = true,
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

            modelIsValid = TryValidateModel(forwardAppViewModel.Comment);
            if (modelIsValid)
            {
                commentContext.Insert(forwardAppViewModel.Comment);

                using (TransactionScope scope = new TransactionScope())
                {
                    commentContext.Commit();
                    ERAppActiveUsersContext.Commit();
                    ForwardApplicationContext.Commit();
                    AuditTrailsContext.Commit();

                    scope.Complete();
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                msg = "ERROR";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult AjaxAdd(string appid, string targetPage, string queryId)
        {
            string[] arr = Session["UserData"] as string[];
            string userid = arr[0];
            object _genericObject;
            QueryDetails queryDetails;
            QueryMaster queryMaster;
            Comment comment;
            switch (targetPage)
            {
                case "QueryCommentRaised":

                    queryDetails = new QueryDetails()
                    {
                        FileRefId = Guid.NewGuid().ToString(),
                        ERApplicationId = appid
                    };

                    QueryCommentViewModel QueryCommentRaised = new QueryCommentViewModel();
                    QueryCommentRaised.QueryDetails = queryDetails;


                    if (arr[2] == "coordinator")
                    {
                        ViewBag.ReciverList = ERAppActiveUsersContext.Collection().Where(x => x.ERApplicationId == appid && x.UserAccountId != userid)
                             .Select(d => new { ListItemKey = d.UserAccountId, ListItemValue = d.UserAccount.FirstName + " " + d.UserAccount.LastName + " (" + d.UserAccount.UserRole + ")" }).ToList();

                    }
                    else if (arr[2] == "Consultant Enhanced Recovery")
                    {
                        ViewBag.ReciverList = ERAppActiveUsersContext.Collection().Where(x => x.ERApplicationId == appid && x.UserAccountId != userid && x.UserAccount.UserRole == "coordinator")
                                .Select(d => new { ListItemKey = d.UserAccountId, ListItemValue = d.UserAccount.FirstName + " " + d.UserAccount.LastName + " (" + d.UserAccount.UserRole + ")" }).ToList();

                    }
                    else if (arr[2] == "operator")
                    {
                        ViewBag.ReciverList = ERAppActiveUsersContext.Collection().Where(x => x.ERApplicationId == appid && x.UserAccountId != userid && x.UserAccount.UserRole == "Consultant Enhanced Recovery")
                             .Select(d => new { ListItemKey = d.UserAccountId, ListItemValue = d.UserAccount.FirstName + " " + d.UserAccount.LastName + " (" + d.UserAccount.UserRole + ")" }).ToList();

                    }
                    else
                    {
                        ViewBag.ReciverList = ERAppActiveUsersContext.Collection().Where(x => x.ERApplicationId == appid && x.UserAccountId != userid && x.UserAccount.UserRole != "operator")
                            .Select(d => new { ListItemKey = d.UserAccount.Id, ListItemValue = d.UserAccount.FirstName + " " + d.UserAccount.LastName + " (" + d.UserAccount.UserRole + ")" }).ToList();

                    }

                    _genericObject = QueryCommentRaised;

                    break;

                case "QueryCommentForward":

                    QueryCommentViewModel QueryCommentForward = new QueryCommentViewModel();
                    QueryDetails qd = QueryDetailsContext.Collection().Where(x => x.Id == queryId).FirstOrDefault();
                    queryDetails = new QueryDetails()
                    {
                        FileRefId = Guid.NewGuid().ToString(),
                        ERApplicationId = appid,
                        QueryParentId = qd.QueryParentId,
                    };

                    queryMaster = new QueryMaster()
                    {
                        Subject = QueryMasterContext.Collection().Where(d => d.Id == qd.QueryParentId).FirstOrDefault().Subject,
                    };
                    comment = new Comment()
                    {
                        Text = commentContext.Collection().Where(c => c.Id == qd.CommentRefId).FirstOrDefault().Text,
                    };

                    QueryCommentForward.QueryDetails = queryDetails;
                    QueryCommentForward.QueryMaster = queryMaster;
                    QueryCommentForward.Comment = comment;

                    QueryUser queryUser = QueryUserContext.Collection().Where(x => x.QueryId == qd.QueryParentId).FirstOrDefault();

                    if (arr[2] == "coordinator")
                    {
                        //  QueryUserContext.Collection().Where(x=>x.QueryId==queryId)
                        var qmc = QueryMasterContext.Collection().Where(e => e.ERApplicationId == appid).ToList();
                        var query = from p in qmc
                                    join q in qmc on p.Id equals q.QueryParentId
                                    select new { pid = p.Id, qid = q.Id };


                        ViewBag.ReciverList = ERAppActiveUsersContext.Collection().Where(x => x.ERApplicationId == appid && x.UserAccountId != userid && x.UserAccountId != queryUser.RecieverId && x.UserAccountId != queryUser.SenderId)
                             .Select(d => new { ListItemKey = d.UserAccountId, ListItemValue = d.UserAccount.FirstName + " " + d.UserAccount.LastName + " (" + d.UserAccount.UserRole + ")" }).ToList();
                    }
                    else
                    {
                        ViewBag.ReciverList = ERAppActiveUsersContext.Collection().Where(x => x.ERApplicationId == appid && x.UserAccountId != userid && x.UserAccount.UserRole != "operator" && x.UserAccountId != queryUser.RecieverId && x.UserAccountId != queryUser.SenderId)
                            .Select(d => new { ListItemKey = d.UserAccount.Id, ListItemValue = d.UserAccount.FirstName + " " + d.UserAccount.LastName + " (" + d.UserAccount.UserRole + ")" }).ToList();
                    }
                    _genericObject = QueryCommentForward;
                    break;
                case "QueryCommentReply":

                    QueryCommentViewModel QueryCommentReply = new QueryCommentViewModel();

                    queryDetails = new QueryDetails()
                    {
                        ERApplicationId = appid,
                        QueryParentId = QueryDetailsContext.Collection().Where(x => x.Id == queryId).Select(d => d.QueryParentId).FirstOrDefault().ToString(),
                        FileRefId = Guid.NewGuid().ToString()
                    };

                    QueryCommentReply.QueryDetails = queryDetails;

                    _genericObject = QueryCommentReply;
                    break;

                case "QueryCommentResolved":
                    _genericObject = new QueryCommentViewModel();

                    break;
                case "ForwardApplication":
                    ForwardAppViewModel forwardAppViewModel = new ForwardAppViewModel();
                    ForwardApplication forwardApplication = new ForwardApplication();
                    //if(arr[2]=="HoD")                   
                    if (arr[2] == UserRoleType.ConsultantEnhancedRecovery.GetDisplayName())
                    {
                        ViewBag.ReciverList = UserAccountContext.Collection().Where(x => (x.UserRole == UserRoleType.Coordinator.ToString()) && x.Id != userid)
                             .Select(d => new { ListItemKey = d.Id, ListItemValue = d.FirstName + " " + d.LastName + " (" + d.UserRole + ")" }).ToList();
                    }
                    else if (arr[2] == UserRoleType.Coordinator.ToString())
                    {
                        int CountForward = ForwardApplicationContext.Collection().Where(x => x.ERApplicationId == appid && x.Is_Active == true && x.Sender == userid && (x.FileStatus == FileStatus.ReviewAgain || x.FileStatus == FileStatus.Forward)).Count();
                        int CountRecommended = ForwardApplicationContext.Collection().Where(x => x.ERApplicationId == appid && x.Is_Active == true && x.Reciever == userid && x.FileStatus == FileStatus.Recommended).Count();
                        if (CountForward == CountRecommended && CountForward != 0 && CountRecommended != 0)
                        {
                            ViewBag.ReciverList = UserAccountContext.Collection().Where(x => x.UserRole == UserRoleType.DG.ToString() && x.Id != userid)
                          .Select(d => new { ListItemKey = d.Id, ListItemValue = d.FirstName + " " + d.LastName + " (" + d.UserRole + ")" }).ToList();

                        }
                        else
                        {
                            ViewBag.ReciverList = UserAccountContext.Collection().Where(x => (x.UserRole == UserRoleType.Hod.ToString()
                            || x.UserRole == UserRoleType.Coordinator.ToString() || x.UserRole == UserRoleType.ADG.ToString()) && x.Id != userid)
                                .Select(d => new { ListItemKey = d.Id, ListItemValue = d.FirstName + " " + d.LastName + " (" + d.UserRole + ")" }).ToList();
                        }

                    }
                    else
                    {
                        ViewBag.ReciverList = UserAccountContext.Collection().Where(x => (x.UserRole == UserRoleType.Hod.ToString()
                        || x.UserRole == UserRoleType.Coordinator.ToString() || x.UserRole == UserRoleType.ADG.ToString()) && x.Id != userid)
                              .Select(d => new { ListItemKey = d.Id, ListItemValue = d.FirstName + " " + d.LastName + " (" + d.UserRole + ")" }).ToList();
                    }

                    forwardApplication.FileRef = Guid.NewGuid().ToString();
                    forwardAppViewModel.ForwardApplication = forwardApplication;
                    _genericObject = forwardAppViewModel;
                    break;

                case "UploadApprovalLetter":
                    UploadApprovalLetterViewModel uploadApprovalLetterViewModel = new UploadApprovalLetterViewModel();
                    ForwardApplication forwardApplication1 = new ForwardApplication()
                    {
                        FileRef = Guid.NewGuid().ToString(),
                        ERApplicationId = appid
                    };
                    uploadApprovalLetterViewModel.ForwardApplication = forwardApplication1;
                    _genericObject = uploadApprovalLetterViewModel;
                    break;
                default:
                    _genericObject = null;
                    break;
            }
            if (null != _genericObject)
            {
                ViewBag.appid = appid;
                return View(targetPage, _genericObject);
            }
            else
            {

                return Content("<div class=\"alert alert-danger\" role=\"alert\"> An Error has occured </div>");
            }

        }
        [HttpPost]
        public JsonResult UploadApprovalLetter(string appid, UploadApprovalLetterViewModel uploadApprovalLetterViewModel)
        {
            string[] arr = Session["UserData"] as string[];
            uploadApprovalLetterViewModel.Comment.ERApplicationId = uploadApprovalLetterViewModel.ForwardApplication.ERApplicationId;
            uploadApprovalLetterViewModel.Comment.UserAccountId = arr[0];


            uploadApprovalLetterViewModel.ForwardApplication.CommentRefId = uploadApprovalLetterViewModel.Comment.Id;
            uploadApprovalLetterViewModel.ForwardApplication.Sender = arr[0];
            uploadApprovalLetterViewModel.ForwardApplication.Reciever = AuditTrailsContext.Collection().Where(x => x.ERApplicationId == appid && x.Is_Active == true && x.StatusId == "S101").FirstOrDefault().SenderId;
            uploadApprovalLetterViewModel.ForwardApplication.FileStatus = FileStatus.ApprovalLetter;
            uploadApprovalLetterViewModel.ForwardApplication.Is_Active = true;

            AuditTrails auditTrails = new AuditTrails()
            {
                SenderId = arr[0],
                ReceiverId = uploadApprovalLetterViewModel.ForwardApplication.Reciever,
                FileRefId = uploadApprovalLetterViewModel.ForwardApplication.FileRef,
                ERApplicationId = appid,
                Is_Active = true,
                StatusId = "S116"
            };
            AuditTrailsContext.Insert(auditTrails);
            ForwardApplicationContext.Insert(uploadApprovalLetterViewModel.ForwardApplication);
            commentContext.Insert(uploadApprovalLetterViewModel.Comment);

            if (TryValidateModel(uploadApprovalLetterViewModel))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    commentContext.Commit();
                    ForwardApplicationContext.Commit();
                    AuditTrailsContext.Commit();
                    scope.Complete();
                    return Json("Successfully Approval Letter Uploaded", JsonRequestBehavior.AllowGet);
                }
            }
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
                Recivername = userlist.Where(c => c.Id == x.Reciever)
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
                Files = UploadFileContext.Collection().Where(f => f.FIleRef == x.FileRef && x.Is_Active == true).ToList()
            }).OrderByDescending(x => x.CreatedAt).GroupBy(g => g.FileRef)
            .ToList();

            return Json(ForwardSummaryData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BtnCheckStatus(string appid, string btnType)
        {
            string[] arr = Session["UserData"] as string[];
            string userid = arr[0];
            string statuscheck = ButtonStatus.Hide.ToString();
            int recievecheck = 0;
            int approvedcount = 0;
            string dgid = "";
            int countdgid = 0;
            int countapprovalletter = 0;
            switch (btnType)
            {
                case "btnForward":
                    recievecheck = ERAppActiveUsersContext.Collection()
                        .Where(x => x.ERApplicationId == appid && x.UserAccountId == userid && x.Is_Active == true).Count();

                    if (recievecheck > 0)
                    {
                        statuscheck = ButtonStatus.Show.ToString();
                        if (AuditTrailsContext.Collection().Where(x => x.ERApplicationId == appid && x.Is_Active == true).Count() == 1)
                        {
                            statuscheck = ButtonStatus.Show.ToString();
                        }
                        else
                        {
                            int actionBtn = ForwardApplicationContext.Collection().Where(x => x.ERApplicationId == appid && x.FileStatus != FileStatus.CommentBack && x.Is_Active == true && x.Sender == userid).Count();

                            //  statuscheck = actionBtn == 0 ? "Hide" : "Show";
                            if (actionBtn > 0)
                            {
                                statuscheck = ButtonStatus.Hide.ToString();
                            }
                            else
                            {
                                int countcommentback = ForwardApplicationContext.Collection().Where(x => x.ERApplicationId == appid && (x.FileStatus == FileStatus.ReviewAgain || x.FileStatus == FileStatus.Forward) && x.Is_Active == true && x.Reciever == userid).Count();
                                approvedcount = ForwardApplicationContext.Collection()
                                    .Where(x => x.Sender == userid && (x.FileStatus == FileStatus.Recommended) && x.ERApplicationId == appid && x.Is_Active == true).Count();

                                statuscheck = countcommentback == 0 ? ButtonStatus.Hide.ToString() : approvedcount > 0 ? ButtonStatus.Hide.ToString() : ButtonStatus.Show.ToString();

                            }
                        }
                    }
                    else
                    {
                        statuscheck = ButtonStatus.Hide.ToString();
                    }
                    dgid = UserAccountContext.Collection().Where(u => u.UserRole == UserRoleType.DG.ToString()).FirstOrDefault().Id;
                    countdgid = AuditTrailsContext.Collection().Where(x => x.ERApplicationId == appid && x.Is_Active == true && x.SenderId == dgid && x.StatusId == "S105").Count();
                    statuscheck = arr[2] == UserRoleType.Coordinator.ToString() && countdgid == 0 ? ButtonStatus.Show.ToString() : statuscheck;

                    break;
                case "BtnUploadLetter":
                    dgid = UserAccountContext.Collection().Where(u => u.UserRole == UserRoleType.DG.ToString()).FirstOrDefault().Id;
                    countdgid = AuditTrailsContext.Collection().Where(x => x.ERApplicationId == appid && x.Is_Active == true && x.SenderId == dgid && x.StatusId == "S105").Count();

                    countapprovalletter = AuditTrailsContext.Collection().Where(x => x.ERApplicationId == appid && x.Is_Active == true && x.StatusId == "S116").Count();
                    statuscheck = countapprovalletter == 0 && countdgid == 1 ? ButtonStatus.Show.ToString() : statuscheck;
                    break;

                default:
                    return Json("ERROR", JsonRequestBehavior.AllowGet);
            }

            return Json(statuscheck, JsonRequestBehavior.AllowGet);
        }

        #region // Query

        // Query Raised Submit
        [HttpPost]
        public JsonResult QueryCommentSubmit(string appid, QueryCommentViewModel queryCommentViewModel)
        {
            string[] arr = Session["UserData"] as string[];


            string dt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("/", "").Replace(":", "").Replace(" ", "");
            queryCommentViewModel.QueryMaster.CustQueryId = "QMID" + dt;
            queryCommentViewModel.QueryDetails.CustQueryId = "QDID" + dt;
            queryCommentViewModel.Comment.ERApplicationId = appid;
            queryCommentViewModel.Comment.UserAccountId = arr[0];

            queryCommentViewModel.QueryMaster.ERApplicationId = appid;
            queryCommentViewModel.QueryMaster.Is_Active = true;
            queryCommentViewModel.QueryMaster.QueryParentId = null;

            queryCommentViewModel.QueryDetails.ERApplicationId = appid;
            queryCommentViewModel.QueryDetails.QuerySeq = 1;
            queryCommentViewModel.QueryDetails.CommentRefId = queryCommentViewModel.Comment.Id;
            queryCommentViewModel.QueryDetails.QueryParentId = queryCommentViewModel.QueryMaster.Id;
            queryCommentViewModel.QueryDetails.Status = StatusMasterContext.Collection().Where(x => x.Status == "Query Rasied").FirstOrDefault().Id;
            queryCommentViewModel.QueryDetails.Is_Active = true;

            QueryUser queryUser = new QueryUser()
            {
                SenderId = arr[0],
                RecieverId = queryCommentViewModel.ReciverIdSelectList[0],
                QueryId = queryCommentViewModel.QueryDetails.QueryParentId,
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

            if (TryValidateModel(queryCommentViewModel))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    commentContext.Commit();
                    AuditTrailsContext.Commit();
                    QueryDetailsContext.Commit();
                    QueryMasterContext.Commit();
                    QueryUserContext.Commit();
                    scope.Complete();
                    return Json("Successfully Query Rasied To Selected Users", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Something Went Wrong! Try Again Later");
            }

        }

        // Query Summary
        [HttpPost]
        public JsonResult QueryCommentSummary(string appid)
        {
            var userlist = UserAccountContext.Collection().ToList();
            var querydetail = QueryDetailsContext.Collection().Where(x => x.ERApplicationId == appid).ToList();
            var FinalData = querydetail.Select(x => new
            {
                x.Id,
                x.QueryParentId,
                x.ERApplicationId,
                x.CreatedAt,
                x.QuerySeq,
                Subject = QueryMasterContext.Collection().Where(d => d.Id == x.QueryParentId).Select(s => s.Subject).FirstOrDefault(),
                Comments = commentContext.Collection().Where(c => c.Id == x.CommentRefId).Select(m => m.Text).FirstOrDefault(),
                Status = StatusMasterContext.Collection().Where(s => s.Id == x.Status).Select(s => s.Status).FirstOrDefault(),
                Files = UploadFileContext.Collection().Where(f => f.FIleRef == x.FileRefId && x.Is_Active == true).ToList(),
                // SenderReciverId = QueryUserContext.Collection().Where(c => c.QueryId == x.Id).Select(u=> new { u.SenderId,u.RecieverId }).ToList(),

                Sender = userlist.Where(uu => uu.Id == (AuditTrailsContext.Collection().Where(q => q.QueryDetailsId == x.Id).Select(u => u.SenderId).FirstOrDefault()))
                 .Select(m => new { SenderName = m.FirstName + " " + m.LastName + " (" + m.UserRole + ")", SenderId = m.Id }).FirstOrDefault(),
                Reciver = userlist.Where(uu => uu.Id == (AuditTrailsContext.Collection().Where(q => q.QueryDetailsId == x.Id).Select(u => u.ReceiverId).FirstOrDefault()))
                 .Select(m => new { ReciverName = m.FirstName + " " + m.LastName + " (" + m.UserRole + ")", ReciverId = m.Id }).FirstOrDefault()

            }).OrderBy(d => d.CreatedAt.DateTime).GroupBy(d => new { d.QueryParentId, d.Subject }).ToList();

            return Json(FinalData, JsonRequestBehavior.AllowGet);

        }

        // Query Reply Submit
        [HttpPost]
        public JsonResult QueryReplySubmit(string queryid, QueryCommentViewModel queryCommentViewModel)
        {
            string[] arr = Session["UserData"] as string[];

            string userid = arr[0];
            string queryparent = queryCommentViewModel.QueryDetails.QueryParentId;
            string appid = queryCommentViewModel.QueryDetails.ERApplicationId;
            QueryUser queryUser = QueryUserContext.Collection().Where(x => x.QueryId == queryparent).FirstOrDefault();
            string receiverId = "";
            if (queryUser.RecieverId == arr[0])
            {
                receiverId = queryUser.SenderId;
            }
            else if (queryUser.SenderId == arr[0])
            {
                receiverId = queryUser.RecieverId;
            }
            else
            {
                receiverId = "";
            }

            queryCommentViewModel.QueryDetails.QuerySeq = QueryDetailsContext.Collection().Where(x => x.QueryParentId == queryparent).Count() + 1;
            queryCommentViewModel.QueryDetails.CommentRefId = queryCommentViewModel.Comment.Id;
            queryCommentViewModel.QueryDetails.Status = StatusMasterContext.Collection().Where(x => x.Status == "Query Reply").FirstOrDefault().Id;
            queryCommentViewModel.QueryDetails.Is_Active = true;

            queryCommentViewModel.Comment.UserAccountId = userid;
            queryCommentViewModel.Comment.ERApplicationId = appid;

            string dt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("/", "").Replace(":", "").Replace(" ", "");
            queryCommentViewModel.QueryDetails.CustQueryId = "QDID" + dt;

            AuditTrails auditTrails = new AuditTrails()
            {
                ERApplicationId = appid,
                FileRefId = queryCommentViewModel.QueryDetails.FileRefId,
                StatusId = queryCommentViewModel.QueryDetails.Status,
                QueryDetailsId = queryCommentViewModel.QueryDetails.Id,
                SenderId = arr[0],
                ReceiverId = receiverId,
                Is_Active = true,
            };
            AuditTrailsContext.Insert(auditTrails);
            QueryDetailsContext.Insert(queryCommentViewModel.QueryDetails);
            commentContext.Insert(queryCommentViewModel.Comment);

            if (TryValidateModel(queryCommentViewModel))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    commentContext.Commit();
                    AuditTrailsContext.Commit();
                    QueryDetailsContext.Commit();

                    scope.Complete();
                    return Json("Query Reply Successfully.", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Something Went Wrong! Try Again Later");
            }


        }
        // Query Forward Submit
        [HttpPost]
        public JsonResult QueryForwardSubmit(string queryid, QueryCommentViewModel queryCommentViewModel)
        {

            string[] arr = Session["UserData"] as string[];
            string appid = queryCommentViewModel.QueryDetails.ERApplicationId;

            queryCommentViewModel.Comment.ERApplicationId = appid;
            queryCommentViewModel.Comment.UserAccountId = arr[0];

            queryCommentViewModel.QueryMaster.Is_Active = true;
            queryCommentViewModel.QueryMaster.QueryParentId = queryCommentViewModel.QueryDetails.QueryParentId;
            queryCommentViewModel.QueryMaster.ERApplicationId = appid;


            queryCommentViewModel.QueryDetails.QuerySeq = 1;
            queryCommentViewModel.QueryDetails.CommentRefId = queryCommentViewModel.Comment.Id;
            queryCommentViewModel.QueryDetails.QueryParentId = queryCommentViewModel.QueryMaster.Id;
            queryCommentViewModel.QueryDetails.Status = StatusMasterContext.Collection().Where(x => x.Status == "Query Forward").FirstOrDefault().Id;
            queryCommentViewModel.QueryDetails.Is_Active = true;

            QueryUser queryUser = new QueryUser()
            {
                SenderId = arr[0],
                RecieverId = queryCommentViewModel.ReciverIdSelectList[0],
                QueryId = queryCommentViewModel.QueryDetails.QueryParentId,
            };

            AuditTrails auditTrails = new AuditTrails()
            {
                ERApplicationId = appid,
                FileRefId = queryCommentViewModel.QueryDetails.FileRefId,
                StatusId = queryCommentViewModel.QueryDetails.Status,
                QueryDetailsId = queryCommentViewModel.QueryDetails.Id,
                SenderId = queryUser.SenderId,
                ReceiverId = queryUser.RecieverId,
                Is_Active = true,
            };
            string dt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("/", "").Replace(":", "").Replace(" ", "");
            queryCommentViewModel.QueryMaster.CustQueryId = "QMID" + dt;
            queryCommentViewModel.QueryDetails.CustQueryId = "QDID" + dt;

            AuditTrailsContext.Insert(auditTrails);
            QueryUserContext.Insert(queryUser);
            commentContext.Insert(queryCommentViewModel.Comment);
            QueryDetailsContext.Insert(queryCommentViewModel.QueryDetails);
            QueryMasterContext.Insert(queryCommentViewModel.QueryMaster);

            if (TryValidateModel(queryCommentViewModel))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    commentContext.Commit();
                    AuditTrailsContext.Commit();
                    QueryDetailsContext.Commit();
                    QueryUserContext.Commit();
                    QueryMasterContext.Commit();
                    scope.Complete();
                    return Json("Successfully Query Forward To Selected User.", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Something Went Wrong! Try Again Later");
            }


        }
        #endregion

        #region Upload,Delete,Get Files
        [HttpPost]
        public ActionResult LoadUploadFile(HttpPostedFileBase file, string RefId)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string[] arr = Session["UserData"] as string[];
                    string _FileName = Path.GetFileName(file.FileName);
                    string NewFileName = RefId + _FileName;
                    string _path = Path.Combine(Server.MapPath("~/Content/Uploads/"), NewFileName);
                    file.SaveAs(_path);
                    //string FileRefId = RefId != null && RefId != "" ? RefId : Guid.NewGuid().ToString();
                    UploadFile uploadFile = new UploadFile() { FileName = _FileName, FilePath = _path, FIleRef = RefId, CreatedBy = arr[0], NewFileName = NewFileName };

                    UploadFileContext.Insert(uploadFile);
                    UploadFileContext.Commit();
                    return Json("File upload Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("File upload failed!!", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                string errormsg = e.Message;
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
            UploadFile uploadFile = UploadFileContext.Find(FileId);

            if (System.IO.File.Exists(uploadFile.FilePath))
            {
                System.IO.File.Delete(Path.Combine(uploadFile.FilePath));
                UploadFileContext.Delete(FileId);
                UploadFileContext.Commit();

                return Json("File Removed Success", JsonRequestBehavior.AllowGet);
            }
            return Json("File Not Removed", JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}