using BookWorm.Models.VO;

namespace BookWorm.Models.Interfaces
{
    public interface IAccountDAO
    {
        /// <summary>
        /// Permet de créer un nouveau compte utilisateur
        /// </summary>
        /// <param name="newAccount">Contient les attributs pour le nouveau compte</param>
        /// <returns>Retourne vrai si la création du compte est réussi</returns>
        bool CreateAccount(Account newAccount);

        /// <summary>
        /// Permet à un utilisateur de s'authentifier.
        /// </summary>
        /// <param name="newAccount">Contient les attributs permettant de s'identifier</param>
        /// <returns> Null si l'identification à échoué. </returns>
        Account LoginAccount(Account newAccount);
    }
}
