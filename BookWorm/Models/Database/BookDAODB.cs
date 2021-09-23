using BookWorm.Models.Interfaces;
using BookWorm.Models.VO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BookWorm.Models.Database
{
    public class BookDAODB : IBookDAO
    {
        // String de connexion pour la base de donnée.
        private readonly string _connectionString = "Data Source=(Localdb)\\LocalDB;Initial Catalog=WormBookDB;Integrated Security=True";

        /// <summary>
        /// Cette fonction permet de recueillir les details d'un livre auprès de la base de donnée.
        /// </summary>
        /// <param name="bookId">Id du livre que l'on cherches les détails</param>
        /// <returns>Retourne null si le livre n'est pas existant</returns>
        public Book GetBookDetails(string bookId)
        {
            Book book = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procGetBookDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", bookId);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                book = new Book();
                                while (reader.Read())
                                {
                                    book.Id = (int)reader["id"];
                                    book.Title = reader["title"].ToString();
                                    book.Description = reader["description"].ToString();
                                    book.ImagePath = reader["image_path"].ToString();
                                    book.PublishedDate = reader["published_date"].ToString();
                                    book.Price = (double)reader["price"];
                                    book.NumberOfPages = (int)reader["number_of_pages"];
                                    book.StockQuantity = (int)reader["stock_quantity"];
                                    book.SoldQuantity = (int)reader["sold_Quantity"];
                                    book.Author = reader["author_first_name"].ToString() + " ";
                                    book.Author += reader["author_last_name"].ToString();
                                    book.Publisher += reader["publisher"].ToString();
                                    book.Genre += reader["genre"].ToString();
                                }

                                reader.Close();
                            }
                        }

                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }

                connection.Close();
            }

            return book;
        }

        /// <summary>
        /// Cette fonction permet de recueillir tous les livres de la base de donnée
        /// </summary>
        /// <returns>Retourne une liste de tous les livres</returns>
        public List<Book> GetBooks()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procGetBooks", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Book book = new Book();

                                    book.Id = (int)reader["id"];
                                    book.Title = reader["title"].ToString();
                                    book.Description = reader["description"].ToString();
                                    book.ImagePath = reader["image_path"].ToString();
                                    book.PublishedDate = reader["published_date"].ToString();
                                    book.Price = (double)reader["price"];
                                    book.NumberOfPages = (int)reader["number_of_pages"];
                                    book.StockQuantity = (int)reader["stock_quantity"];
                                    book.SoldQuantity = (int)reader["sold_Quantity"];
                                    book.Author = reader["author_first_name"].ToString() + " ";
                                    book.Author += reader["author_last_name"].ToString();
                                    book.Publisher += reader["publisher"].ToString();
                                    book.Genre += reader["genre"].ToString();

                                    books.Add(book);
                                }

                                reader.Close();
                            }
                        }
                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }

                connection.Close();
            }

            return books;
        }

        /// <summary>
        /// Cette fonction permet d'inséré dans le chariot d'achat le livre choisis.
        /// </summary>
        /// <param name="bookId">Id du livre</param>
        /// <param name="accountId">Id du compte de l'utilisateur</param>
        /// <param name="quantity">Quantite à insérer</param>
        /// <param name="wish">À implenter (WISHLIST)</param>
        /// <returns>Vrai si l'insertion à réussi</returns>
        public bool AddToCart(int bookId, int accountId, int quantity, bool wish)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procAddToCart", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@bookId", bookId);
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@wish", (wish ? 1 : 0));

                    try
                    {
                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            connection.Close();
                            return true;
                        }

                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }

                connection.Close();
            }

            return false;
        }

        /// <summary>
        /// Cette fonction permet de receuillir tous les livre dans le chariot
        /// d'achat d'un utilisateur.
        /// </summary>
        /// <param name="accountId">Id du compte utilisateur</param>
        /// <returns> Retourne null si aucune livre est dans le chariot.</returns>
        public List<Book> GetCart(int accountId)
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procGetCart", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@accountId", accountId);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Book book = new Book();

                                    book.Id = (int)reader["id"];
                                    book.Title = reader["title"].ToString();
                                    book.Description = reader["description"].ToString();
                                    book.ImagePath = reader["image_path"].ToString();
                                    book.PublishedDate = reader["published_date"].ToString();
                                    book.Price = (double)reader["price"];
                                    book.NumberOfPages = (int)reader["number_of_pages"];
                                    book.StockQuantity = (int)reader["stock_quantity"];
                                    book.SoldQuantity = (int)reader["sold_Quantity"];
                                    book.Author = reader["author_first_name"].ToString() + " ";
                                    book.Author += reader["author_last_name"].ToString();
                                    book.Publisher += reader["publisher"].ToString();
                                    book.Genre += reader["genre"].ToString();

                                    books.Add(book);
                                }

                                reader.Close();
                            }
                        }

                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }

                connection.Close();
            }

            return books;
        }

        /// <summary>
        /// Cette fonction permet de receuillir les 5 livres les plus récent.
        /// </summary>
        /// <returns> Retourne liste de livre </returns>
        public List<Book> GetNewestBooks()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procGetNewestBooks", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Book book = new Book();

                                    book.Id = (int)reader["id"];
                                    book.Title = reader["title"].ToString();
                                    book.Description = reader["description"].ToString();
                                    book.ImagePath = reader["image_path"].ToString();
                                    book.PublishedDate = reader["published_date"].ToString();
                                    book.Price = (double)reader["price"];
                                    book.NumberOfPages = (int)reader["number_of_pages"];
                                    book.StockQuantity = (int)reader["stock_quantity"];
                                    book.SoldQuantity = (int)reader["sold_Quantity"];
                                    book.Author = reader["author_first_name"].ToString() + " ";
                                    book.Author += reader["author_last_name"].ToString();
                                    book.Publisher += reader["publisher"].ToString();
                                    book.Genre += reader["genre"].ToString();

                                    books.Add(book);
                                }

                                reader.Close();
                            }
                        }
                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }

                connection.Close();
            }

            return books;
        }

        /// <summary>
        /// Cette fonction permet d'aller chercher les 5 livre les plus vendus.
        /// </summary>
        /// <returns> Retourne une liste de livre. </returns>
        public List<Book> GetBestSellers()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procGetBestSellers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Book book = new Book();

                                    book.Id = (int)reader["id"];
                                    book.Title = reader["title"].ToString();
                                    book.Description = reader["description"].ToString();
                                    book.ImagePath = reader["image_path"].ToString();
                                    book.PublishedDate = reader["published_date"].ToString();
                                    book.Price = (double)reader["price"];
                                    book.NumberOfPages = (int)reader["number_of_pages"];
                                    book.StockQuantity = (int)reader["stock_quantity"];
                                    book.SoldQuantity = (int)reader["sold_Quantity"];
                                    book.Author = reader["author_first_name"].ToString() + " ";
                                    book.Author += reader["author_last_name"].ToString();
                                    book.Publisher += reader["publisher"].ToString();
                                    book.Genre += reader["genre"].ToString();

                                    books.Add(book);
                                }

                                reader.Close();
                            }
                        }
                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }

                connection.Close();
            }

            return books;
        }

        /// <summary>
        /// Cette fonctions permet d'aller chercher tous les genres stocké dans la base de donnée.
        /// </summary>
        /// <returns>Retourne une liste de genre.</returns>
        public List<Genre> GetGenres()
        {
            List<Genre> genres = new List<Genre>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procGetGenres", connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    genres.Add(new Genre(reader["genre"].ToString()));
                                }

                                reader.Close();
                            }
                        }
                    }
                    catch (SqlException exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }
                }

                connection.Close();
            }

            return genres;
        }
    }
}
