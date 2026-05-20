using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
    public class ParentRepositoryAsync : IRepositoryAsync<Parent>
    {
        private readonly LoginUserRepository _userRepo;

        public ParentRepositoryAsync(LoginUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task CreateAsync(Parent item)
        {
            string query = @"INSERT INTO Parent(ParentID, ParentName,PhoneNumber,ParentAddress,PostalCode)
                             Values(@ParentID, @ParentName,@PhoneNumber,@ParentAddress,@PostalCode)";
            int userId = await _userRepo.CreateAsync(item.Username, item.Password);
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {

                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ParentID", userId);
                    command.Parameters.AddWithValue("@ParentName", item.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    command.Parameters.AddWithValue("@ParentAddress", item.Address);
                    command.Parameters.AddWithValue("@PostalCode", item.PostalCode);
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException sEx)
                {
                    sEx.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Create, "Ugyldigt input");
                }
                catch (Exception ex)
                {
                    //ExceptionHelpers.PrintWithType(ex);
                    ex.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Create, ex.GetFullMessage());
                }

            }
        }

        public async Task DeleteAsync(Parent item)
        {
            string query = "DELETE FROM Parent WHERE ParentID = @ParentID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@ParentID", item.Id);
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException sEx)
                {
                    sEx.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Delete, "Ugyldig ID");
                }
                catch (Exception ex)
                {
                    //ExceptionHelpers.PrintWithType(ex);
                    ex.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Delete, ex.GetFullMessage());
                }
                await _userRepo.DeleteAsync(item.Id);
            }
        }

        public async Task<List<Parent>> FilterAsync(string filterCriteria)
        {
            string query = /*@"
                SELECT * FROM Parent 
                WHERE ParentID LIKE @filterCriteria 
                OR ParentName LIKE @filterCriteria 
                OR Username LIKE @filterCriteria
                OR PhoneNumber LIKE @filterCriteria
                OR ParentAddress LIKE @filterCriteria
                OR Password LIKE @filterCriteria
                OR PostalCode LIKE @filterCriteria";*/
            @"SELECT p.ParentID, p.ParentName, p.PhoneNumber, p.ParentAddress, p.PostalCode, lu.Username, lu.Password 
                  FROM Parent p
                  INNER JOIN LoginUser lu ON p.ParentID = lu.UserID
                  WHERE p.ParentName LIKE @filterCriteria
                  OR lu.Username LIKE @filterCriteria
                  OR p.PhoneNumber LIKE @filterCriteria
                  OR p.ParentAddress LIKE @filterCriteria
                  OR p.PostalCode LIKE @filterCriteria";
            List<Parent> parents = new List<Parent>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@filterCriteria", $"%{filterCriteria}%");
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int parentId = reader.GetInt32("ParentID");
                        string parentName = reader.GetString("ParentName");
                        string mail = reader.GetString("Username");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        string ParentAddress = reader.GetString("ParentAddress");
                        string ParentPassword = reader.GetString("Password");
                        string postalCode = reader.GetString("PostalCode");
                        Parent parent = new Parent(parentId, mail, ParentPassword, parentName, phoneNumber, ParentAddress, postalCode);
                        parents.Add(parent);
                    }
                    reader.CloseAsync();
                }
                catch (SqlException sEx)
                {
                    sEx.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Read, "Kan ikke indlæses");
                }
                catch (Exception ex)
                {
                    //ExceptionHelpers.PrintWithType(ex);
                    ex.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Read, ex.GetFullMessage());
                }
                return parents;
            }
        }

        public async Task<Parent> GetAsync(int id)
        {
            string query =
                /*"SELECT * FROM Parent WHERE ParentID = @ParentID"*/
                @"SELECT p.ParentID, p.ParentName, p.PhoneNumber, p.ParentAddress, p.PostalCode, lu.Username, lu.Password 
                  FROM Parent p
                  INNER JOIN LoginUser lu ON p.ParentID = lu.UserID
                  WHERE p.ParentID = @ParentID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParentID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Parent(id,
                        reader.GetString("Username"),
                        reader.GetString("Password"),
                        reader.GetString("ParentName"),
                        reader.GetString("PhoneNumber"),
                        reader.GetString("ParentAddress"),
                        reader.GetString("PostalCode"));
                }
                await reader.CloseAsync();
            }
            return null;
        }

        public async Task<List<Parent>> GetAllAsync()
        {
            string query =
                /*"SELECT * FROM Parent"*/
                @"SELECT p.ParentID, p.ParentName, p.PhoneNumber, p.ParentAddress, p.PostalCode, lu.Username, lu.Password 
                  FROM Parent p
                  INNER JOIN LoginUser lu ON p.ParentID = lu.UserID"; ;
            List<Parent> parents = new List<Parent>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int parentId = reader.GetInt32("ParentID");
                        string parentName = reader.GetString("ParentName");
                        string mail = reader.GetString("Username");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        string parentAddress = reader.GetString("ParentAddress");
                        string parentPassword = reader.GetString("Password");
                        string postalCode = reader.GetString("PostalCode");
                        Parent parent = new Parent(parentId, mail, parentPassword, parentName, phoneNumber, parentAddress, postalCode);
                        parents.Add(parent);
                    }
                    reader.CloseAsync();
                }
                catch (SqlException sEx)
                {
                    sEx.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Read, "Kan ikke indlæses");
                }
                catch (Exception ex)
                {
                    //ExceptionHelpers.PrintWithType(ex);
                    ex.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Read, ex.GetFullMessage());
                }
                return parents;
            }
        }

        public async Task UpdateAsync(Parent item)
        {
            string query = "UPDATE Parent SET ParentName = @ParentName, PhoneNumber = @PhoneNumber, ParentAddress = @ParentAddress, PostalCode = @PostalCode WHERE ParentID = @ParentID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await _userRepo.UpdateAsync(item.Id, item.Username, item.Password);
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ParentID", item.Id);
                    command.Parameters.AddWithValue("@ParentName", item.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    command.Parameters.AddWithValue("@ParentAddress", item.Address);
                    command.Parameters.AddWithValue("@PostalCode", item.PostalCode);
                    await command.ExecuteNonQueryAsync();

                }
                catch (SqlException e)
                {
                    e.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Update, "Ugyldigt input");
                }
            }
        }
    }
}
