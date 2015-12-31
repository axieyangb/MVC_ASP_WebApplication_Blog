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
                                   AuthorName = String.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName,
                               };
            ViewBag.ArticleAmount = Home_Article.Count();
            Home_Article = Home_Article.OrderBy(i => i.ArticleID).Take(10);
           
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
            Home_Article = Home_Article.OrderBy(i => i.ArticleID).Take(num + 10);
            return View("Index", Home_Article.ToList());
        }

        [HttpPost]
        public ActionResult UploadImage(int id, HttpPostedFileWrapper upload)
        {
            string ret = "";
            if (upload != null)
            {
                if (upload.ContentLength <= 1024 * 1024 * 5)
                {
                    string url = "";
                    string ImageName = DateTime.Now.ToString("yyyyMMddHHmmss") + '_' + upload.FileName;
                    if (!System.IO.Directory.Exists(Server.MapPath("/Content/Users/" + id)))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("/Content/users/" + id));
                    }
                    string path = System.IO.Path.Combine(Server.MapPath("/Content/users/" + id), ImageName);
                    url = "/Content/users/" + id + "/" + ImageName;
                    upload.SaveAs(path);
                    ImageViewModel image = new ImageViewModel();
                    ImageMetaDataModel metadata = new ImageMetaDataModel();
                    ImageMetaData imageMetaDate = new ImageMetaData(Server.MapPath(image.Url));
                    imageMetaDate.fetchData();
                    metadata = imageMetaDate.getMetaData();
                    image.ContentType=upload.ContentType;
                    image.UpdateDate = System.DateTime.Now;
                    image.UserID = long.Parse(Session["LoggedUserID"].ToString());
                    image.Url = url;
                    image.FileName = ImageName;
                    db.Images.Add(image);
                    db.ImageMetaData.Add(metadata);
                    db.SaveChanges();
                    ret = url;
                }
                else
                {
                    ret = "<p style=\"color:red\">image exceed the maximum 5MB</p>";
                }
            }
            else
            {
                ret = "<p style=\"color:red\">image is empty</p>";
            }
            return Content(ret);
        }

        [HttpGet]
        public ActionResult ViewImage(int id)
        {
            if (Session["LoggedUserID"] == null)
            {
                return Content("Sorry ! please login first <a href='/Admin/Login'>Click Here to Login</a>");
            }
            var images = db.Images.Where(a => a.UserID == id && a.isBlock == 0 && a.DeleteTime == null);
            return View(images);
        }
        [HttpGet]
        public ActionResult ImageDetail(ImageViewModel image)
        {
            return View(image);
        }

        [HttpPost]
        public ActionResult PicOperation(string ImageID, string operation)
        {
            long imageId = long.Parse(ImageID);
            int oper = int.Parse(operation);
            long id = long.Parse(Session["LoggedUserID"].ToString());
            var query = from image in db.Images
                        where image.ImageID == imageId && image.UserID == id 
                        select image;
            foreach (ImageViewModel one in query)
            {
                if (oper == 1)
                    one.isPublish = 1;
                else if (oper == 0)
                    one.isPublish = 0;
                else
                {
                    string archivePath = Server.MapPath("/Content/Users/" + id + "/DelArchive");
                    if (!System.IO.Directory.Exists(archivePath))
                    {
                        System.IO.Directory.CreateDirectory(archivePath);
                    }
                    string [] splits = one.Url.Split('/');
                    System.IO.File.Move( Server.MapPath(one.Url), System.IO.Path.Combine(archivePath,splits[splits.Length-1]));
                    one.DeleteTime = System.DateTime.Now;
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            var images = db.Images.Where(a => a.UserID == id && a.isBlock == 0 && a.DeleteTime == null);
            return View("ViewImage", images);
        }


    }
}
