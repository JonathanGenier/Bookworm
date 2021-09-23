using BookWorm.Models.VO;
using System.Collections.Generic;

namespace BookWorm.Models.Interfaces
{
    public interface IAddressDAO
    {
        /// <summary>
        /// Permet de créer une nouvelle addresse.
        /// </summary>
        /// <param name="newAddress">Contient les attributs de la nouvelles adresse</param>
        /// <param name="accountId">Id du compte afin d'établir une relation.</param>
        /// <returns> Retourne vrai si la création à réussi. </returns>
        bool CreateAddress(Address newAddress, int accountId);

        /// <summary>
        /// Permet d'aller receuillir une adresse précise.
        /// </summary>
        /// <param name="addressId">Id de l'adresse recherchée</param>
        /// <returns> Retourne null si aucune adresse est trouvé </returns>
        Address GetAddress(int addressId);

        /// <summary>
        /// Permet de receuillir tous les adresse d'un utilisateur.
        /// </summary>
        /// <param name="accountId">Id du compte utilisateur</param>
        /// <returns> Null si aucune adresse est trouvé </returns>
        List<Address> GetAddresses(int accountId);

        /// <summary>
        /// Permet de receuillir tous les pays stocké.
        /// </summary>
        /// <returns> Retourne null si aucune pays est trouvé </returns>
        List<Country> GetCountries();

        /// <summary>
        /// Permet de receuillir tous les régions (province, état) d'un pays.
        /// </summary>
        /// <param name="countryId">Id du pays</param>
        /// <returns>Retourne null si aucune région est trouvé</returns>
        List<Region> GetRegions(int countryId);
    }
}
