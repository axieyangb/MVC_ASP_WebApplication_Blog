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
        public ActionResult Index()
        {
            var publicImg = from x in db.PublicImagesVW
                            orderby x.PublicID descending
                            select x;
            return View(publicImg);
        }

    }
}
