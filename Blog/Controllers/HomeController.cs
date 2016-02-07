using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
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
        public ActionResult Index()
        {
            var homeArticle = from a in Db.Articles
                              join b in Db.Members on a.AuthorId equals b.UserId
                              select new ArticleAbstract
                              {
                                  ArticleId = a.ArticleId,
                                  AuthorId = b.UserId,
                                  Title = a.Title,
                                  SubTitle = a.SubTitle,
                                  PostDate = a.PostDate,
                                  AuthorName = string.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName,
                              };
            ViewBag.ArticleAmount = homeArticle.Count();
            homeArticle = homeArticle.OrderByDescending(i => i.PostDate).Take(10);

            return View(homeArticle.ToList());
        }

        [HttpGet]
        public ActionResult IndexPrev(int num)
        {
            var homeArticle = from a in Db.Articles
                              join b in Db.Members on a.AuthorId equals b.UserId
                              select new ArticleAbstract
                              {
                                  ArticleId = a.ArticleId,
                                  AuthorId = b.UserId,
                                  Title = a.Title,
                                  SubTitle = a.SubTitle,
                                  PostDate = a.PostDate,
                                  AuthorName = string.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName
                              };
            ViewBag.ArticleAmount = homeArticle.Count();
            homeArticle = homeArticle.OrderBy(i => i.ArticleId).Take(num + 10);
            return View("Index", homeArticle.ToList());
        }

        [HttpPost]
        public ActionResult UploadImage(int id, HttpPostedFileWrapper upload)
        {
            string ret;
            if (upload != null)
            {
                if (upload.ContentLength <= 1024 * 1024 * 5)
                {
                    var imageName = DateTime.Now.ToString("yyyyMMddHHmmss") + '_' + upload.FileName;
                    if (!System.IO.Directory.Exists(Server.MapPath("/Content/Users/" + id)))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("/Content/users/" + id));
                    }
                    var path = System.IO.Path.Combine(Server.MapPath("/Content/users/" + id), imageName);
                    var url = "/Content/users/" + id + "/" + imageName;
                    upload.SaveAs(path);
                    var resizer = new ImageResizer(@path);
                    var thumbtailPath = System.IO.Path.Combine(Server.MapPath("~/Content/Users/" + id + "/thumbtail/"), upload.FileName);
                    resizer.Resize(400, 400, ImageEncoding.Jpg90);
                    if (!System.IO.Directory.Exists(Server.MapPath("~/Content/Users/" + id + "/thumbtail/")))
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Content/Users/" + id + "/thumbtail/"));
                    resizer.SaveToFile(@thumbtailPath);
                    var image = new ImageViewModel();
                    var imageMetaDate = new ImageMetaData(Server.MapPath(image.Url));
                    imageMetaDate.FetchData();
                    var metadata = imageMetaDate.GetMetaData();
                    image.ContentType = upload.ContentType;
                    image.UpdateDate = DateTime.Now;
                    image.UserID = long.Parse(Session["LoggedUserID"].ToString());
                    image.Url = url;
                    image.FileName = imageName;
                    Db.Images.Add(image);
                    Db.ImageMetaData.Add(metadata);
                    Db.SaveChanges();
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
            var images = Db.Images.Where(a => a.UserID == id && a.isBlock == 0 && a.DeleteTime == null);
            return View(images);
        }
        [HttpGet]
        public ActionResult ImageDetail(ImageViewModel image)
        {
            return View(image);
        }

        [HttpPost]
        // ReSharper disable once InconsistentNaming
        public ActionResult PicOperation(string ImageID, string operation)
        {
            var imageId = long.Parse(ImageID);
            var oper = int.Parse(operation);
            var id = long.Parse(Session["LoggedUserID"].ToString());
            var query = from image in Db.Images
                        where image.ImageID == imageId && image.UserID == id
                        select image;
            foreach (var one in query)
            {
                if (oper == 1)
                    one.isPublish = 1;
                else if (oper == 0)
                    one.isPublish = 0;
                else
                {
                    var archivePath = Server.MapPath("/Content/Users/" + id + "/DelArchive");
                    if (!System.IO.Directory.Exists(archivePath))
                    {
                        System.IO.Directory.CreateDirectory(archivePath);
                    }
                    var splits = one.Url.Split('/');
                    System.IO.File.Move(Server.MapPath(one.Url), System.IO.Path.Combine(archivePath, splits[splits.Length - 1]));
                    one.DeleteTime = DateTime.Now;
                }
            }
            try
            {
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            var images = Db.Images.Where(a => a.UserID == id && a.isBlock == 0 && a.DeleteTime == null);
            return View("ViewImage", images);
        }


    }
}
