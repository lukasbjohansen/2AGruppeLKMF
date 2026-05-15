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
        private ITeacherRepository _teacherRepo;
        public SchoolClassRepositoryAsync(IRepositoryAsync<School>schoolRepository,ITeacherRepository teacherRepository)
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

		public async Task DeleteAsync(SchoolClass item)
		{
            string query = "DELETE FROM SchoolClass WHERE SchoolClassID = @SchoolClassID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@SchoolClassID", item.Id);
                    await command.ExecuteNonQueryAsync();

                }
                catch (SqlException sEx)
                {
                    sEx.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Delete, "Ugyldig ID");
                }
                catch (Exception ex)
                {
                    ex.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Delete, ex.GetFullMessage());
                }
            }
        }

		public async Task<List<SchoolClass>> FilterAsync(string filterCriteria)
		{
            string query = @"
                SELECT * FROM SchoolClass
                WHERE SchoolClassID LIKE @filterCriteria
                OR SchoolClassName LIKE @filterCriteria
                OR SchoolClassYear LIKE @filterCriteria
                OR SchoolID LIKE @filterCriteria
                OR TeacherID LIKE @filterCriteria";
            List<SchoolClass> schoolClasses = new List<SchoolClass>();
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
                        int schoolClassId = reader.GetInt32("SchoolClassID");
                        string schoolClassName = reader.GetString("SchoolClassName");   
                        int schoolId = reader.GetInt32("SchoolID");
                        int teacherId = reader.GetInt32("TeacherID");
                        int schoolClassYear = reader.GetInt32("SchoolClassYear");

                        School school = await _schoolRepo.GetAsync(schoolId);
                        Teacher teacher = await _teacherRepo.GetAsync(teacherId);

                        SchoolClass schoolClass = new SchoolClass(schoolClassId, schoolClassName, school, teacher, schoolClassYear);
                        schoolClasses.Add(schoolClass);
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
                return schoolClasses;
            }
        }

        public async Task<SchoolClass?> GetAsync(int id)
        {
            string query = "SELECT * FROM SchoolClass WHERE SchoolClassID = @SchoolClassID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SchoolClassID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    string schoolClassName = reader.GetString("SchoolClassName");
                    int schoolId = reader.GetInt32("SchoolID");
                    int teacherId = reader.GetInt32("TeacherID");
                    int schoolClassYear = reader.GetInt32("SchoolClassYear");

                    
                    School school = await _schoolRepo.GetAsync(schoolId);
                    Teacher teacher = await _teacherRepo.GetAsync(teacherId);

                    return new SchoolClass(id, schoolClassName, school, teacher, schoolClassYear);
                }
                await reader.CloseAsync();
            }
            return null;
        }

        public async Task<List<SchoolClass>> GetAllAsync()
		{
            string query = "SELECT * FROM SchoolClass";
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

		public async Task UpdateAsync(SchoolClass item)
		{
            string query = "UPDATE SchoolClass SET SchoolClassName = @SchoolClassName, SchoolClassYear = @SchoolClassYear, SchoolID = @SchoolID, TeacherID=@TeacherID WHERE SchoolClassID = @SchoolClassID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SchoolClassID", item.Id);
                    command.Parameters.AddWithValue("@SchoolClassName", item.Name);
                    command.Parameters.AddWithValue("@SchoolClassYear", item.Year);
                    command.Parameters.AddWithValue("@SchoolID", item.School.Id);
                    command.Parameters.AddWithValue("@TeacherID", item.Teacher.Id);
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
