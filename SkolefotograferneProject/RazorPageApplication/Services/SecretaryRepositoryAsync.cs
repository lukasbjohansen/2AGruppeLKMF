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
            string query = "INSERT INTO Secretary(SecretaryName, PhoneNumber, Mail, SecretaryPassword, SchoolID) Values (@SecretaryName, @PhoneNumber, @Mail, @SecretaryPassword, @SchoolID)";
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SecretaryName", item.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    command.Parameters.AddWithValue("@Mail", item.Mail);
                    command.Parameters.AddWithValue("@SecretaryPassword", item.Password);
                    command.Parameters.AddWithValue("@SchoolID", item.School); //kan vel kun være School
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
                OR Mail LIKE @filterCriteria
                OR SecretaryPassword LIKE @filterCriteria
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
                        string mail = reader.GetString("Mail");
                        string secretaryPassword = reader.GetString("SecretaryPassword");
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

        public async Task<Secretary> GetAsync(int id)
        {
            string query = "SELECT * FROM Secretary WHERE SecretaryID = @SecretaryID";
            School? school = await _schoolRepo.GetAsync(id); //hvorfor gør man det her igen?
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SecretaryID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Secretary(id,
                        reader.GetString("SecretaryName"),
                        reader.GetString("PhoneNumber"),
                        reader.GetString("Mail"),
                        reader.GetString("SecretaryPassword"),
                        school);
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
                        string mail = reader.GetString("Mail");
                        string secretaryPassword = reader.GetString("SecretaryPassword");
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
            string query = "UPDATE Secretary SET SecretaryID = @SecretaryID, SecretaryName = @SecretaryName, PhoneNumber = @PhoneNumber, Mail = @Mail, SecretaryPassword = @SecretaryPassword, SchoolID = @SchoolID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SecretaryID", item.Id);
                    command.Parameters.AddWithValue("@SecretaryName", item.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    command.Parameters.AddWithValue("@Mail", item.Mail);
                    command.Parameters.AddWithValue("@SecretaryPassword", item.Password);
                    command.Parameters.AddWithValue("@SchoolID", item.School);
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
