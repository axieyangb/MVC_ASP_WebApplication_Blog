using System.Web.Mvc;

namespace Blog.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            Response.StatusCode = 404;
            return View();
        }

    }
}
