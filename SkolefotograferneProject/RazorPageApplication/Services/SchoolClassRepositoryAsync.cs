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
        private IRepositoryAsync<School> _schoolRepo;
        private IRepositoryAsync<Teacher> _teacherRepo;
        public SchoolClassRepositoryAsync(IRepositoryAsync<School>schoolRepository,IRepositoryAsync<Teacher>teacherRepository)
        {
            _schoolRepo = schoolRepository;
            _teacherRepo = teacherRepository;
        }
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
                    command.Parameters.AddWithValue("@SchoolID", item.School.Id);
                    command.Parameters.AddWithValue("@TeacherID", item.Teacher.Id);
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
                        int schoolId = reader.GetInt32("SchoolID");
                        int teacherId = reader.GetInt32("TeacherID");
                        int schoolClassYear = reader.GetInt32("SchoolClassYear");


                        SchoolClass schoolClass = new SchoolClass(schoolClassId, schoolClassName, await _schoolRepo.GetAsync(schoolId),await _teacherRepo.GetAsync(teacherId), schoolClassYear);

                        schoolClasses.Add(schoolClass);
                        
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
