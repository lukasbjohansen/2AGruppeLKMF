using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
    public class StudentRepositoryAsync : IRepositoryAsync<Student>
    {
        private IRepositoryAsync<SchoolClass> _schoolClassRepo;
        private IRepositoryAsync<Parent> _parentRepo;
        public StudentRepositoryAsync(IRepositoryAsync<SchoolClass> schoolClassRepo, IRepositoryAsync<Parent> parentRepo)
        {
            _schoolClassRepo = schoolClassRepo;
            _parentRepo = parentRepo;
        }

        public async Task CreateAsync(Student user)
        {
            string query = "INSERT INTO Student(StudentName,PhotoCode,SchoolClassID) Values(@StudentName,@PhotoCode,@SchoolClassID)";
            //string query2 = "INSERT INTO ParentStudent(StudentID,ParentID) Values(@StudentID,@ParentID)";
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@StudentName", user.Name);
                    command.Parameters.AddWithValue("@PhotoCode", user.PhotoCode);
                    command.Parameters.AddWithValue("@SchoolClassID", user.SchoolClass.Id);
                    //command.Parameters.AddWithValue("@ParentID", user.Parents);

                    //foreach (Parent p in user.Parents)
                    //{
                    //    SqlCommand command2 = new SqlCommand(query2, connection);
                    //    command.Parameters.AddWithValue("@StudentID", user.Id);
                    //    command.Parameters.AddWithValue("@ParentID", p.Id);
                    //    await command2.ExecuteNonQueryAsync();
                    //}

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

        public async Task DeleteAsync(Student user)
        {
            string query = "DELETE FROM Student WHERE StudentID = @StudentID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@StudentID", user.Id);
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

        public async Task<List<Student>> FilterAsync(string filterCriteria)
        {
            string query = @"
                SELECT * FROM Student
                WHERE StudentID LIKE @filterCriteria
                OR StudentName LIKE @filterCriteria
                OR PhotoCode LIKE @filterCriteria
                OR SchoolClassID LIKE @filterCriteria
                OR ParentID LIKE @filterCriteria";
            List<Student> students = new List<Student>();
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
                        int studentId = reader.GetInt32("StudentID");
                        string studentName = reader.GetString("StudentName");
                        string photoCode = reader.GetString("PhotoCode");
                        int schoolClassId = reader.GetInt32("SchoolClassID");
                        int parentId = reader.GetInt32("ParentID");

                        SchoolClass schoolClass = await _schoolClassRepo.GetAsync(schoolClassId);
                        List<Parent> parent = await _parentRepo.GetAllAsync();

                        Student student = new Student(studentId, studentName, schoolClass, parent, photoCode);
                        students.Add(student);
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
                return students;
            }
        }

        public async Task<Student?> GetAsync(int id)
        {
            string query = "SELECT * FROM Student WHERE StudentID = @StudentID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    string studentName = reader.GetString("StudentName");
                    string photoCode = reader.GetString("PhotoCode");
                    int schoolClassId = reader.GetInt32("SchoolClassID");
                    int parentId = reader.GetInt32("ParentID");

                    List<Parent> parent = await _parentRepo.GetAllAsync();
                    SchoolClass schoolClass = await _schoolClassRepo.GetAsync(schoolClassId);

                    return new Student(id, studentName, schoolClass, parent, photoCode);
                }
                await reader.CloseAsync();
            }
            return null;
        }

        public async Task<List<Student>> GetAllAsync()
        
        {
            string query = "SELECT * FROM Student";
            List<Student> students = new List<Student>();

            using ( SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        int studentId = reader.GetInt32("StudentID");
                        string studentName = reader.GetString("StudentName");
                        string photoCode = reader.GetString("PhotoCode");
                        int schoolClassId = reader.GetOrdinal("SchoolClassID");
                        int parentId = reader.GetOrdinal("ParentID");

                        await reader.CloseAsync();
                        SchoolClass schoolClass = await _schoolClassRepo.GetAsync(schoolClassId);
                        List<Parent> parents = await _parentRepo.GetAllAsync();

                        Student student = new Student(studentId, studentName, schoolClass, parents, photoCode);
                        students.Add(student);
                    }

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
                return students;
            }
        }

        public async Task UpdateAsync(Student user)
        {
            string query = "UPDATE Student SET StudentName = @StudentName, PhotoCode = @PhotoCode, SchoolClassID = @SchoolClassID, ParentID=@ParentID WHERE StudentID = @StudentID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@StudentID", user.Id);
                    command.Parameters.AddWithValue("@StudentName", user.Name);
                    command.Parameters.AddWithValue("@PhotoCode", user.PhotoCode);
                    command.Parameters.AddWithValue("@SchoolClassID", user.SchoolClass.Id);
                    command.Parameters.AddWithValue("@ParentID", user.Parents.Select(p => p.Id).FirstOrDefault());


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
