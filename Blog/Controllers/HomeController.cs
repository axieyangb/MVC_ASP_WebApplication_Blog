using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Simple.ImageResizer;
namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private readonly BlogContext _db = new BlogContext();
        [HttpGet]
        public ActionResult Index()
        {
            var homeArticle = from a in _db.Articles
                               join b in _db.Members on a.AuthorId equals b.UserId
                               select new ArticleAbstract
                               {
                                   ArticleId = a.ArticleId,
                                   AuthorId = b.UserId,
                                   Title = a.Title,
                                   SubTitle = a.SubTitle,
                                   PostDate = a.PostDate,
                                   AuthorName = String.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName,
                               };
            ViewBag.ArticleAmount = homeArticle.Count();
            homeArticle = homeArticle.OrderByDescending(i => i.PostDate).Take(10);
           
            return View(homeArticle.ToList());
        }

        [HttpGet]
        public ActionResult IndexPrev(int num)
        {
            var homeArticle = from a in _db.Articles
                               join b in _db.Members on a.AuthorId equals b.UserId
                               select new ArticleAbstract
                               {
                                   ArticleId = a.ArticleId,
                                   AuthorId = b.UserId,
                                   Title = a.Title,
                                   SubTitle = a.SubTitle,
                                   PostDate = a.PostDate,
                                   AuthorName = String.IsNullOrEmpty(b.NickName) ? b.UserName : b.NickName
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
                    ImageMetaDataModel metadata;
                    var imageMetaDate = new ImageMetaData(Server.MapPath(image.Url));
                    imageMetaDate.FetchData();
                    metadata = imageMetaDate.GetMetaData();
                    image.ContentType=upload.ContentType;
                    image.UpdateDate = DateTime.Now;
                    image.UserId = long.Parse(Session["LoggedUserID"].ToString());
                    image.Url = url;
                    image.FileName = imageName;
                    _db.Images.Add(image);
                    _db.ImageMetaData.Add(metadata);
                    _db.SaveChanges();
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
            var images = _db.Images.Where(a => a.UserId == id && a.IsBlock == 0 && a.DeleteTime == null);
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
            if (String.IsNullOrEmpty(ImageID))
                throw new ArgumentException("Argument is null or empty", nameof(ImageID));
            var imageId = long.Parse(ImageID);
            var oper = int.Parse(operation);
            var id = long.Parse(Session["LoggedUserID"].ToString());
            var query = from image in _db.Images
                        where image.ImageId == imageId && image.UserId == id 
                        select image;
            foreach (var one in query)
            {
                switch (oper)
                {
                    case 1:
                        one.IsPublish = 1;
                        break;
                    case 0:
                        one.IsPublish = 0;
                        break;
                    default:
                        var archivePath = Server.MapPath("/Content/Users/" + id + "/DelArchive");
                        if (!System.IO.Directory.Exists(archivePath))
                        {
                            System.IO.Directory.CreateDirectory(archivePath);
                        }
                        var splits = one.Url.Split('/');
                        System.IO.File.Move( Server.MapPath(one.Url), System.IO.Path.Combine(archivePath,splits[splits.Length-1]));
                        one.DeleteTime = DateTime.Now;
                        break;
                }
            }
            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            var images = _db.Images.Where(a => a.UserId == id && a.IsBlock == 0 && a.DeleteTime == null);
            return View("ViewImage", images);
        }


    }
}
