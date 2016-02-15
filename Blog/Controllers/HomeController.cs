using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net.Http.Headers;
using Blog.Models;
using Simple.ImageResizer;
namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        public BlogContext Db;

        public HomeController()
        {
            Db = new BlogContext(ConfigurationManager.ConnectionStrings["BlogContext"].ConnectionString);
        }
        public HomeController(string connectionString)
        {
            Db = new BlogContext(connectionString);
        }

        //
        // GET: /Home/
        [HttpGet]
        [OutputCache(CacheProfile = "CacheFor300Seconds",VaryByParam ="*")]
        public ActionResult Index()
        {
            ViewBag.currentPage = 1;
            ViewBag.lastPage = Math.Ceiling(Convert.ToDouble(Db.Articles.ToList().Count)/10);
            var homeArticle = (from a in Db.Articles
                join b in Db.Members on a.AuthorId equals b.UserId
                select new ArticleAbstract
                {
                    ArticleId = a.ArticleId,
                    AuthorId = b.UserId,
                    Title = a.Title,
                    SubTitle = a.SubTitle,
                    PostDate = a.PostDate,
                    AuthorName = string.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName,
                }).OrderByDescending(a => a.PostDate).Take(10);
           
            return View(homeArticle.ToList());
        }

        [HttpPost]
        [OutputCache(CacheProfile = "CacheFor300Seconds")]
        public ActionResult Index(int currentPage, int lastPage)
        {
            ViewBag.currentPage = currentPage;
            ViewBag.lastPage = lastPage;
            var homeArticle = (from a in Db.Articles
                              join b in Db.Members on a.AuthorId equals b.UserId
                              select new ArticleAbstract
                              {
                                  ArticleId = a.ArticleId,
                                  AuthorId = b.UserId,
                                  Title = a.Title,
                                  SubTitle = a.SubTitle,
                                  PostDate = a.PostDate,
                                  AuthorName = string.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName
                              }).OrderByDescending(a => a.PostDate).Skip((currentPage-1) * 10).Take(10);  
            return PartialView("_ArticlesList", homeArticle.ToList());
        }
    }
}
