using System;
using System.Collections.Generic;
using BookWorm.Models.Factories;
using BookWorm.Models.Interfaces;
using BookWorm.Models.VO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookWorm.Controllers
{
    public class ShopController : Controller
    {
        // Contient la classe qui appelle la base de donnée pour les livres.
        private IBookDAO bookDB;

        // Contient la classe qui appelle la base de donnée pour les addresses
        private IAddressDAO addressDB;

        /// <summary>
        /// Constructeur de base de la classe.
        /// </summary>
        public ShopController()
        {
            bookDB = BookDAOFactory.CreateBookDAO(BookDAOFactory.Type.Database);
            addressDB = AddressDAOFactory.CreateAddressDAO(AddressDAOFactory.Type.Database);
        }

        /// <summary>
        /// Page d'achat de livre
        /// </summary>
        /// <returns>Shop.cshtml</returns>
        [HttpGet]
        [Route("Shop")]
        public IActionResult Shop()
        {
            // Appele la base de donnée pour aller receuillir
            // tous les livres et les genres.
            ViewData["Books"] = bookDB.GetBooks();
            ViewData["Genres"] = bookDB.GetGenres();

            return View();
        }

        /// <summary>
        /// Page d'abbonement (NON IMPLÉMENTÉ)
        /// </summary>
        /// <returns>Subscribe.cshtml</returns>
        [Route("Shop/Subscribe")]
        public IActionResult Subscribe()
        {
            // À IMPLÉMENTER

            return View();
        }

        /// <summary>
        /// Chariot d'achat de l'utilisateur
        /// </summary>
        /// <returns>Cart.cshtml</returns>
        [Authorize]
        [Route("Shop/Cart")]
        public IActionResult Cart()
        {
            int accountId = int.Parse(HttpContext.User.FindFirst(x => x.Type == "Id").Value);

            // Appele la base de donnée pour aller recueillir tous les livres
            // dans la chariot d'achat de l'utilisateur.
            List<Book> cart = bookDB.GetCart(accountId);

            // Calcul du total $ avant les taxes.
            double subtotal = 0.0;
            foreach (Book book in cart)
            {
                subtotal += book.Price;
            }

            // Si il n'a pas de livre dans le chario d'achat, on
            // empêche l'utilisateur d'accéder au checkout page.
            if (cart.Count == 0)
                ViewBag.DisableCheckout = true;
            else
                ViewBag.DisableCheckout = false;

            // Donne à la vue, les livres et le sous-total.
            ViewBag.Cart = cart;
            ViewBag.Subtotal = Math.Round((Double)subtotal, 2);

            return View();
        }

        /// <summary>
        /// Page wishlist (PAS IMPLÉMENTER)
        /// </summary>
        /// <returns>Wishlist.cshtml</returns>
        [Authorize]
        [Route("Shop/Wishlist")]
        public IActionResult Wishlist()
        {
            // À IMPLÉMENTER

            return View();
        }

        /// <summary>
        /// Cette page contient tous les détails du livre.
        /// </summary>
        /// <param name="id">Id du livre</param>
        /// <returns>ProductDetail.cshtml</returns>
        [Route("Shop/Book-Detail/{id?}")]
        public IActionResult ProductDetail(string id = "")
        {
            if (!id.Trim().Equals(""))
            {
                // Appele la base de donnée pour aller 
                // chercher les détails du livre.
                ViewBag.Book = bookDB.GetBookDetails(id);
            }

            return View();
        }

        /// <summary>
        /// Cette page contient tous les détails concernant la préparation de la commande.
        /// Elle s'occupe de calculer les montants à payer.
        /// </summary>
        /// <returns>Checkout.cshtml</returns>
        [Authorize]
        [Route("Shop/Checkout/{id?}")]
        public IActionResult Checkout()
        {
            // Id du compte de l'utilisateur
            int accountId = int.Parse(HttpContext.User.FindFirst(x => x.Type == "Id").Value);

            // Appele la base de donnée pour recueillir le panier d'achat de l'utilisateur
            ViewBag.Cart = bookDB.GetCart(accountId);

            // Appele la base de donnée pour recueillir les adresses de l'utilisateur
            List<Address> addresses = addressDB.GetAddresses(accountId);

            // Calcul des montants à payer.
            // Shipping & handling sont hardcoded *À IMPLÉMENTER*
            double subtotal = 0.0;
            double shipping = 0.0;
            double handling = 0.0;

            foreach (Book book in ViewBag.Cart as List<Book>)
            {
                subtotal += book.Price;
                shipping += 3.47;       
                handling += 1.02;
            }

            // S'il n'y a pas d'addresse, nous ne pouvons
            // pas connaitre la taxe de la région.
            if (addresses.Count == 0)
            {
                ViewBag.LoadedId = -1;
                ViewBag.LoadedTax = -1;
            }
            else
            {
                ViewBag.LoadedId = addresses[0].Id;
                ViewBag.LoadedTax = addresses[0].Region.Tax;
            }

            // On donne les attributs précédent à la vue.
            ViewBag.Addresses = addresses;
            ViewBag.Subtotal = Math.Round(subtotal, 2);
            ViewBag.Shipping = Math.Round(shipping, 2);
            ViewBag.Handling = Math.Round(handling, 2);
            ViewBag.ShippingAndHandling = Math.Round((shipping + handling), 2);

            return View();
        }

        /// <summary>
        /// Appelé par une fonction AJAX.
        /// Ajoute le livre choisis au chariot d'achat.
        /// </summary>
        /// <param name="bookId">ID du livre choisis</param>
        [HttpGet]
        public void AddToCart(int bookId)
        {
            var accountId = HttpContext.User.FindFirst(x => x.Type == "Id")?.Value;
            
            // L'utilisateur doit être authentifier afin de 
            // pouvoir insérer un livre dans un chariot d'achat.
            if (accountId != null)
                bookDB.AddToCart(bookId, int.Parse(accountId), 1, false);
        }

        /// <summary>
        /// Appelé par une fonction AJAX.
        /// </summary>
        /// <param name="addressId">Id de l'addresse</param>
        /// <returns> Retourne une addresse formatté en JSON </returns>
        [HttpGet]
        public JsonResult GetAddress(int addressId)
        {
            Address address = addressDB.GetAddress(addressId);
            string jsonString = JsonConvert.SerializeObject(address);
            return Json(jsonString);
        }
    }
}
