using System;
using System.Collections.Generic;
using System.Globalization;
using BookWorm.Models.VO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BookWorm
{
    public class Startup
    {
        // Langue par défaut
        private readonly RequestCulture defaultCulture = new RequestCulture("en");

        // Langues supportées
        private readonly List<CultureInfo> cultures = new List<CultureInfo>
        {
            new CultureInfo("en"),
            new CultureInfo("fr")
        };

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Ajoute la localisation aux services et indique le dossier ou se trouves les fichiers de traduction.
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Configure les langues supporté
            services.Configure<RequestLocalizationOptions>(config =>
            {
                config.DefaultRequestCulture = defaultCulture;                 // Language par défaut.
                config.SupportedCultures = cultures;                           // Formats
                config.SupportedUICultures = cultures;                         // Strings des langues.
            });

            // Configure l'authentification des utilisateurs
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(config =>
            {
                // Nom du cookie
                config.Cookie.Name = "AuthenticationCookie";

                // Si l'utilisateur n'est pas authentifier, l'utilisateur sera re-diriger vers la page
                // login lorsqu'il essaie d'accèder une page qui nécessite un authentification.
                config.LoginPath = "/Account/Login";

                // Le cookie peut etre utiliser seulement par le serveur 
                // Ne peut pas etre accéder via JavaScript 
                config.Cookie.HttpOnly = true;

                // Le cookie peut etre utiliser sur ce site Web.
                config.Cookie.SameSite = SameSiteMode.Strict;

                // Le cookie sera existant pour 60 minutes.
                config.ExpireTimeSpan = TimeSpan.FromMinutes(60);   
                
            });

            // Lets the.NET framework and the web server know how to process incoming requests etc.
            services.AddMvc(config =>
            {
                config.EnableEndpointRouting = false;
            }) 
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Styles, Images, Javascript, etc.
            app.UseStaticFiles();

            app.UseAuthentication();    // Identifier l'utilisateur
            app.UseAuthorization();     // Autoriser l'utilisateur
            app.UseCookiePolicy();      // 
            // Ajoute toutes les langues supportées au pipeline.
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            // Routing
            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
