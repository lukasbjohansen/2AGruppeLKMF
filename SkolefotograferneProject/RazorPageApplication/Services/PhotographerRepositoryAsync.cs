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
        private readonly LoginUserRepository _userRepo;

        public PhotographerRepositoryAsync(LoginUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task CreateAsync(Photographer photographer)
        {
            string query =
                @"INSERT INTO 
                Photographer(PhotographerID, PhotographerName, PhoneNumber, CVR) 
                Values(@PhotographerID, @PhotographerName, @PhoneNumber, @CVR)";
            try
            {
                int userId = await _userRepo.CreateAsync(photographer.Username, photographer.Password);
                await using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                await using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PhotographerID", userId);
                command.Parameters.AddWithValue("@PhotographerName", photographer.Name);
                command.Parameters.AddWithValue("@PhoneNumber", photographer.PhoneNumber);
                command.Parameters.AddWithValue("@CVR", photographer.CVR);
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
                await _userRepo.DeleteAsync(photographer.Id);

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

        public async Task<List<Photographer>> FilterAsync(string filterCriteria)
        {
            string query =
                @"SELECT p.PhotographerID, p.PhotographerName, p.PhoneNumber, p.CVR, lu.Username, lu.Password 
                  FROM Photographer p
                  INNER JOIN LoginUser lu ON p.PhotographerID = lu.UserID
                  WHERE p.PhotographerName LIKE @filterCriteria
                  OR lu.Username LIKE @filterCriteria
                  OR p.PhoneNumber LIKE @filterCriteria
                  OR p.CVR LIKE @filterCriteria";
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
                        reader.GetString("Username"),
                        reader.GetString("Password"),
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

        public async Task<Photographer> GetAsync(int id)
        {
            string query =
                @"SELECT p.PhotographerID, p.PhotographerName, p.PhoneNumber, p.CVR, lu.Username, lu.Password 
                  FROM Photographer p
                  INNER JOIN LoginUser lu ON p.PhotographerID = lu.UserID
                  WHERE p.PhotographerID = @PhotographerID";
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
                        reader.GetString("Username"),
                        reader.GetString("Password"),
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

        public async Task<List<Photographer>> GetAllAsync()
        {
            string query =
                @"SELECT p.PhotographerID, p.PhotographerName, p.PhoneNumber, p.CVR, lu.Username, lu.Password 
                  FROM Photographer p
                  INNER JOIN LoginUser lu ON p.PhotographerID = lu.UserID";
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
                        reader.GetString("Username"),
                        reader.GetString("Password"),
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

        public async Task UpdateAsync(Photographer photographer)
        {
            string query =
                @"UPDATE Photographer
                SET PhotographerName = @PhotographerName,
                    PhoneNumber = @PhoneNumber,
                    CVR = @CVR
                WHERE PhotographerID = @PhotographerID";
            try
            {
                await _userRepo.UpdateAsync(photographer.Id, photographer.Username, photographer.Password);
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PhotographerID", photographer.Id);
                command.Parameters.AddWithValue("@PhotographerName", photographer.Name);
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
    }
}
