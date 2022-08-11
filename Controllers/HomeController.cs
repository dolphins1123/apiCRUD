using System.Threading.Tasks;
using System.Web.Mvc;

namespace apiCRUD.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {


            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
