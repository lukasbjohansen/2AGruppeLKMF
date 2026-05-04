using Microsoft.Data.SqlClient;
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
                catch (Exception ex)
                {
                    //ExceptionHelpers.PrintWithType(ex);
                    ex.PrintWithType();
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
        }

        public async Task UpdateSchoolAsync(School school)
        {
            throw new NotImplementedException();
        }
    }
}
