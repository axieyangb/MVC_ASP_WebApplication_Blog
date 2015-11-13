using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class DashBoardController : Controller
    {
        //
        // GET: /DashBoard/

        public ActionResult Index()
        {
            if (Session["LoggedUserID"] != null)
                return View();
            else
                return RedirectToAction("Login", "Admin");
        }
        public ActionResult Post()
        {
            if (Session["LoggedUserID"] != null)
                return View();
            else
                return RedirectToAction("Login", "Admin");
        }
    }
}
