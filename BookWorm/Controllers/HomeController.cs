using BookWorm.Models.Factories;
using BookWorm.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Controllers
{
    public class HomeController : Controller
    {
        private IBookDAO bookDb;

        /// <summary>
        /// Constructeur de base de la classe
        /// </summary>
        public HomeController()
        {
            bookDb = BookDAOFactory.CreateBookDAO(BookDAOFactory.Type.Database);
        }

        /// <summary>
        /// Appel la page d'accueille du site Web.
        /// </summary>
        /// <returns>Index.cshtml</returns>
        [Route("")]
        [Route("Home")]
        public IActionResult Index()
        {
            ViewData["BestSellers"] = bookDb.GetBestSellers();
            ViewData["NewAdditions"] = bookDb.GetNewestBooks(); ;
            return View();
        }
    }
}
