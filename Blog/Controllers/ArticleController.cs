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

            var comments = _db.CommentDetailInfo.Where(a => (a.ArticleId == articleId && ( a.ReplyId ==-1))).ToList();
            var oneArticle = new ArticleStruct
            {
                Article = article,
                RootComments = new List<CommentLevel>()
            };
            foreach (var rootComment in comments)
            {
                var oneComment = new CommentLevel();
                oneComment.ParentComment = rootComment;
                oneComment.ChildComments = _db.CommentDetailInfo.Where(a => (a.ArticleId == articleId && a.ReplyId == rootComment.CommentId)).ToList();
                oneArticle.RootComments.Add(oneComment);
            }
            /*fetch the emoji in Content\img\emoji    */
            var emojiPath = Server.MapPath("/Content/img/emoji/");
            var emojiDir = new DirectoryInfo(emojiPath);
            try
            {
                var categoryList = emojiDir.GetDirectories();
                ViewBag.emoji= new string[categoryList.Length];
                for (var i = 0; i < categoryList.Length; i++)
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
            var categoaryName = Request.Form["categoaryName"];
            var emojis=new DirectoryInfo(Path.Combine(Server.MapPath("/Content/img/emoji/"),categoaryName));
            var oneCategory = emojis.GetFiles();
            var urls = new string[oneCategory.Length];
            for (var i = 0; i < oneCategory.Length; i++)
            {
                urls[i] = oneCategory[i].FullName.Replace(Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);
            }
            return Json(urls);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArticleReview(ArticleSubmitView articlePost)
        {
            var article = new Article
            {
                AuthorId = articlePost.AuthorId,
                Title = HttpUtility.HtmlEncode(articlePost.Title),
                SubTitle = articlePost.SubTitle,
                Content = articlePost.Content
            };
            var tags = Request.Form["tags"];
            if (article.Content != null)
            {
                article.Content = articlePost.Content.Replace("style=\"height:", "name=\"height:").Replace("\r\n", "<br>");
                article.Content = HttpUtility.HtmlEncode(article.Content);
            }
            article.PostDate = DateTime.Now;
            var one = new ArticleStruct
            {
                Article = article,
                RootComments = new List<CommentLevel>()
            };
            if (articlePost.Action.Equals("post"))
            {
                if (ModelState.IsValid && !String.IsNullOrEmpty(article.Title))
                {
                    var tagsId=GetTagId(tags);
                    article.TagId1 = tagsId[0];
                    article.TagId2 = tagsId[1];
                    article.TagId3 = tagsId[2];
                    article.TagId4 = tagsId[3];
                    article.TagId5 = tagsId[4];
                    _db.Articles.Add(article);
                    _db.SaveChanges();
                    return View("Index", one);
                }
                else return View("Index", one);
            }
              
            else
            {
                ViewBag.isPreView = true;
                return View("Index", one);
            }
        }

        public long?[] GetTagId(string tags)
        {
            var splitsTags = tags.Split(',');
            var retId = new long?[5];
            for (var i = 0; i < splitsTags.Length; i++)
            {
                var tag = splitsTags[i];
                try
                {
                    var oneTag = _db.Tags.First(s => s.TagContent.Equals(tag));
                    retId[i] = oneTag.TagId;
                    oneTag.TagCount++;
                    oneTag.LastUsedDate = DateTime.Now;
                    _db.SaveChanges();
                }
               catch(Exception)
               {
                   var oneTag = new Tags
                   {
                       TagContent = splitsTags[i],
                       TagCount = 1
                   };
                   _db.Tags.Add(oneTag);
                   _db.SaveChanges();
                   retId[i] = oneTag.TagId;
               }
            }
            return retId;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentPost(ArticleStruct articlePost)
        {
            if (string.IsNullOrEmpty(articlePost.CommentArticle.Content))
                return RedirectToAction("Index", "Article", new {ArticleID = articlePost.Article.ArticleId});
            articlePost.CommentArticle.CommentId = null;
            articlePost.CommentArticle.IsValid = 1;
            if (Session["LoggedUserID"] !=null)
                articlePost.CommentArticle.CommenterId =long.Parse(Session["LoggedUserID"].ToString());
            if (articlePost.CommentArticle.ReplyId == null)
                articlePost.CommentArticle.ReplyId = -1;
            var address = Request.ServerVariables["REMOTE_ADDR"];
            articlePost.CommentArticle.IpAddress = address;
            articlePost.CommentArticle.ArticleId = articlePost.Article.ArticleId;
            articlePost.CommentArticle.CreateDate = DateTime.Now;
            articlePost.CommentArticle.Content = HttpUtility.HtmlEncode(articlePost.CommentArticle.Content.Replace("\r\n", "<br>"));
            _db.ArticleComments.Add(articlePost.CommentArticle);
            _db.SaveChanges();
            return RedirectToAction("Index", "Article", new{ArticleID = articlePost.Article.ArticleId});
        }


        [HttpPost]
        public JsonResult ArticleUpdate()
        {
            var ret = new RetJsonModel();
            var articleContent  =HttpUtility.UrlDecode(Request.Form["Content"]);
            var userIdStr = Request.Form["UserID"];
            var articeIdStr = Request.Form["ArticleID"];
            try
            {
                var articleId = long.Parse(articeIdStr);
                var userId = long.Parse(userIdStr);
                var article = _db.Articles.Find(articleId);
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
            var ret = new RetJsonModel();
            var titleStr  =HttpUtility.UrlDecode(Request.Form["Title"]);
            var subTitleStr = HttpUtility.UrlDecode(Request.Form["SubTitle"]);
            var userIdStr = Request.Form["UserID"];
            var articeIdStr = Request.Form["ArticleID"];
            try
            {
                var articleId = long.Parse(articeIdStr);
                var userId = long.Parse(userIdStr);
                var article = _db.Articles.Find(articleId);
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
            public string Error {  get; set; }
        }


    }
}
