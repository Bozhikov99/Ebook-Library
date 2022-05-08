using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult All()
        {

            return View();
        }
    }
}
