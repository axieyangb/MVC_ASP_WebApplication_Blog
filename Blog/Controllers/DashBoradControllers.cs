using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Data.Entity;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
namespace Blog.Controllers
{
    public class DashBoardController : Controller
    {
        //
        // GET: /DashBoard/
        private BlogContext db = new BlogContext();
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
            LinkedList<retJsonModel> retList = new LinkedList<retJsonModel>();
            ImageMetaData imageMetaDate;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                retJsonModel ret = new retJsonModel();
                ret.ContentType = file.ContentType;
                ret.UserID = Session["LoggedUserID"].ToString();
                if (ret.ContentType.Contains("image/") || ret.ContentType.Contains("video/"))
                {
                    ret.isAccept = 0;
                    ret.fileTypeAccept = "yes";
                    ret.fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + '_' + i + '_' + file.FileName;
                    if (!String.IsNullOrEmpty(ret.UserID))
                    {
                        ret.URL = "/Content/Users/" + ret.UserID + "/" + ret.fileName;
                        var path = Path.Combine(Server.MapPath("~/Content/Users/" + ret.UserID + ""), ret.fileName);
                        var stream = file.InputStream;
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        ImageViewModel image = new ImageViewModel();
                        ImageMetaDataModel metadata = new ImageMetaDataModel();
                        imageMetaDate = new ImageMetaData(Server.MapPath(ret.URL));
                        image.FileName = ret.fileName;
                        image.UpdateDate = System.DateTime.Now;
                        image.UserID = long.Parse(ret.UserID);
                        image.Url = ret.URL;
                        image.ContentType = file.ContentType;
                        db.Images.Add(image);
                        imageMetaDate.fetchData();
                        metadata = imageMetaDate.getMetaData();
                        db.ImageMetaData.Add(metadata);
                        db.SaveChanges();
                    }
                    else
                    {
                        ret.isAccept = 1;
                        ret.Error = "Member Session Expired";
                    }

                }
                else
                {
                    ret.isAccept = 1;
                    ret.fileName = file.FileName;
                    ret.Error = "Content Type Deny";
                }
                retList.AddLast(ret);

            }
            var javaScriptSerializer = new JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(retList);
            return Json(jsonString);
        }

        private class retJsonModel
        {
            public int isAccept { get; set; }
            public string fileTypeAccept { get; set; }
            public string fileName { get; set; }
            public string ContentType { get; set; }
            public string URL { get; set; }
            public string UserID { get; set; }
            public string Error { get; set; }
        }
    }
}
