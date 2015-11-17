using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Blog.Models;
namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private BlogContext db = new BlogContext();
        [HttpGet]
        public ActionResult Index()
        {
            var Home_Article = from a in db.Articles
                               join b in db.Members on a.AuthorID equals b.UserID
                               select new ArticleAbstract
                               {
                                   ArticleID = a.ArticleID,
                                   AuthorID = b.UserID,
                                   Title = a.Title,
                                   SubTitle = a.SubTitle,
                                   PostDate = a.PostDate,
                                   AuthorName = String.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName
                               };
            ViewBag.ArticleAmount = Home_Article.Count();
           Home_Article=Home_Article.OrderBy(i => i.ArticleID).Take(10);
            return View(Home_Article.ToList());
        }

        [HttpGet]
        public ActionResult IndexPrev(int num)
        {
            var Home_Article = from a in db.Articles
                               join b in db.Members on a.AuthorID equals b.UserID
                               select new ArticleAbstract
                               {
                                   ArticleID = a.ArticleID,
                                   AuthorID = b.UserID,
                                   Title = a.Title,
                                   SubTitle = a.SubTitle,
                                   PostDate = a.PostDate,
                                   AuthorName = String.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName
                               };
            ViewBag.ArticleAmount = Home_Article.Count();
            Home_Article = Home_Article.OrderBy(i => i.ArticleID).Take(num+10);
            return View("Index",Home_Article.ToList());
        }
        public ActionResult UploadImage(int id, HttpPostedFileWrapper upload)
        {
            string url="";
            if (upload != null)
            {
                string ImageName = DateTime.Now.ToString("yyyyMMddHHmmss") +'_' + upload.FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("/Content/img/article/" + id)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("/Content/img/article/" + id));
                }
                string path = System.IO.Path.Combine(Server.MapPath("/Content/img/article/" + id), ImageName);
                url = "/Content/img/article/" + id + "/" + ImageName;
                upload.SaveAs(path);
                ImageView image = new ImageView();
                image.UpdateDate=System.DateTime.Now;
                image.UserID = long.Parse(Session["LoggedUserID"].ToString());
                image.Url=url;
                db.Images.Add(image);
                db.SaveChanges();
                
            }

            return Content(url);
        }
        public ActionResult ViewImage(int id)
        {
            if (Session["LoggedUserID"] == null)
            {
                return Content("Sorry ! please login first");
            }
            var Imgdir = Server.MapPath("/Content/img/article/" + id);
            var images = db.Images.Where(a => a.UserID == id);
            return View(images);
        }
        public ActionResult ImageDetail(ImageView image)
        {
            return View(image);
        }


    }
}
