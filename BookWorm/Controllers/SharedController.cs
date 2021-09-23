using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace BookWorm.Controllers
{
    public class SharedController : Controller
    {
        // Utiliser pour le language toggler
        private readonly IHtmlLocalizer<HomeController> _localizer;

        /// <summary>
        /// Constructeur de base de la classe
        /// </summary>
        /// <param name="localizer"></param>
        public SharedController(IHtmlLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Appelé lorsqu'on change de langue 
        /// </summary>
        /// <param name="culture">Langue choisie</param>
        /// <param name="returnURL">URL de la page actuel de l'utilisateur</param>
        /// <returns>Retourne la page actuelle de l'utilisateur avec la langue choisie.</returns>
        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnURL)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) }
                );

            return LocalRedirect(returnURL);
        }
    }
}
