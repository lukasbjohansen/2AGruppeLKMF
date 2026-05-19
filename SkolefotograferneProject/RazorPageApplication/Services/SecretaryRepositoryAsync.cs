using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
    public class SecretaryRepositoryAsync : IRepositoryAsync<Secretary>
    {
        private IRepositoryAsync<School> _schoolRepo;
        public SecretaryRepositoryAsync(IRepositoryAsync<School> schoolRepo)
        {
            _schoolRepo = schoolRepo;
        }

        //Create, GetAll, Get, Delete, Update, Filter
        public async Task CreateAsync(Secretary item)
        {
            string query = "INSERT INTO Secretary(SecretaryName, PhoneNumber, Username, Password, SchoolID) Values (@SecretaryName, @PhoneNumber, @Username, @Password, @SchoolID)";
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SecretaryName", item.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    command.Parameters.AddWithValue("@Username", item.Username);
                    command.Parameters.AddWithValue("@Password", item.Password);
                    command.Parameters.AddWithValue("@SchoolID", item.School.Id);
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

        public async Task DeleteAsync(Secretary item) //DEN HAR EN FK SÅ KAN IKKE SLETTE EN SECRETARY
        {
            string query = "DELETE FROM Secretary WHERE SecretaryID = @SecretaryID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@SecretaryID", item.Id);
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
            }
        }

        public async Task<List<Secretary>> FilterAsync(string filterCriteria)
        {
            string query = @"
                SELECT * FROM Secretary 
                WHERE SecretaryID LIKE @filterCriteria 
                OR SecretaryName LIKE @filterCriteria 
                OR PhoneNumber LIKE @filterCriteria
                OR Username LIKE @filterCriteria
                OR Password LIKE @filterCriteria
                OR SchoolID LIKE @filterCriteria";
            List<Secretary> Secretaries = new List<Secretary>();
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
                        int secretaryID = reader.GetInt32("SecretaryID");
                        string secretaryName = reader.GetString("SecretaryName");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        string mail = reader.GetString("Username");
                        string secretaryPassword = reader.GetString("Password");
                        School schoolID = await _schoolRepo.GetAsync(reader.GetInt32("SchoolID"));
                        Secretary secretary = new Secretary(secretaryID, mail, secretaryPassword, secretaryName, phoneNumber, schoolID);
                        Secretaries.Add(secretary);
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
                return Secretaries;
            }
        }

        public async Task<Secretary?> GetAsync(int id)
        {
            string query = "SELECT * FROM Secretary WHERE SecretaryID = @SecretaryID";
            
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SecretaryID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Secretary(id,
                        reader.GetString("Username"),
                        reader.GetString("Password"),
                        reader.GetString("SecretaryName"),
                        reader.GetString("PhoneNumber"),
                        await _schoolRepo.GetAsync(reader.GetInt32("SchoolID")));
                }
                await reader.CloseAsync();
            }
            return null;
        }

        public async Task<List<Secretary>> GetAllAsync()
        {
            string query = "SELECT * FROM Secretary";
            List<Secretary> secretaries = new List<Secretary>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int secretaryID = reader.GetInt32("SecretaryID");
                        string secretaryName = reader.GetString("SecretaryName");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        string mail = reader.GetString("Username");
                        string secretaryPassword = reader.GetString("Password");
                        int schoolID = reader.GetInt32("SchoolID");

                        Secretary secretary = new Secretary(secretaryID, mail, secretaryPassword, secretaryName, phoneNumber, await _schoolRepo.GetAsync(schoolID));
                        secretaries.Add(secretary);
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
                return secretaries;

            }
        }

        public async Task UpdateAsync(Secretary item)
        {
            string query = "UPDATE Secretary SET SecretaryName = @SecretaryName, PhoneNumber = @PhoneNumber, Username = @Username, Password = @Password, SchoolID = @SchoolID WHERE SecretaryID = @SecretaryID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SecretaryID", item.Id);
                    command.Parameters.AddWithValue("@SecretaryName", item.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    command.Parameters.AddWithValue("@Username", item.Username);
                    command.Parameters.AddWithValue("@Password", item.Password);
                    command.Parameters.AddWithValue("@SchoolID", item.School.Id);
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
