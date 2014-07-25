using System.Web.Mvc;

namespace ITSApplication.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        public ActionResult NewsManager()
        {
            return View();
        }

        public ActionResult EventManager()
        {
            return View();
        }
    }
}