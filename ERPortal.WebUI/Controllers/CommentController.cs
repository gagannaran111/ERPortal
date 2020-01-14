using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.Core.ViewModels;
using Microsoft.AspNet.Identity;

namespace ERPortal.WebUI.Controllers
{
    public class CommentController : Controller
    {
        IRepository<Comment> commentContext;
         IRepository<ERApplication> eRApplicationContext;
        public CommentController(IRepository<Comment> _commentContext, IRepository<ERApplication> _eRApplicationContext)
        {
            eRApplicationContext = eRApplicationContext;
            commentContext = _commentContext;
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
        public JsonResult CommentSubmit(Comment comment,string appid)
        {
            comment.ERApplicationId = appid;
        // comment.UserAccountId=  
            if (ModelState.IsValid)
            {
                commentContext.Insert(comment);
                commentContext.Commit();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}