using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Text.RegularExpressions;
using System.IO;
namespace Blog.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: /Article/
        private readonly BlogContext _db = new BlogContext();
        [HttpGet]
        public ActionResult Index(long articleId=0)
        {
            var article = _db.Articles.Find(articleId);
            if (article == null)
                return HttpNotFound();
            article.Content = article.Content.Replace("\r\n", "<br>");
            if (articleId > 0)
            {
                var query = from a in _db.Members
                            where a.UserId == article.AuthorId
                            select String.IsNullOrEmpty(a.NickName) ? a.UserName : a.NickName;
                ViewBag.AuthorName = query.ToList().ElementAt(0);
            }

            List<CommentDetailInfoView> comments = _db.CommentDetailInfo.Where(a => (a.ArticleId == articleId && ( a.ReplyId ==-1))).ToList();
            ArticleStruct oneArticle = new ArticleStruct
            {
                Article = article,
                RootComments = new List<CommentLevel>()
            };
            foreach (CommentDetailInfoView rootComment in comments)
            {
                var oneComment = new CommentLevel
                {
                    ParentComment = rootComment,
                    ChildComments =
                        _db.CommentDetailInfo.Where(
                            a => (a.ArticleId == articleId && a.ReplyId == rootComment.CommentId)).ToList()
                };
                oneArticle.RootComments.Add(oneComment);
            }
            /*fetch the emoji in Content\img\emoji    */
            string emojiPath = Server.MapPath("/Content/img/emoji/");
            DirectoryInfo emojiDir = new DirectoryInfo(emojiPath);
            try
            {
                DirectoryInfo[] categoryList = emojiDir.GetDirectories();
                ViewBag.emoji= new string[categoryList.Length];
                for (int i = 0; i < categoryList.Length; i++)
                    ViewBag.emoji[i] = categoryList[i].Name;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return View(oneArticle);
        }


        [HttpPost]
        public JsonResult GetEmoji()
        {
            string categoaryName = Request.Form["categoaryName"];
            DirectoryInfo emojis=new DirectoryInfo(Path.Combine(Server.MapPath("/Content/img/emoji/"),categoaryName));
            FileInfo[] oneCategory = emojis.GetFiles();
            string[] urls = new string[oneCategory.Length];
            for (int i = 0; i < oneCategory.Length; i++)
            {
                urls[i] = oneCategory[i].FullName.Replace(Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);
            }
            return Json(urls);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArticleReview(ArticleSubmitView articlePost)
        {
            Article article = new Article();
            article.AuthorId = articlePost.AuthorId;
            article.Title = HttpUtility.HtmlEncode(articlePost.Title);
            article.SubTitle = articlePost.SubTitle;
            article.Content = articlePost.Content;
            if (article.Content != null)
            {
                article.Content = articlePost.Content.Replace("style=\"height:", "name=\"height:").Replace("\r\n", "<br>");
                article.Content = HttpUtility.HtmlEncode(article.Content);
            }
            article.PostDate = DateTime.Now;
            ArticleStruct one = new ArticleStruct();
            one.Article = article;
            one.RootComments = new List<CommentLevel>();
            if (articlePost.Action.Equals("post"))
            {
                if (ModelState.IsValid && !String.IsNullOrEmpty(article.Title))
                {
                   
                    _db.Articles.Add(article);
                    _db.SaveChanges();
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
            if (!String.IsNullOrEmpty(articlePost.CommentArticle.Content))
            {
                articlePost.CommentArticle.CommentId = null;
                articlePost.CommentArticle.IsValid = 1;
                if (Session["LoggedUserID"] !=null)
                articlePost.CommentArticle.CommenterId =Int64.Parse(Session["LoggedUserID"].ToString());
                if (articlePost.CommentArticle.ReplyId == null)
                    articlePost.CommentArticle.ReplyId = -1;
                string address = Request.ServerVariables["REMOTE_ADDR"];
                articlePost.CommentArticle.IpAddress = address;
                articlePost.CommentArticle.ArticleId = articlePost.Article.ArticleId;
                articlePost.CommentArticle.CreateDate = DateTime.Now;
                articlePost.CommentArticle.Content = HttpUtility.HtmlEncode(articlePost.CommentArticle.Content.Replace("\r\n", "<br>"));
                _db.ArticleComments.Add(articlePost.CommentArticle);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Article", new{ArticleID=articlePost.Article.ArticleId});
        }


        [HttpPost]
        public JsonResult ArticleUpdate()
        {
            RetJsonModel ret = new RetJsonModel();
            string articleContent  =HttpUtility.UrlDecode(Request.Form["Content"]);
            string userIdStr = Request.Form["UserID"];
            string articeIdStr = Request.Form["ArticleID"];
            try
            {
                long articleId = long.Parse(articeIdStr);
                long userId = long.Parse(userIdStr);
                Article article = _db.Articles.Find(articleId);
                if (Session["LoggedUserID"].Equals(userIdStr) && article.AuthorId == userId)
                {
                    if (articleContent != null)
                        article.Content = HttpUtility.HtmlEncode(articleContent.Replace("style=\"height:", "style=\"name:"));
                    article.ModifyDate = DateTime.Now;
                    _db.SaveChanges();
                    ret.IsAccept = 1;
                    ret.UserId = article.AuthorId.ToString();
                }
                else
                {
                    ret.IsAccept = 0;
                    ret.Error = "Update Failed";
                }
            }
            catch (Exception ex)
            {
                ret.IsAccept = 0;
                ret.Error = ex.ToString();
            }
            return Json(ret);
        }
         [HttpPost]
        public JsonResult TitleUpdate()
        {
            RetJsonModel ret = new RetJsonModel();
            string titleStr  =HttpUtility.UrlDecode(Request.Form["Title"]);
            string subTitleStr = HttpUtility.UrlDecode(Request.Form["SubTitle"]);
            string userIdStr = Request.Form["UserID"];
            string articeIdStr = Request.Form["ArticleID"];
            try
            {
                long articleId = long.Parse(articeIdStr);
                long userId = long.Parse(userIdStr);
                Article article = _db.Articles.Find(articleId);
                if (Session["LoggedUserID"].Equals(userIdStr) && article.AuthorId == userId)
                {
                    if (titleStr != null) article.Title = Regex.Replace(titleStr, "[^0-9a-zA-Z \u4E00-\u9FFF]+", "");
                    if (subTitleStr != null)
                        article.SubTitle = Regex.Replace(subTitleStr, "[^0-9a-zA-Z \u4E00-\u9FFF]+", "");
                    article.ModifyDate = DateTime.Now;
                    _db.SaveChanges();
                    ret.IsAccept = 1;
                    ret.UserId = article.AuthorId.ToString();
                }
                else
                {
                    ret.IsAccept = 0;
                    ret.Error = "Update Failed";
                }
            }
            catch (Exception ex)
            {
                ret.IsAccept = 0;
                ret.Error = ex.ToString();
            }
            return Json(ret);
        }
         public class RetJsonModel
        {
            public int IsAccept { get; set; }
            public string UserId { get; set; }
            public string Error { get; set; }
        }


    }
}
