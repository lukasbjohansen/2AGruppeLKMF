using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;

namespace RazorPageApplication.Services
{
    public class SchoolRepositoryAsync : ISchoolRepositoryAsync
    {
        public async Task CreateSchoolAsync(School school)
        {
            string query = "INSERT INTO School(SchoolName,SchoolAddress,PostalCode) Values(@SchoolName,@SchoolAddress,@PostalCode)";
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SchoolName", school.Name);
                    command.Parameters.AddWithValue("@SchoolAddress", school.Address);
                    command.Parameters.AddWithValue("@PostalCode", school.PostalCode);
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

        public async Task DeleteSchoolAsync(School school)
        {
            throw new NotImplementedException();
        }

        public async Task<List<School>> FilterSchoolAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public async Task<List<School>> GetAllSchoolsAsync()
        {
            throw new NotImplementedException();
            //string query = "SELECT * FROM School";
            //using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            //{

            //}
        }

        public async Task UpdateSchoolAsync(School school)
        {
            throw new NotImplementedException();
        }
    }
}
