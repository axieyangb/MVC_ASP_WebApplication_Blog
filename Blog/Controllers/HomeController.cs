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
        public ActionResult Index(long? tagId)
        {
            ViewBag.currentPage = 1;
            ViewBag.category = (from a in Db.Tags
                                select a).OrderByDescending(a => a.TagCount).Take(10).ToList();
            if (tagId != null)
            {
                ViewBag.filter = tagId;
                 
                ViewBag.lastPage =
                    Math.Ceiling(
                        Convert.ToDouble(
                            Db.Articles.Where(
                                a =>
                                    a.TagId1 == tagId || a.TagId2 == tagId || a.TagId3 == tagId || a.TagId4 == tagId ||
                                    a.TagId5 == tagId).ToList().Count)/10);
                var filtedArticles = (from a in Db.Articles
                                   join b in Db.Members on a.AuthorId equals b.UserId
                                   select new ArticleAbstract
                                   {
                                       ArticleId = a.ArticleId,
                                       AuthorId = b.UserId,
                                       Title = a.Title,
                                       SubTitle = a.SubTitle,
                                       PostDate = a.PostDate,
                                       AuthorName = string.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName,
                                       TagId1 = a.TagId1,
                                       TagId2 = a.TagId2,
                                       TagId3 = a.TagId3,
                                       TagId4 = a.TagId4,
                                       TagId5 = a.TagId5,
                                   }).Where(a => (a.TagId1 == tagId || a.TagId2 == tagId || a.TagId3 == tagId || a.TagId4 == tagId || a.TagId5 == tagId)).OrderByDescending(a => a.PostDate).Take(10);
                return View(filtedArticles.ToList());

            }
            else
            {
                ViewBag.lastPage = Math.Ceiling(Convert.ToDouble(Db.Articles.ToList().Count)/10);
                var articles = (from a in Db.Articles
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
                return View(articles.ToList());
            }
        
        }

        [HttpPost]
        public ActionResult Index(int currentPage, int lastPage,int? tagId)
        {
            ViewBag.currentPage = currentPage;
            ViewBag.lastPage = lastPage;
            if (tagId == null)
            {
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
                    }).OrderByDescending(a => a.PostDate).Skip((currentPage - 1)*10).Take(10);
                return PartialView("_ArticlesList", homeArticle.ToList());
            }
            else
            {
                ViewBag.filter = tagId;
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
                                       TagId1 = a.TagId1,
                                       TagId2 = a.TagId2,
                                       TagId3= a.TagId3,
                                       TagId4 = a.TagId4,
                                       TagId5 = a.TagId5,
                                   }).Where(a=>a.TagId1==tagId||a.TagId2==tagId||a.TagId3==tagId||a.TagId4==tagId||a.TagId5==tagId).OrderByDescending(a => a.PostDate).Skip((currentPage - 1) * 10).Take(10);
                return PartialView("_ArticlesList", homeArticle.ToList());
            }
           
        }
    }
}
