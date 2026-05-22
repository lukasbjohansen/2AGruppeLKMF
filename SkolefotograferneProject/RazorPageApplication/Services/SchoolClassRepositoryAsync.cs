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
        #region Instance fields
        /// <summary>
        /// Instance of the School repository. 
        /// Used to retrieve school information when creating or updating a school class, and when retrieving school classes from the database.
        /// </summary>
        private IRepositoryAsync<School> _schoolRepo;
        /// <summary>
        /// Instance of the Teacher repository. 
        /// Used to retrieve teacher information when creating or updating a school class, and when retrieving school classes from the database.
        /// </summary>
        private ITeacherRepository _teacherRepo;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for the SchoolClassRepositoryAsync class.
        /// </summary>
        public SchoolClassRepositoryAsync(IRepositoryAsync<School> schoolRepository, ITeacherRepository teacherRepository)
        {
            _schoolRepo = schoolRepository;
            _teacherRepo = teacherRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new school class. 
        /// The method takes a SchoolClass object as input and inserts its properties into the SchoolClass table in the database. 
        /// It uses parameterized queries to prevent SQL injection attacks. 
        /// If the operation is successful, the new school class is added to the database. 
        /// If there is an error during the operation, a RepositoryException is thrown with an appropriate message. 
        /// </summary>
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
        /// <summary>
        /// Deletes a school class. 
        /// The method takes a SchoolClass object as input and deletes the corresponding record from the SchoolClass table in the database based on the SchoolClassID. 
        /// It uses parameterized queries to prevent SQL injection attacks. 
        /// If the operation is successful, the school class is removed from the database. 
        /// If there is an error during the operation, a RepositoryException is thrown with an appropriate message. 
        /// </summary>
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
        /// <summary>
        /// Filters school classes based on the provided filter criteria. 
        /// The method constructs a SQL query that searches for matches in multiple columns of the SchoolClass table, including SchoolClassID, SchoolClassName, SchoolClassYear, SchoolID, and TeacherID. 
        /// It uses parameterized queries to prevent SQL injection attacks.
        /// If the operation is successful, a list of school classes that match the filter criteria is returned.
        /// If there is an error during the operation, a RepositoryException is thrown with an appropriate message.
        /// </summary>
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
        /// <summary>
        /// Gets a school class by its unique identifier. 
        /// The method takes an integer id as input and retrieves the corresponding record from the SchoolClass table in the database based on the SchoolClassID. 
        /// It uses a parameterized query to prevent SQL injection attacks. 
        /// If the operation is successful, the school class is returned. 
        /// If the school class is not found, null is returned. 
        /// If there is an error during the operation, a RepositoryException is thrown with an appropriate message.
        /// </summary>
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
        /// <summary>
        /// Gets all school classes from the database.
        /// The method executes a SQL query to retrieve all records from the SchoolClass table. 
        /// It uses a SqlDataReader to read the results and constructs a list of SchoolClass objects based on the retrieved data. 
        /// If there is an error during the operation, a RepositoryException is thrown with an appropriate message. 
        /// The method returns a list of all school classes in the database. 
        /// If there are no school classes, an empty list is returned.
        /// </summary>
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


                        SchoolClass schoolClass = new SchoolClass(schoolClassId, schoolClassName, await _schoolRepo.GetAsync(schoolId), await _teacherRepo.GetAsync(teacherId), schoolClassYear);

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
        /// <summary>
        /// Updates an existing school class in the database. 
        /// The method takes a SchoolClass object as input and updates the corresponding record in the SchoolClass table based on the SchoolClassID. 
        /// It uses a parameterized query to prevent SQL injection attacks. 
        /// If the operation is successful, the school class is updated in the database. 
        /// If there is an error during the operation, a RepositoryException is thrown with an appropriate message. 
        /// </summary>
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
        #endregion
    } 
}
