using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.Mvc;

namespace Homework_04.Controllers
{
    public class AdminController : Controller
    {
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        public ActionResult Dashboard()
        {

            //if (User.IsInRole("Admin"))
            //{
                
            //}
            //Console.WriteLine(AuthenticationManager);
            ViewBag.Title = "Dashboard";
            return View("~/Views/Admin/Dashboard.cshtml");
            //return RedirectToAction("Index", "Home");

        }

        public ActionResult ManageResource()
        {

            //if (User.IsInRole("Admin"))
            //{

            //}
            //Console.WriteLine(AuthenticationManager);
            ViewBag.Title = "Dashboard";
            return View("~/Views/Admin/ManageResource.cshtml");
            //return RedirectToAction("Index", "Home");

        }

        public ActionResult PublishSurveys()
        {

            //if (User.IsInRole("Admin"))
            //{

            //}
            //Console.WriteLine(AuthenticationManager);
            ViewBag.Title = "Dashboard";
            return View("~/Views/Admin/PublishSurveys.cshtml");
            //return RedirectToAction("Index", "Home");

        }

        public ActionResult AnalyseResponses()
        {

            //if (User.IsInRole("Admin"))
            //{

            //}
            //Console.WriteLine(AuthenticationManager);
            ViewBag.Title = "Dashboard";
            return View("~/Views/Admin/AnalyseResponses.cshtml");
            //return RedirectToAction("Index", "Home");

        }
    }
}
