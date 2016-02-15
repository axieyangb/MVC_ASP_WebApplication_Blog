using System;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly BlogContext _db;

        public GalleryController()
        {
            _db = new BlogContext(ConfigurationManager.ConnectionStrings["BlogContext"].ConnectionString);
        }

        public GalleryController(BlogContext db)
        {
            _db = db;
        }
        [OutputCache(CacheProfile = "CacheFor300Seconds",VaryByHeader = "X-Requested-With")]
        public ActionResult Index(int pageIndex =0, int pageSize=12)
        {
            var publicImg = from x in _db.PublicImagesVw
                            orderby x.PublicId descending
                            select x;
            ViewBag.pageIndex = pageIndex;
            var count = (int)Math.Ceiling(publicImg.Count() / (double)pageSize);
            ViewBag.pageCount = count;
            return View(publicImg.Skip(pageIndex * pageSize).Take(pageSize));
        }

    }
}
