using System;
using System.Linq;
using System.Web.Mvc;
using Blog.Models;
namespace Blog.Controllers
{
    public class GalleryController : Controller
    {
        //
        // GET: /Gallery/
        private BlogContext _db = new BlogContext();
        public ActionResult Index(int pageIndex =0, int pageSize=12)
        {
            var publicImg = from x in _db.PublicImagesVw
                            orderby x.PublicId descending
                            select x;
            ViewBag.pageIndex = pageIndex;
            int count = (int)Math.Ceiling(publicImg.Count() / (double)pageSize);
            ViewBag.pageCount = count;
            return View(publicImg.Skip(pageIndex * pageSize).Take(pageSize));
        }

    }
}
