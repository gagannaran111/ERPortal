using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;

namespace ERPortal.WebUI.Models
{

    public class BaseController : Controller
    {
        // This method helps to render a partial view into html string.
        // http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
        // Credit: Kevin Craft
        public string RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult =
                    ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext,
                    viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        // This method helps to get the error information from the MVC "ModelState".
        // We can not directly send the ModelState to the client in Json. The "ModelState"
        // object has some circular reference that prevents it to be serialized to Json.
        public Dictionary<string, object> GetErrorsFromModelState()
        {
            var errors = new Dictionary<string, object>();
            foreach (var key in ModelState.Keys)
            {
                // Only send the errors to the client.
                if (ModelState[key].Errors.Count > 0)
                {
                    errors[key] = ModelState[key].Errors;
                }
            }

            return errors;
        }
    }

}