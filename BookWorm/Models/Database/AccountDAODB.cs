using BookWorm.Models.Interfaces;
using BookWorm.Models.VO;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BookWorm.Models.Database
{
    public class AccountDAODB : IAccountDAO
    {
        // String de connexion pour la base de donnée.
        private readonly string _connectionString = "Data Source=(Localdb)\\LocalDB;Initial Catalog=WormBookDB;Integrated Security=True";

        /// <summary>
        /// Créer un nouveau compte avec les attributs passé en paramètres.
        /// </summary>
        /// <param name="newAccount">Contient les attributs du nouveau compte</param>
        /// <returns>Retourne vrai si la création à réussi</returns>
        public bool CreateAccount(Account newAccount)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procCreateAccount", connection))
                {
                    command.Parameters.AddWithValue("@firstName", newAccount.FirstName);
                    command.Parameters.AddWithValue("@lastName", newAccount.LastName);
                    command.Parameters.AddWithValue("@email", newAccount.Email);
                    command.Parameters.AddWithValue("@username", newAccount.Username);
                    command.Parameters.AddWithValue("@password", newAccount.Password);
                    command.Parameters.AddWithValue("@isAdmin", (newAccount.IsAdmin ? 1 : 0));

                    command.CommandType = CommandType.StoredProcedure;
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
        /// Cette fonction permet à un utilisateur de s'identifier auprès de la base de donnée.
        /// </summary>
        /// <param name="account">Contient les attributs nécessaire pour s'identifier</param>
        /// <returns>Retourn null si l'identification n'a pas réussi.</returns>
        public Account LoginAccount(Account account)
        {
            Account loggedIn = null;
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procLoginAccount", connection))
                {
                    command.Parameters.AddWithValue("@username", account.Username);
                    command.Parameters.AddWithValue("@password", account.Password);

                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                loggedIn = new Account();
                                while (reader.Read())
                                {
                                    loggedIn.Id = (int)reader["id"];
                                    loggedIn.FirstName = reader["first_name"].ToString();
                                    loggedIn.LastName = reader["last_name"].ToString();
                                    loggedIn.Email = reader["email"].ToString();
                                    loggedIn.IsAdmin = reader.GetBoolean(6);
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

            return loggedIn;
        }
    }
}
