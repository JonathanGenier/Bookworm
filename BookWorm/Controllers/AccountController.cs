using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BookWorm.Models.Factories;
using BookWorm.Models.Interfaces;
using BookWorm.Models.Security;
using BookWorm.Models.VO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Controllers
{
    public class AccountController : Controller
    {
        // Contient la classe qui appelle la base de donnée pour les addresses
        private IAddressDAO addressDB;

        // Contient la classe qui appelle la base de donnée pour les comptes.
        private IAccountDAO accountDB;

        // Liste de tous les pays de la base de donnée.
        private List<Country> countries;

        /// <summary>
        /// Constructeur de base de la classe
        /// </summary>
        public AccountController()
        {
            addressDB = AddressDAOFactory.CreateAddressDAO(AddressDAOFactory.Type.Database);
            accountDB = AccountDAOFactory.CreateAccountDAO(AccountDAOFactory.Type.Database);
            countries = addressDB.GetCountries();   
        }

        /// <summary>
        /// Page UserAccount
        /// </summary>
        /// <returns>UserAccount.cshtml</returns>
        [Authorize]
        [HttpGet]
        [Route("Account")]
        public IActionResult UserAccount()
        {
            // On va cherche l'utilisateur connecter par le cookie
            Account account = new Account
                (
                    int.Parse(HttpContext.User.FindFirst(x => x.Type == "Id").Value),
                    HttpContext.User.FindFirst(x => x.Type == "FirstName").Value,
                    HttpContext.User.FindFirst(x => x.Type == "LastName").Value,
                    HttpContext.User.FindFirst(x => x.Type == "Email").Value,
                    null,
                    null,
                    bool.Parse(HttpContext.User.FindFirst(x => x.Type == "IsAdmin").Value)
                );

            // On donne au view le compte d'utilisateur et ses addresses.
            ViewBag.Account = account;
            ViewBag.Addresses = addressDB.GetAddresses(account.Id);
            ViewBag.Countries = countries;
            ViewBag.Regions = addressDB.GetRegions(ViewBag.Countries[0].Id);
            
            return View();
        }

        /// <summary>
        /// Appelé lorsque l'utilisateur créer un nouvelle adresse.
        /// </summary>
        /// <param name="address">Nouvelle adresse de l'utilisateur</param>
        /// <returns>UserAccount.cshtml</returns>
        [Authorize]
        [HttpPost]
        [Route("Account")]
        public IActionResult UserAccount(Address address)
        {
            // On va cherche l'utilisateur connecter par le cookie
            Account account = new Account
                (
                    int.Parse(HttpContext.User.FindFirst(x => x.Type == "Id").Value),
                    HttpContext.User.FindFirst(x => x.Type == "FirstName").Value,
                    HttpContext.User.FindFirst(x => x.Type == "LastName").Value,
                    HttpContext.User.FindFirst(x => x.Type == "Email").Value,
                    null,
                    null,
                    bool.Parse(HttpContext.User.FindFirst(x => x.Type == "IsAdmin").Value)
                );

            // On appel la base de donnée pour créer la nouvelle adresse.
            addressDB.CreateAddress(address, account.Id);

            // On donne au view le compte d'utilisateur et ses addresses.
            ViewBag.Account = account;
            ViewBag.Addresses = addressDB.GetAddresses(account.Id);
            ViewBag.Countries = countries;
            ViewBag.Regions = addressDB.GetRegions(ViewBag.Countries[0].Id);

            return View();
        }

        /// <summary>
        /// Page Login
        /// </summary>
        /// <returns>Login.cshtml</returns>
        [HttpGet]
        [Route("Account/Login")]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Appelé lorsque l'utilisateur tente de log in dans son compte.
        /// </summary>
        /// <param name="loginAttempt">Username et password de l'utilisateur.</param>
        /// <returns>Si login valide, on retourne Index.cshtml sinon on retourne Login.cshtml</returns>
        [HttpPost]
        [Route("Account/Login")]
        public IActionResult Login(Account loginAttempt)
        {
            // Décryption du mot de passe.
            Console.WriteLine("Mot de passe encrypter : " + loginAttempt.Password);
            loginAttempt.Password = AesCryptography.DecryptAES(loginAttempt.Password, AesCryptography.Source.Web);
            Console.WriteLine("Mot de passe decrypter : " + loginAttempt.Password);

            // On appel la base de donnée pour aller valider le username et le password.
            Account validatedAccount = accountDB.LoginAccount(loginAttempt);

            // Si le login n'est pas valide (mauvais username ou mauvais mot de passe),
            // validatedAccount retourne null sinon, validatedAccount contiendra les 
            // informations du compte de l'utilisateur.
            if (validatedAccount != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", validatedAccount.Id.ToString()),
                    new Claim("FirstName", validatedAccount.FirstName),
                    new Claim("LastName", validatedAccount.LastName),
                    new Claim("Email", validatedAccount.Email),
                    new Claim("IsAdmin", validatedAccount.IsAdmin.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Page Register
        /// </summary>
        /// <returns>Register.cshtml</returns>
        [HttpGet]
        [Route("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Appelé lorsque l'utilisateur créer un nouveau compte.
        /// </summary>
        /// <param name="newAccount">Contient les informations du nouveau compte d'utilisateur</param>
        /// <returns>Login.cshtml si le compte est créer sinon on retourne Register.cshtml</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/Register")]
        public IActionResult Register(Account newAccount)
        {
            // Décryption du mot de passe.
            newAccount.Password = AesCryptography.DecryptAES(newAccount.Password, AesCryptography.Source.Web);

            // Retourne vrai si le compte est créer
            // Faux si le compte n'a pas pu être créer
            // (EX: Nom utilisateur déjà existant)
            if (accountDB.CreateAccount(newAccount))
            {
                return View("Login");
            } else
            {
                return View("Register");
            }
        }

        /// <summary>
        /// Appelé lorsqu'on clique sur le bouton Logout dans la page UserAccount.cshtml
        /// </summary>
        /// <returns>Index.cshtml</returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Appelé par une fonction AJAX.
        /// Cette fonction va cherché les régions (province/etats)
        /// d'un pays donné en paramètre.
        /// </summary>
        /// <param name="countryName">Nom du pays qu'on cherche avoir ses régions.</param>
        /// <returns>Objet JSON contenant les noms des régions du pays.</returns>
        [HttpGet]
        public JsonResult GetRegions(string countryName)
        {
            List<string> regions = null;
            int countryId = -1;

            foreach (Country country in countries)
            {
                if (countryName.Equals(country.Name))
                    countryId = country.Id;
            }

            // Si countryId = -1, le pays n'est pas valide.
            if (countryId != -1)
            {
                regions = new List<string>();

                // Appele la base de donnée pour avoir les régions(province/états) du pays.
                foreach (Region region in addressDB.GetRegions(countryId))
                {
                    string regionString = string.Format("<option value={0}>{1}</option>", region.Id, region.Name);
                    regions.Add(regionString);
                }
            }

            return Json(regions);
        }
    }
}
