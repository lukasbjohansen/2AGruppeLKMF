using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
    public class SchoolRepositoryAsync : IRepositoryAsync<School>
    {
        public async Task CreateAsync(School school)
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

        public async Task DeleteAsync(School school)
        {
            string query = "DELETE FROM School WHERE SchoolID = @SchoolID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@SchoolID", school.Id);
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

        public async Task<List<School>> FilterAsync(string filterCriteria)
        {
            string query = @"
                SELECT * FROM School 
                WHERE SchoolID LIKE @filterCriteria 
                OR SchoolName LIKE @filterCriteria 
                OR SchoolAddress LIKE @filterCriteria
                OR PostalCode LIKE @filterCriteria";
            List<School> schools = new List<School>();
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

        public async Task<School?> GetAsync(int id)
        {
            string query = "SELECT * FROM School WHERE SchoolID = @SchoolID";
            using(SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SchoolID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if(await reader.ReadAsync())
                {
                    return new School(id,
                        reader.GetString("SchoolName"),
                        reader.GetString("SchoolAddress"),
                        reader.GetString("PostalCode"));
                }
                await reader.CloseAsync();
            }
            return null;
        }

        public async Task<List<School>> GetAllAsync()
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

        public async Task UpdateAsync(School school)
        {
            string query = "UPDATE School SET SchoolName = @SchoolName, SchoolAddress = @SchoolAddress, PostalCode = @PostalCode WHERE SchoolID = @SchoolID";
            using(SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SchoolID", school.Id);
                    command.Parameters.AddWithValue("@SchoolName", school.Name);
                    command.Parameters.AddWithValue("@SchoolAddress", school.Address);
                    command.Parameters.AddWithValue("@PostalCode", school.PostalCode);
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
