using BookWorm.Models.VO;
using System.Collections.Generic;

namespace BookWorm.Models.Interfaces
{
    public interface IBookDAO
    {
        /// <summary>
        /// Ajoute un produit au chariot d'achat de l'utilisateur.
        /// </summary>
        /// <param name="bookId">Id du livre</param>
        /// <param name="accountId">Id du compte utilisateur</param>
        /// <param name="quantity">Quantité à insérer</param>
        /// <param name="wish">À implenter (wishlist) </param>
        /// <returns></returns>
        bool AddToCart(int bookId, int accountId, int quantity, bool wish);

        /// <summary>
        /// Permet de receuillir les détails d'un livre précis.
        /// </summary>
        /// <param name="id">Id du livre recherché</param>
        /// <returns> retourne null si le livre n'est pas trouvé </returns>
        Book GetBookDetails(string id);

        /// <summary>
        /// Permet de receuillir tous les livres de la base de donnée
        /// </summary>
        /// <returns>Retourne null si aucune livre est trouvé</returns>
        List<Book> GetBooks();

        /// <summary>
        /// Permet de receuillir tous les livres dans un chariot d'achat
        /// d'un utilisateur.
        /// </summary>
        /// <param name="accountId">Id du compte utilisateur</param>
        /// <returns> Retourne null si aucun livre est trouvé </returns>
        List<Book> GetCart(int accountId);

        /// <summary>
        /// Permet de receuillir 5 livres les plus récent
        /// </summary>
        /// <returns> Retourne null si aucune livre est trouvé </returns>
        List<Book> GetNewestBooks();

        /// <summary>
        /// Permet de receuillir 5 livres les plus vendus.
        /// </summary>
        /// <returns> Retourne null si aucune livre est trouvé </returns>
        List<Book> GetBestSellers();

        /// <summary>
        /// Permet de receuillir tous les genres stocké.
        /// </summary>
        /// <returns>Retourne null si aucun genre est trouvé </returns>
        List<Genre> GetGenres();
    }
}
