using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
    public class PhotographerRepositoryAsync : IRepositoryAsync<Photographer>
    {
        #region Methods
        /// <summary>
        /// Handles creating/adding a photographer to the database asynchronously.
        /// Takes a photographer reference as an argument.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
        public async Task CreateAsync(Photographer photographer)
        {
            string query =
                @"INSERT INTO 
                Photographer(PhotographerName, Mail, PhoneNumber, CVR, PhotographerPassword) 
                Values(@PhotographerName, @Mail, @PhoneNumber, @CVR, @PhotographerPassword)";
            try
            {
                await using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                await using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PhotographerName", photographer.Name);
                command.Parameters.AddWithValue("@Mail", photographer.Mail);
                command.Parameters.AddWithValue("@PhoneNumber", photographer.PhoneNumber);
                command.Parameters.AddWithValue("@CVR", photographer.CVR);
                command.Parameters.AddWithValue("@PhotographerPassword", photographer.Password);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Create, "Ugyldigt input");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Create, e.GetFullMessage());
            }
        }

        /// <summary>
        /// Handles deleting an existing Photographer from the database asynchronously.
        /// Takes a photographer reference as an argument.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
        public async Task DeleteAsync(Photographer photographer)
        {
            string query =
                @"DELETE FROM Photographer
                WHERE PhotographerID = @PhotographerID";
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PhotographerID", photographer.Id);
                await command.ExecuteNonQueryAsync();

            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Delete, "Ugyldig ID");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Delete, e.GetFullMessage());
            }
        }

        /// <summary>
        /// Handles filtering photographers from the database asynchronously.
        /// Takes the string filterCriteria as an argument, and returns a List matching the criteria.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
        public async Task<List<Photographer>> FilterAsync(string filterCriteria)
        {
            string query =
                @"SELECT * FROM Photographer
                WHERE PhotographerName LIKE @filterCriteria
                OR Mail LIKE @filterCriteria
                OR PhotographerPassword LIKE @filterCriteria
                OR PhoneNumber LIKE @filterCriteria
                OR CVR LIKE @filterCriteria";
            List<Photographer> photographers = new();
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@filterCriteria", $"%{filterCriteria}%");
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    photographers.Add(new(
                        reader.GetInt32("PhotographerID"),
                        reader.GetString("PhotographerName"),
                        reader.GetString("Mail"),
                        reader.GetString("PhotographerPassword"),
                        reader.GetString("PhoneNumber"),
                        reader.GetString("CVR")
                    ));
                }
                return photographers;
            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Read, "Kan ikke indlæses");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Read, e.GetFullMessage());
            }
        }

        /// <summary>
        /// Handles retrieving and returning a Photographer from the database asynchronously.
        /// Takes an int, id, as an argument, and returns a Photographer matching that id,
        /// if it exists, otherwise null is returned.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
        public async Task<Photographer> GetAsync(int id)
        {
            string query =
                @"SELECT * FROM Photographer 
                WHERE PhotographerID = @PhotographerID";
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PhotographerID", id);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new(
                        id,
                        reader.GetString("PhotographerName"),
                        reader.GetString("Mail"),
                        reader.GetString("PhotographerPassword"),
                        reader.GetString("PhoneNumber"),
                        reader.GetString("CVR")
                    );
                }
            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Read, "Indlæsningsfejl");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Delete, e.GetFullMessage());
            }
            return null;
        }

        /// <summary>
        /// Handles retrieving all Photographers from the database asynchronously and returns them as a List.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
        public async Task<List<Photographer>> GetAllAsync()
        {
            string query = "SELECT * FROM Photographer";
            List<Photographer> photographers = new();
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    photographers.Add(new(
                        reader.GetInt32("PhotographerID"),
                        reader.GetString("PhotographerName"),
                        reader.GetString("Mail"),
                        reader.GetString("PhotographerPassword"),
                        reader.GetString("PhoneNumber"),
                        reader.GetString("CVR")
                    ));
                }
                return photographers;
            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Read, "Kan ikke indlæses");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Read, e.GetFullMessage());
            }
        }

        /// <summary>
        /// Handles updating all Photographers from the database asynchronously.
        /// Takes a photographer reference, which we want to update, as an argument.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
        public async Task UpdateAsync(Photographer photographer)
        {
            string query =
                @"UPDATE Photographer
                SET PhotographerName = @PhotographerName,
                    Mail = @Mail,
                    PhotographerPassword = @PhotographerPassword,
                    PhoneNumber = @PhoneNumber,
                    CVR = @CVR
                WHERE PhotographerID = @PhotographerID";
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PhotographerID", photographer.Id);
                command.Parameters.AddWithValue("@PhotographerName", photographer.Name);
                command.Parameters.AddWithValue("@Mail", photographer.Mail);
                command.Parameters.AddWithValue("@PhotographerPassword", photographer.Password);
                command.Parameters.AddWithValue("@PhoneNumber", photographer.PhoneNumber);
                command.Parameters.AddWithValue("@CVR", photographer.CVR);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Update, "Ugyldigt input");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Read, e.GetFullMessage());
            }
        }
        #endregion
    }
}
