using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
	public class SchoolClassRepositoryAsync : IRepositoryAsync<SchoolClass>
	{
		public async Task CreateAsync(SchoolClass item)
		{
            string query = "INSERT INTO SchoolClass(SchoolClassName,SchoolClassYear,SchoolID,TeacherID) Values(@SchoolClassName,@SchoolClassYear,@SchoolID,@TeacherID)";
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SchoolClassName", item.Name);
                    command.Parameters.AddWithValue("@SchoolClassYear", item.Year);
                    command.Parameters.AddWithValue("@SchoolID", item.School);
                    command.Parameters.AddWithValue("@TeacherID", item.Teacher);
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

		public Task DeleteAsync(SchoolClass item)
		{
			throw new NotImplementedException();
		}

		public Task<List<SchoolClass>> FilterAsync(string filterCriteria)
		{
			throw new NotImplementedException();
		}

        public Task<SchoolClass> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SchoolClass>> GetAllAsync()
		{
            const string query = "SELECT * FROM SchoolClass";
            List<SchoolClass> schoolClasses = new List<SchoolClass>();

            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        int schoolClassId = reader.GetInt32("SchoolClassID");
                        string schoolClassName = reader.GetString("SchoolClassName");
                        string schoolClassYear = reader.GetString("SchoolClassYear");
                        int schoolId = reader.GetInt32("SchoolID");
                        int teacherId = reader.GetInt32("TeacherID");
                        
                        
                        SchoolClass schoolClass= new SchoolClass(schoolId,schoolClassName,)
                        
                        
                        
                    }
                    await reader.CloseAsync();
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
                return schoolClasses;
            }
        }

		public Task UpdateAsync(SchoolClass item)
		{
			throw new NotImplementedException();
		}
	}
}
