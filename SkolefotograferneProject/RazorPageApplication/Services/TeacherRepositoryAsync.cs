using Microsoft.Data.SqlClient;
using RazorPageApplication.Enums;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
    public class TeacherRepositoryAsync : ITeacherRepository
    {
        private readonly LoginUserRepository _userRepo;

        public TeacherRepositoryAsync(LoginUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task CreateAsync(Teacher item)
        {
            string query = @"INSERT INTO Teacher
                            (TeacherID, TeacherName, Username, Password, PhoneNumber) 
                            Values(@TeacherID, @TeacherName, @Mail, @TeacherPassword, @PhoneNumber)";
            int userId = await _userRepo.CreateAsync(item.Username, item.Password);
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TeacherID", userId);
                    command.Parameters.AddWithValue("@TeacherName", item.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber = item.PhoneNumber.TrimPhoneNumber());
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException sEx)
                {
                    sEx.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Create, "Ugyldigt input");
                }
                catch (Exception ex)
                {
                    ex.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Create, ex.GetFullMessage());
                }

            }
        }

        public async Task DeleteAsync(Teacher item)
        {
            string query = "DELETE FROM Teacher WHERE TeacherID = @TeacherID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@TeacherID", item.Id);
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
            await _userRepo.DeleteAsync(item.Id);
        }
        public async Task<List<Teacher>> FilterAsync(string filterCriteria)
        {
            return await FilterAsync(filterCriteria, TeacherFilterBy.All);
        }
        public async Task<List<Teacher>> FilterAsync(string filterCriteria, TeacherFilterBy teacherFilterBy)
        {
            string whereSql;
            switch (teacherFilterBy)
            {
                case TeacherFilterBy.TeacherID:
                    whereSql = "TeacherID LIKE @filterCriteria";
                    break;
                case TeacherFilterBy.TeacherName:
                    whereSql = "TeacherName LIKE @filterCriteria";
                    break;
                case TeacherFilterBy.Mail:
                    whereSql = "Username LIKE @filterCriteria";
                    break;
                case TeacherFilterBy.PhoneNumber:
                    whereSql = "PhoneNumber LIKE @filterCriteria";
                    break;
                default:
                    whereSql = @"TeacherID LIKE @filterCriteria 
                               OR TeacherName LIKE @filterCriteria 
                               OR Username LIKE @filterCriteria 
                               OR PhoneNumber LIKE @filterCriteria";
                    break;
            }
            string query = $"SELECT * FROM Teacher WHERE {whereSql}";
            List<Teacher> teachers = new List<Teacher>();
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
                        int teacherID = reader.GetInt32("TeacherID");
                        string mail = reader.GetString("Username");
                        string teacherPassword = reader.GetString("Password");
                        string teacherName = reader.GetString("TeacherName");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        Teacher teacher = new Teacher(id:teacherID, mail:mail, name:teacherName, password:teacherPassword, phoneNumber:phoneNumber);
                        teachers.Add(teacher);
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
                    ex.PrintWithType();
                    throw new RepositoryException(RepositoryExceptionType.Read, ex.GetFullMessage());
                }
                return teachers;
            }
        }

        public async Task<Teacher?> GetAsync(int id)
        {
            string query =
                                /*"SELECT * FROM Teacher WHERE TeacherID = @TeacherID";*/
                @"SELECT p.TeacherID, p.TeacherName, p.PhoneNumber, lu.Username, lu.Password 
                  FROM Teacher p
                  INNER JOIN LoginUser lu ON p.TeacherID = lu.UserID
                  WHERE p.TeacherID = @TeacherID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TeacherID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Teacher(id:id,
                        name:reader.GetString("TeacherName"),
                        mail:reader.GetString("Username"),
                        password:reader.GetString("Password"),
                        phoneNumber:reader.GetString("PhoneNumber"));
                }
                await reader.CloseAsync();
            }
            return null;
        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            string query = /*"SELECT * FROM Teacher";*/
                                @"SELECT p.TeacherID, p.TeacherName, p.PhoneNumber, lu.Username, lu.Password 
                                  FROM Teacher p
                                  INNER JOIN LoginUser lu ON p.TeacherID = lu.UserID";
            List<Teacher> teachers = new List<Teacher>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int teacherID = reader.GetInt32("TeacherID");
                        string teacherName = reader.GetString("TeacherName");
                        string mail = reader.GetString("Username");
                        string teacherPassword = reader.GetString("Password");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        Teacher teacher = new Teacher(id:teacherID, name:teacherName, mail:mail, password:teacherPassword, phoneNumber:phoneNumber);
                        teachers.Add(teacher);
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
                return teachers;
            }
        }

        public async Task UpdateAsync(Teacher item)
        {
            string query = /*"UPDATE Teacher SET TeacherName = @TeacherName, Username = @Username, Password = @Password, PhoneNumber = @PhoneNumber WHERE TeacherID = @TeacherID";*/
              @"UPDATE Teacher
                SET TeacherName = @TeacherName,
                    PhoneNumber = @PhoneNumber,
                WHERE TeacherID = @TeacherID";
            _userRepo.UpdateAsync(item.Id, item.Username, item.Password);
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TeacherID", item.Id);
                    command.Parameters.AddWithValue("@TeacherName", item.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
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
