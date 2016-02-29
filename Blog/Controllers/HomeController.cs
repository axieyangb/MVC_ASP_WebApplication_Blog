using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Objects;
using System.Globalization;
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
                                    a.TagId5 == tagId).ToList().Count) / 10);
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
                ViewBag.lastPage = Math.Ceiling(Convert.ToDouble(Db.Articles.ToList().Count) / 10);
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
        public ActionResult Index(int currentPage, int lastPage, string filterField, string matchKey, int? tagId)
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
                                   AuthorName = string.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName,
                                   TagId1 = a.TagId1,
                                   TagId2 = a.TagId2,
                                   TagId3 = a.TagId3,
                                   TagId4 = a.TagId4,
                                   TagId5 = a.TagId5
                               });

            if (tagId != null)
            {

                ViewBag.filter = tagId;
                homeArticle =
                    homeArticle.Where(
                        a =>
                            a.TagId1 == tagId || a.TagId2 == tagId || a.TagId3 == tagId || a.TagId4 == tagId ||
                            a.TagId5 == tagId);
            }
            if (!filterField.Equals("")&& !matchKey.Equals(""))
            {
                ViewBag.filterField = filterField;
                ViewBag.matchKey = matchKey;
                switch (filterField.ToUpper())
                {
                    case "TITLE":
                        homeArticle=homeArticle.Where(a => a.Title.Contains(matchKey));
                        break;
                    case "SUBTITLE":
                        homeArticle = homeArticle.Where(a => a.SubTitle.Contains(matchKey));
                        break;
                    case "YEAR":
                        var givenOnlyYear = int.Parse(matchKey);
                        homeArticle = homeArticle.Where(a => a.PostDate.Year == givenOnlyYear);
                        break;
                    case "MONTH":
                        var givenMonth = Convert.ToDateTime(matchKey).Month;
                        var givenYear = Convert.ToDateTime(matchKey).Year;
                        homeArticle = homeArticle.Where(a => a.PostDate.Month == givenMonth && a.PostDate.Year==givenYear);
                        break;
                    case "DAY":
                        var givenDate =Convert.ToDateTime(matchKey).Date;
                        homeArticle = homeArticle.Where(a => EntityFunctions.TruncateTime(a.PostDate) == givenDate);
                        break;
                    case "AUTHOR":
                        homeArticle = homeArticle.Where(a => a.AuthorName.Contains(matchKey));
                        break;
                    default:
                        break;
                }
                if (currentPage == 1)
                {
                    ViewBag.lastPage = Math.Ceiling(
                        Convert.ToDouble(homeArticle.Count()) / 10);
                }
            }
            homeArticle = homeArticle.OrderByDescending(a => a.PostDate).Skip((currentPage - 1) * 10).Take(10);
            return PartialView("_ArticlesList", homeArticle.ToList());
        }

    }


}
