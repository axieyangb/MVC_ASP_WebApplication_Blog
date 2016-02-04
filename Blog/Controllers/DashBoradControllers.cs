using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.IO;
using System.Web.Script.Serialization;
using Simple.ImageResizer;
namespace Blog.Controllers
{
    public class DashBoardController : Controller
    {
        //
        // GET: /DashBoard/
        private readonly BlogContext _db = new BlogContext();
        public ActionResult Index()
        {
            if (Session["LoggedUserID"] != null)
                return View();
            else
                return RedirectToAction("Login", "Admin");
        }
        public ActionResult Post()
        {
            if (Session["LoggedUserID"] != null)
                return View();
            else
                return RedirectToAction("Login", "Admin");
        }
        public ActionResult Upload()
        {
            if (Session["LoggedUserID"] != null)
                return View();
            else
                return RedirectToAction("Login", "Admin");
        }


        [HttpPost]
        public JsonResult FileUpload()
        {
            LinkedList<RetJsonModel> retList = new LinkedList<RetJsonModel>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                RetJsonModel ret = new RetJsonModel();
                if (file != null)
                {
                    ret.ContentType = file.ContentType;
                    ret.UserId = Session["LoggedUserID"].ToString();
                    if (ret.ContentType.Contains("image/"))
                    {
                        ret.IsAccept = 0;
                        ret.FileTypeAccept = "yes";
                        ret.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + '_' + i + '_' + file.FileName;
                        if (!String.IsNullOrEmpty(ret.UserId))
                        {
                       
                            ret.URL = "/Content/Users/" + ret.UserId + "/" + ret.FileName;
                            var path = Path.Combine(Server.MapPath("~/Content/Users/" + ret.UserId + ""), ret.FileName);
                            var stream = file.InputStream;
                            using (var fileStream = System.IO.File.Create(path))
                            {
                                stream.CopyTo(fileStream);
                            }
                            ImageResizer resizer = new ImageResizer(@path);
                            var thumbtailPath= Path.Combine(Server.MapPath("~/Content/Users/" + ret.UserId + "/thumbtail/"), ret.FileName);
                            resizer.Resize(400, 400, ImageEncoding.Jpg90);
                            if (!Directory.Exists(Server.MapPath("~/Content/Users/" + ret.UserId + "/thumbtail/")))
                                Directory.CreateDirectory(Server.MapPath("~/Content/Users/" + ret.UserId + "/thumbtail/"));
                            resizer.SaveToFile(@thumbtailPath);
                            ImageViewModel image = new ImageViewModel();
                            var imageMetaDate = new ImageMetaData(Server.MapPath(ret.URL));
                            image.FileName = ret.FileName;
                            image.UpdateDate = DateTime.Now;
                            image.UserId = long.Parse(ret.UserId);
                            image.Url = ret.URL;
                            image.ContentType = file.ContentType;
                            _db.Images.Add(image);
                            imageMetaDate.FetchData();
                            var metadata = imageMetaDate.GetMetaData();
                            _db.ImageMetaData.Add(metadata);
                            _db.SaveChanges();
                        }
                        else
                        {
                            ret.IsAccept = 1;
                            ret.Error = "Member Session Expired";
                        }

                    }
                    else
                    {
                        ret.IsAccept = 1;
                        ret.FileName = file.FileName;
                        ret.Error = "Content Type Deny";
                    }
                }
                retList.AddLast(ret);

            }
            var javaScriptSerializer = new JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(retList);
            return Json(jsonString);
        }

        public class RetJsonModel
        {
            public int IsAccept { get; set; }
            public string FileTypeAccept { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            // ReSharper disable once InconsistentNaming
            public string URL { get; set; }
            public string UserId { get; set; }
            public string Error { get; set; }
        }
        public ActionResult EditProfile()
        {
            return View();
        }
    }

    
}
