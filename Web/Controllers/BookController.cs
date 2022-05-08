using Core.ViewModels.Book;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BookController : Controller
    {
        public BookController()
        {

        }
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookModel model, IFormFile cover)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            return RedirectToAction("All");
        }
    }
}
