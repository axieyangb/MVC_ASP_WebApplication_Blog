﻿using System;
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
            return View();
        }


        [HttpPost]
        public JsonResult FileUpload()
        {
            LinkedList<retJsonModel> retList = new LinkedList<retJsonModel>();
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
                    ret.fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + '_' + file.FileName;
                    if (!String.IsNullOrEmpty(ret.UserID))
                    {
                        ret.URL = "/Content/img/article/" + ret.UserID + "/" + ret.fileName;
                        var path = Path.Combine(Server.MapPath("~/Content/img/article/" + ret.UserID + ""), ret.fileName);
                        var stream = file.InputStream;
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
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
