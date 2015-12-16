using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Blog.Models;
using System.Text.RegularExpressions;
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
            article.Content = article.Content.Replace("\r\n", "<br>");
            if (ArticleID > 0)
            {
                var query = from a in db.Members
                            where a.UserID == article.AuthorID
                            select String.IsNullOrEmpty(a.NickName) ? a.UserName : a.NickName;
                ViewBag.AuthorName = query.ToList().ElementAt(0).ToString();
            }

            List<CommentDetailInfoView> comments = db.CommentDetailInfo.Where(a => (a.ArticleID == ArticleID && ( a.ReplyID ==-1))).ToList();
            ArticleStruct oneArticle = new ArticleStruct();
            oneArticle.article = article;
            oneArticle.rootComments = new List<CommentLevel>();
            CommentLevel oneComment;
            foreach (CommentDetailInfoView rootComment in comments)
            {
                oneComment = new CommentLevel();
                oneComment.parentComment = rootComment;
                oneComment.childComments = db.CommentDetailInfo.Where(a => (a.ArticleID == ArticleID && a.ReplyID == rootComment.CommentID)).ToList();
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
            if (article.Content != null)
            {
                article.Content = articlePost.Content.Replace("style=\"height:", "name=\"height:").Replace("\r\n", "<br>");
                article.Content = HttpUtility.HtmlEncode(article.Content);
            }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentPost(ArticleStruct articlePost)
        {
            if (!String.IsNullOrEmpty(articlePost.commentArticle.Content))
            {
                articlePost.commentArticle.CommentID = null;
                articlePost.commentArticle.isValid = 1;
                if (Session["LoggedUserID"] !=null)
                articlePost.commentArticle.CommenterID =Int64.Parse(Session["LoggedUserID"].ToString());
                if (articlePost.commentArticle.ReplyID == null)
                    articlePost.commentArticle.ReplyID = -1;
                articlePost.commentArticle.ArticleID = articlePost.article.ArticleID;
                articlePost.commentArticle.CreateDate = System.DateTime.Now;
                articlePost.commentArticle.Content = HttpUtility.HtmlEncode(articlePost.commentArticle.Content.Replace("\r\n", "<br>"));
                db.ArticleComments.Add(articlePost.commentArticle);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Article", new{ArticleID=articlePost.article.ArticleID});
        }


        [HttpPost]
        public JsonResult ArticleUpdate()
        {
            retJsonModel ret = new retJsonModel();
            string ArticleContent  =HttpUtility.UrlDecode(Request.Form["Content"]);
            string userID_str = Request.Form["UserID"];
            string ArticeID_str = Request.Form["ArticleID"];
            try
            {
                long ArticleID = long.Parse(ArticeID_str);
                long userID = long.Parse(userID_str);
                Article article = db.Articles.Find(ArticleID);
                if (Session["LoggedUserID"].Equals(userID_str) && article.AuthorID == userID)
                {
                    article.Content = HttpUtility.HtmlEncode(ArticleContent.Replace("style=\"height:", "style=\"name:"));
                    article.ModifyDate = DateTime.Now;
                    db.SaveChanges();
                    ret.isAccept = 1;
                    ret.UserID = article.AuthorID.ToString();
                }
                else
                {
                    ret.isAccept = 0;
                    ret.Error = "Update Failed";
                }
            }
            catch (Exception ex)
            {
                ret.isAccept = 0;
                ret.Error = ex.ToString();
            }
            return Json(ret);
        }
         [HttpPost]
        public JsonResult TitleUpdate()
        {
            retJsonModel ret = new retJsonModel();
            string Title_str  =HttpUtility.UrlDecode(Request.Form["Title"]);
            string SubTitle_str = HttpUtility.UrlDecode(Request.Form["SubTitle"]);
            string userID_str = Request.Form["UserID"];
            string ArticeID_str = Request.Form["ArticleID"];
            try
            {
                long ArticleID = long.Parse(ArticeID_str);
                long userID = long.Parse(userID_str);
                Article article = db.Articles.Find(ArticleID);
                if (Session["LoggedUserID"].Equals(userID_str) && article.AuthorID == userID)
                {
                    article.Title = Regex.Replace(Title_str, "[^0-9a-zA-Z \u4E00-\u9FFF]+", "");
                    article.SubTitle = Regex.Replace(SubTitle_str, "[^0-9a-zA-Z \u4E00-\u9FFF]+", ""); 
                    article.ModifyDate = DateTime.Now;
                    db.SaveChanges();
                    ret.isAccept = 1;
                    ret.UserID = article.AuthorID.ToString();
                }
                else
                {
                    ret.isAccept = 0;
                    ret.Error = "Update Failed";
                }
            }
            catch (Exception ex)
            {
                ret.isAccept = 0;
                ret.Error = ex.ToString();
            }
            return Json(ret);
        }
        private class retJsonModel
        {
            public int isAccept { get; set; }
            public string UserID { get; set; }
            public string Error { get; set; }
        }


    }
}
