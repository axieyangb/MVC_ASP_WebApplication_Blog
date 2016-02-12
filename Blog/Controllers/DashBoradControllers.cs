﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Data.Entity;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Simple.ImageResizer;
namespace Blog.Controllers
{
    public class DashBoardController : Controller
    {
        //
        // GET: /DashBoard/
        private readonly BlogContext _db = new BlogContext(ConfigurationManager.ConnectionStrings["BlogContext"].ConnectionString);
        public ActionResult Index()
        {
            if (Session["LoggedUserID"] != null)
                return View();
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult Post()
        {
            if (Session["LoggedUserID"] != null)
                return View();
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult Upload()
        {
            if (Session["LoggedUserID"] != null)
                return View();
            return RedirectToAction("Login", "Admin");
        }
      [HttpGet]
        public ActionResult ViewImage(int id)
        {
            if (Session["LoggedUserID"] == null)
            {
                return Content("Sorry ! please login first <a href='/Admin/Login'>Click Here to Login</a>");
            }
            var images = _db.Images.Where(a => a.UserID == id && a.isBlock == 0 && a.DeleteTime == null);
            return View(images);
        }


        [HttpPost]
        // ReSharper disable once InconsistentNaming
        public ActionResult PicOperation(string ImageID, string operation)
        {
            var imageId = long.Parse(ImageID);
            var oper = int.Parse(operation);
            var id = long.Parse(Session["LoggedUserID"].ToString());
            var query = from image in _db.Images
                        where image.ImageID == imageId && image.UserID == id
                        select image;
            foreach (var one in query)
            {
                switch (oper)
                {
                    case 1:
                        one.isPublish = 1;
                        break;
                    case 0:
                        one.isPublish = 0;
                        break;
                    default:
                        var archivePath = Server.MapPath("/Content/Users/" + id + "/DelArchive");
                        if (!Directory.Exists(archivePath))
                        {
                            Directory.CreateDirectory(archivePath);
                        }
                        var splits = one.Url.Split('/');
                        System.IO.File.Move(Server.MapPath(one.Url), Path.Combine(archivePath, splits[splits.Length - 1]));
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
            var images = _db.Images.Where(a => a.UserID == id && a.isBlock == 0 && a.DeleteTime == null);
            return View("ViewImage", images);
        }

        [HttpPost]
        public JsonResult FileUpload()
        {
            var retList = new LinkedList<RetJsonModel>();
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file == null) continue;
                var ret = new RetJsonModel
                {
                    ContentType = file.ContentType,
                    UserId = Session["LoggedUserID"].ToString()
                };
                if (ret.ContentType.Contains("image/"))
                {
                    ret.IsAccept = 0;
                    ret.FileTypeAccept = "yes";
                    ret.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + '_' + i + '_' + file.FileName;
                    if (!string.IsNullOrEmpty(ret.UserId))
                    {

                        ret.Url = "/Content/Users/" + ret.UserId + "/" + ret.FileName;
                        var path = Path.Combine(Server.MapPath("~/Content/Users/" + ret.UserId + ""), ret.FileName);
                        var stream = file.InputStream;
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        var resizer = new ImageResizer(@path);
                        var thumbtailPath = Path.Combine(Server.MapPath("~/Content/Users/" + ret.UserId + "/thumbtail/"), ret.FileName);
                        resizer.Resize(400, 400, ImageEncoding.Jpg90);
                        if (!Directory.Exists(Server.MapPath("~/Content/Users/" + ret.UserId + "/thumbtail/")))
                            Directory.CreateDirectory(Server.MapPath("~/Content/Users/" + ret.UserId + "/thumbtail/"));
                        resizer.SaveToFile(@thumbtailPath);
                        var image = new ImageViewModel();
                        var imageMetaDate = new ImageMetaData(Server.MapPath(ret.Url));
                        image.FileName = ret.FileName;
                        image.UpdateDate = DateTime.Now;
                        image.UserID = long.Parse(ret.UserId);
                        image.Url = ret.Url;
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
                retList.AddLast(ret);
            }
            var javaScriptSerializer = new JavaScriptSerializer();
            var jsonString = javaScriptSerializer.Serialize(retList);
            return Json(jsonString);
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

        public class RetJsonModel
        {
            public int IsAccept { get; set; }
            public string FileTypeAccept { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public string Url { get; set; }
            public string UserId { get; set; }
            public string Error { get; set; }
        }
        public ActionResult EditProfile()
        {
            return View();
        }
    }


}
