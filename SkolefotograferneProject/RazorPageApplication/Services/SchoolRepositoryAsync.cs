using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

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
            string query = "SELECT * FROM School";
            List<School> schools = new List<School>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int schoolId = reader.GetInt32("SchoolID");
                        string schoolName = reader.GetString("SchoolName");
                        string schoolAddress = reader.GetString("SchoolAddress");
                        string postalCode = reader.GetString("PostalCode");
                        School school = new School(schoolId, schoolName, schoolAddress, postalCode);
                        schools.Add(school);
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
                return schools;
            }
        }

        public async Task UpdateSchoolAsync(School school)
        {
            throw new NotImplementedException();
        }
    }
}
