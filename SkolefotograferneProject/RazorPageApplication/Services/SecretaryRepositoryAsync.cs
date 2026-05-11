using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class SecretaryRepositoryAsync : IRepositoryAsync<Secretary>
    {
        public SecretaryRepositoryAsync()
        {
            
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

        public Task DeleteAsync(Secretary item) //DEN HAR EN FK SÅ KAN IKKE SLETTE EN SECRETARY
        {
            throw new NotImplementedException();
        }

        public Task<List<Secretary>> FilterAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<Secretary> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Secretary>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Secretary item)
        {
            throw new NotImplementedException();
        }
    }
}
