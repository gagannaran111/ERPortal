using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using ERPortal.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPortal.WebUI.Controllers
{
    public class OperatorController : Controller
    {
        IRepository<ERApplication> ERApplicationContext;
        IRepository<FieldType> FieldTypeContext;
        
        public OperatorController(IRepository<ERApplication> _ERApplicationContext, IRepository<FieldType> _FieldTypeContext)
        {
            ERApplicationContext = _ERApplicationContext;
            FieldTypeContext = _FieldTypeContext;
        }
        
        // GET: Operator
        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        public ActionResult SubmitERProposal()
        {
            ViewBag.Title = "Submit Proposal";

            OperatorERApplicationViewModel viewModel = new OperatorERApplicationViewModel();

            viewModel.ERApplications = new ERApplication();
            viewModel.FieldTypes = FieldTypeContext.Collection();

            return View(viewModel);
        }
    }
}