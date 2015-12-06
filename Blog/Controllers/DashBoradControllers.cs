using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Data.Entity;
namespace Blog.Controllers
{
    public class DashBoardController : Controller
    {
        //
        // GET: /DashBoard/
        private BlogContext db = new BlogContext();
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
        public ActionResult Upload()
        {
            return View();
        }


        [HttpPost]
        public JsonResult FileUpload()
        {
            foreach (var file in Request.Files)
            {
                
            }
            return Json(new { result = true });
        }
    }
}
