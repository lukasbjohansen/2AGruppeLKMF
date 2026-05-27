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
        #region Instance fields
        /// <summary>
        /// _schoolClassRepo instance field that represents the repository for the SchoolClass model.
        /// </summary>
        private IRepositoryAsync<SchoolClass> _schoolClassRepo;
        /// <summary>
        /// _parentRepo instance field that represents the repository for the Parent model.
        /// </summary>
        private IRepositoryAsync<Parent> _parentRepo;
        #endregion

        #region Constructors
        /// <summary>
        ///StudentRepositoryAsync constructor that is used to create a new instance of the StudentRepositoryAsync class with the repositories for the SchoolClass and Parent models injected as parameters.
        /// </summary>

        public StudentRepositoryAsync(IRepositoryAsync<SchoolClass> schoolClassRepo, IRepositoryAsync<Parent> parentRepo)
        {
            _schoolClassRepo = schoolClassRepo;
            _parentRepo = parentRepo;
        }
        #endregion

        #region Methods
        /// <summary>
        ///CreateAsync method that is used to create a new student in the system.
        /// </summary>
        public async Task CreateAsync(Student user)
        {

            string query = "INSERT INTO Student(StudentName,PhotoCode,SchoolClassID,ParentID) Values(@StudentName,@PhotoCode,@SchoolClassID,@ParentID)";

            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@StudentName", user.Name);
                    command.Parameters.AddWithValue("@PhotoCode", user.PhotoCode);
                    command.Parameters.AddWithValue("@SchoolClassID", user.SchoolClass.Id);
                    command.Parameters.AddWithValue("@ParentID", user.Parent.Id);

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
        /// <summary>
        ///DeleteAsync method that is used to delete an existing student from the system.
        /// </summary>
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
        /// <summary>
        /// FilterAsync method that is used to filter students based on a given criteria.
        /// </summary>
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
                        Parent parent = await _parentRepo.GetAsync(parentId);

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
        /// <summary>
        ///GetAsync method that is used to retrieve a specific student from the system based on their ID.
        /// </summary>
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

                    Parent parent = await _parentRepo.GetAsync(parentId);
                    SchoolClass schoolClass = await _schoolClassRepo.GetAsync(schoolClassId);

                    return new Student(id, studentName, schoolClass, parent, photoCode);
                }
                await reader.CloseAsync();
            }
            return null;
        }
        /// <summary>
        /// GetAllAsync method that is used to retrieve all students from the system.
        /// </summary>
        public async Task<List<Student>> GetAllAsync()

        {
            string query = "SELECT * FROM Student";
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
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
                        int schoolClassId = reader.GetInt32("SchoolClassID");
                        int parentId = reader.GetInt32("ParentID");

                        SchoolClass schoolClass = await _schoolClassRepo.GetAsync(schoolClassId);
                        Parent parent = await _parentRepo.GetAsync(parentId);

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
        /// <summary>
        /// UpdateAsync method that is used to update an existing student's information in the system.
        /// </summary>
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
                    command.Parameters.AddWithValue("@SchoolClassID", user.SchoolClass!.Id);
                    command.Parameters.AddWithValue("@ParentID", user.Parent!.Id);


                    await command.ExecuteNonQueryAsync();

                }
                catch (SqlException e)
                {
                    e.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Update, "Ugyldigt input");
                }
            }
        }
        #endregion
    } 
}
