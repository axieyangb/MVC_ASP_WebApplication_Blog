using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Blog.Models;
namespace Blog.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: /Article/
        private BlogContext db = new BlogContext();
        [HttpGet]
        public ActionResult Index(long ArticleID=0)
        {
            Article article = db.Articles.Find(ArticleID);
            if (article == null)
                return HttpNotFound();
            article.Content=article.Content.Replace("\r\n", "<br>");
            if (ArticleID > 0)
            {
                var query = from a in db.Members
                            where a.UserID == article.AuthorID
                            select String.IsNullOrEmpty(a.NickName) ? a.UserName : a.NickName;
                ViewBag.AuthorName = query.ToList().ElementAt(0).ToString();
            }

            List<CommentDetailInfoView> comments = db.CommentDetailInfo.Where(a => (a.ArticleID == ArticleID && a.ReplyID==0)).ToList();
            ArticleStruct oneArticle = new ArticleStruct();
            oneArticle.article = article;
            oneArticle.rootComments = new List<CommentLevel>();
            CommentLevel oneComment;
            foreach (CommentDetailInfoView rootComment in comments)
            {
                oneComment = new CommentLevel();
                oneComment.parentComment = rootComment;
                oneComment.childComments = db.CommentDetailInfo.Where(a => (a.ArticleID == ArticleID && a.ReplyID == rootComment.CommenterID)).ToList();
                oneArticle.rootComments.Add(oneComment);
            }
            return View(oneArticle);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArticleReview(ArticleSubmitView articlePost)
        {
            Article article = new Article();
            article.AuthorID = articlePost.AuthorID;
            article.Title = HttpUtility.HtmlEncode(articlePost.Title);
            article.SubTitle = articlePost.SubTitle;
            article.Content = articlePost.Content;
            article.PostDate = System.DateTime.Now;
            ArticleStruct one = new ArticleStruct();
            one.article = article;
            one.rootComments = new List<CommentLevel>();
            if (articlePost.Action.Equals("post"))
            {
                if (ModelState.IsValid && !String.IsNullOrEmpty(article.Title))
                {
                   
                    db.Articles.Add(article);
                    db.SaveChanges();
                    return View("Index", one);
                }
                else return View();
            }
              
            else
            {
                ViewBag.isPreView = true;
                return View("Index", one);
            }
        }



    }
}
