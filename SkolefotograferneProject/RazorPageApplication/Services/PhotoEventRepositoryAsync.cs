using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class PhotoEventRepositoryAsync : IRepositoryAsync<PhotoEvent>
    {
        public async Task CreateAsync(PhotoEvent photoEvent)
        {
            throw new NotImplementedException();
            //string query = "INSERT INTO School(SchoolName, SchoolAddress, PostalCode) Values(@SchoolName, @SchoolAddress, @PostalCode)";
            //try
            //{
            //    await using SqlConnection connection = new SqlConnection(Secret.ConnectionString);
            //    await connection.OpenAsync();
            //    await using SqlCommand command = new SqlCommand(query, connection);
            //    command.Parameters.AddWithValue("@StartDate", photoEvent.StartTime);
            //    command.Parameters.AddWithValue("@EndDate", photoEvent.EndTime);
            //    command.Parameters.AddWithValue("@PostalCode", photoEvent.Location);
            //    await command.ExecuteNonQueryAsync();
            //}
            //catch (SqlException sEx)
            //{
            //    sEx.PrintWithType();
            //    throw new RepositoryException(RepositoryExceptionType.Create, "Ugyldigt input");
            //}
            //catch (Exception ex)
            //{
            //    ex.PrintWithType();
            //    throw new RepositoryException(RepositoryExceptionType.Create, ex.GetFullMessage());
            //}
        }

        public async Task DeleteAsync(PhotoEvent item)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PhotoEvent>> FilterAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public async Task<PhotoEvent> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PhotoEvent>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(PhotoEvent item)
        {
            throw new NotImplementedException();
        }
    }
}
