using BookWorm.Models.Interfaces;
using BookWorm.Models.VO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BookWorm.Models.Database
{
    public class AddressDAODB : IAddressDAO
    {
        // String de connexion pour la base de donnée.
        private readonly string _connectionString = "Data Source=(Localdb)\\LocalDB;Initial Catalog=WormBookDB;Integrated Security=True";

        /// <summary>
        /// /// Cette fonction créer une addresse dans la base de donnée.
        /// </summary>
        /// <param name="newAddress">Contient les attributs pour créer la nouvelle adresse</param>
        /// <param name="accountId">Id du compte de l'utilisateur</param>
        /// <returns></returns>
        public bool CreateAddress(Address newAddress, int accountId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procCreateAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@countryName", newAddress.CountryName);
                    command.Parameters.AddWithValue("@regionName", newAddress.RegionName);
                    command.Parameters.AddWithValue("@cityName", newAddress.City);
                    command.Parameters.AddWithValue("@localAddress", newAddress.LocalAddress);
                    command.Parameters.AddWithValue("@postalCode", newAddress.PostalCode);
                    command.Parameters.AddWithValue("@apartment", newAddress.Apartment);
                    command.Parameters.AddWithValue("@accountId", accountId);
                    
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
        /// Cette fonction permet de recueillir une adresse auprès de la base de donnée.
        /// </summary>
        /// <param name="addressId">Id de l'adresse recherché.</param>
        /// <returns>Retourne null, si aucune adresse est trouvé</returns>
        public Address GetAddress(int addressId)
        {
            Address address = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procGetAddress", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@addressId", addressId);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    address = new Address();
                                    Region region = new Region();
                                    Country country = new Country();

                                    address.Id = (int)reader["id"];
                                    address.LocalAddress = reader["address"].ToString();
                                    address.PostalCode = reader["postal_code"].ToString();
                                    address.Apartment = (int)reader["apartment"];
                                    address.City = reader["city"].ToString();
                                    region.Id = (int)reader["region_Id"];
                                    region.Name = reader["region"].ToString();
                                    region.Tax = (double)reader["tax"];
                                    country.Id = (int)reader["country_id"];
                                    country.Name = reader["country"].ToString();

                                    address.Region = region;
                                    address.Country = country;
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

            return address;
        }

        /// <summary>
        /// Cette fonction permet de recueuillir toutes les addresse d'un compte
        /// utilisateur auprès de la base de donnée.
        /// </summary>
        /// <param name="accountId">Id du compte utilisateur</param>
        /// <returns>Retourne null si aucune adresse existe</returns>
        public List<Address> GetAddresses(int accountId)
        {
            List<Address> addresses = new List<Address>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procGetAddresses", connection))
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
                                    Address address = new Address();
                                    Region region = new Region();
                                    Country country = new Country();

                                    address.Id = (int)reader["id"];
                                    address.LocalAddress = reader["address"].ToString();
                                    address.PostalCode = reader["postal_code"].ToString();
                                    address.Apartment = (int)reader["apartment"];
                                    address.City = reader["city"].ToString();
                                    region.Id = (int)reader["region_Id"];
                                    region.Name = reader["region"].ToString();
                                    region.Tax = (double)reader["tax"];
                                    country.Id = (int)reader["country_id"];
                                    country.Name = reader["country"].ToString();

                                    address.Region = region;
                                    address.Country = country;

                                    addresses.Add(address);
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

            return addresses;
        }

        /// <summary>
        /// Cette fonction permet de recueillir tous les pays stocké dans la base de donnée.
        /// </summary>
        /// <returns> Retourne null si aucun pays est existant. </returns>
        public List<Country> GetCountries()
        {
            List<Country> countries = new List<Country>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procGetCountries", connection))
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
                                    Country country = new Country();
                                    country.Id = (int)reader["id"];
                                    country.Name = reader["country"].ToString();
                                    countries.Add(country);
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

            return countries;
        }

        /// <summary>
        /// Cette fonction permet de recueillir tous les régions (province, états) d'un pays.
        /// </summary>
        /// <param name="countryId">Id du pays.</param>
        /// <returns> Null si aucune région est existante </returns>
        public List<Region> GetRegions(int countryId)
        {
            List<Region> regions = new List<Region>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("procGetRegions", connection))
                {
                    command.Parameters.AddWithValue("@countryId", countryId);
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
                                    Region region = new Region();
                                    region.Id = (int)reader["id"];
                                    region.Name = reader["region"].ToString();
                                    region.Tax = (double)reader["tax"];
                                    regions.Add(region);
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

            return regions;
        }
    }
}
