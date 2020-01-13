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
    public class CommentController : Controller
    {
        IRepository<Comment> commentContext;
        public CommentController(IRepository<Comment> _commentContext)
        {
            commentContext = _commentContext;
        }
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Comment()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CommentSubmit(Comment comment)
        {
            if (ModelState.IsValid)
            {
               // commentContext.Insert(comment);
               // commentContext.Commit();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}