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
        /// Repository for managing SchoolClass entities in the database.
        /// Used to retrieve SchoolClass information when creating or updating Student records, as well as when filtering or retrieving Student records that are associated with a SchoolClass.
        /// </summary>
        private IRepositoryAsync<SchoolClass> _schoolClassRepo;
        /// <summary>
        /// Repository for managing Parent entities in the database.
        /// Used to retrieve Parent information when creating or updating Student records, as well as when filtering or retrieving Student records that are associated with a Parent.
        /// </summary>
        private IRepositoryAsync<Parent> _parentRepo;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for the StudentRepositoryAsync class. 
        /// Used to initialize the repository with the necessary dependencies, specifically the SchoolClass and Parent repositories.
        /// </summary>

        public StudentRepositoryAsync(IRepositoryAsync<SchoolClass> schoolClassRepo, IRepositoryAsync<Parent> parentRepo)
        {
            _schoolClassRepo = schoolClassRepo;
            _parentRepo = parentRepo;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new student in the system. 
        /// The method uses ADO.NET and Async to execute an INSERT statement against the database.
        /// It takes a Student object as a parameter and inserts the corresponding record into the database.
        /// If the operation is successful, the new student will be added to the system; otherwise, an exception will be thrown.
        /// If the input data is invalid, a RepositoryException with the type Create will be thrown, indicating that the creation process failed due to invalid input.
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
        /// Deletes a student from the system. 
        /// The method uses ADO.NET and Async to execute a DELETE statement against the database, using the student's ID as a parameter. 
        /// It takes a Student object as a parameter and deletes the corresponding record from the database based on the student's ID. 
        /// If the operation is successful, the student will be removed from the system; otherwise, an exception will be thrown. 
        /// If the provided ID is invalid, a RepositoryException with the type Delete will be thrown, indicating that the deletion process failed due to an invalid ID.If any other error occurs during the deletion process, a RepositoryException with the type Delete will be thrown, containing the full message of the exception that occurred.
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
        /// Filters students based on a given criteria.
        /// The method uses ADO.NET and Async to execute a SELECT statement against the database, using the provided filter criteria to search for matching records.
        /// It takes a string parameter representing the filter criteria and returns a list of Student objects that match the criteria.
        /// If the operation is successful, a list of matching students will be returned; otherwise, an exception will be thrown.
        /// If any error occurs during the filtering process, a RepositoryException with the type Read will be thrown, containing the full message of the exception that occurred.
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
        /// Gets a student by their ID.
        /// The method uses ADO.NET and Async to execute a SELECT statement against the database, using the provided ID to search for a matching record.
        /// It takes an integer parameter representing the student's ID and returns a Student object that matches the ID.
        /// If the operation is successful and a matching student is found, the corresponding Student object will be returned; otherwise, null will be returned.
        /// If any error occurs during the retrieval process, a RepositoryException with the type Read will be thrown, containing the full message of the exception that occurred.
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
        /// Gets all students from the system.
        /// The method uses ADO.NET and Async to execute a SELECT statement against the database to retrieve all student records.
        /// It returns a list of Student objects representing all the students in the system.
        /// If the operation is successful, a list of all students will be returned; otherwise, an exception will be thrown.
        /// If any error occurs during the retrieval process, a RepositoryException with the type Read will be thrown, containing the full message of the exception that occurred.
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
        /// Updates an existing student in the system.
        /// The method uses ADO.NET and Async to execute an UPDATE statement against the database, using the student's ID to identify the record to be updated.
        /// It takes a Student object as a parameter and updates the corresponding record in the database based on the student's ID.
        /// If the operation is successful, the student's information will be updated in the system; otherwise, an exception will be thrown.
        /// If the input data is invalid, a RepositoryException with the type Update will be thrown, indicating that the update process failed due to invalid input.
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
