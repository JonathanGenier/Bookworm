using BookWorm.Models.Database;
using BookWorm.Models.Interfaces;
using System;

namespace BookWorm.Models.Factories
{
    public class BookDAOFactory
    {
        public enum Type { Database, /*File, Json*/ };

        /// <summary>
        /// Permet de créer un DAO d'un type demandé.
        /// </summary>
        /// <param name="type">Type de DAO recherché</param>
        /// <returns>Retourne une clase DAO</returns>
        public static IBookDAO CreateBookDAO(Type type)
        {
            switch (type)
            {
                case Type.Database: return new BookDAODB();
                default: 
                    Console.WriteLine("DAO doesn't not exist"); 
                    return null;
            }
        }
    }
}
