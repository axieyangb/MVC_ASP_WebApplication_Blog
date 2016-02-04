using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Blog.Models;
namespace Blog.Controllers
{
    public class GalleryController : Controller
    {
        //
        // GET: /Gallery/
        private BlogContext db = new BlogContext();
        public ActionResult Index(int pageIndex =0, int pageSize=12)
        {
            var publicImg = from x in db.PublicImagesVW
                            orderby x.PublicID descending
                            select x;
            ViewBag.pageIndex = pageIndex;
            int count = (int)Math.Ceiling(publicImg.Count() / (double)pageSize);
            ViewBag.pageCount = count;
            return View(publicImg.Skip(pageIndex * pageSize).Take(pageSize));
        }

    }
}
