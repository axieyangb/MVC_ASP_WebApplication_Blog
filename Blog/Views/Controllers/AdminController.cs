using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Data.Entity;
namespace Blog.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Member member)
        {
            if (ModelState.IsValid)
            {
                using (BlogContext db = new BlogContext())
                {
                    var v = db.Members.Where(a => a.UserName.Equals(member.UserName) && a.Password.Equals(member.Password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LoggedUserID"] = v.UserID.ToString();
                        Session["LoggedUserName"] = String.IsNullOrEmpty(v.NickName) ? v.UserName : v.NickName;
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }
            return View(member);
        }
        public ActionResult Signup()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }



    }
}
