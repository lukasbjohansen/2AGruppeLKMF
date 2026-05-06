using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
    public class TeacherRepositoryAsync : IRepositoryAsync<Teacher>
    {
        public async Task CreateAsync(Teacher item)
        {
            string query = @"INSERT INTO Teacher
                            (TeacherID, TeacherName, Mail, TeacherPassword, PhoneNumber) 
                            Values(@TeacherID, @TeacherName, @Mail, @TeacherPassword, @PhoneNumber)";
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TeacherID", item.Id);
                    command.Parameters.AddWithValue("@TeacherName", item.Name);
                    command.Parameters.AddWithValue("@Mail", item.Mail);
                    command.Parameters.AddWithValue("@TeacherPassword", item.Password);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
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
        }

        public async Task<List<Teacher>> FilterAsync(string filterCriteria)
        {
            string query = @"
                SELECT * FROM Teacher 
                WHERE TeacherID LIKE @filterCriteria 
                OR TeacherName LIKE @filterCriteria 
                OR Mail LIKE @filterCriteria
                OR PhoneNumber LIKE @filterCriteria";
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
                        string teacherName = reader.GetString("TeacherName");
                        string mail = reader.GetString("Mail");
                        string teacherPassword = reader.GetString("TeacherPassword");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        Teacher teacher = new Teacher(teacherID, teacherName, mail, teacherPassword, phoneNumber);
                        teachers.Add(teacher);
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
                return teachers;
            }
        }

        public async Task<Teacher?> GetAsync(int id)
        {
            string query = "SELECT * FROM Teacher WHERE TeacherID = @TeacherID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TeacherID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Teacher(id,
                        reader.GetString("TeacherName"),
                        reader.GetString("Mail"),
                        reader.GetString("TeacherPassword"),
                        reader.GetString("PhoneNumber"));
                }
                await reader.CloseAsync();
            }
            return null;
        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            string query = "SELECT * FROM Teacher";
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
                        string mail = reader.GetString("Mail");
                        string teacherPassword = reader.GetString("TeacherPassword");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        Teacher teacher = new Teacher(teacherID, teacherName, mail, teacherPassword, phoneNumber);
                        teachers.Add(teacher);
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
                return teachers;
            }
        }

        public async Task UpdateAsync(Teacher item)
        {
            string query = "UPDATE Teacher SET TeacherName = @TeacherName, Mail = @Mail, TeacherPassword = @TeacherPassword, PhoneNumber = @PhoneNumber WHERE TeacherID = @TeacherID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TeacherID", item.Id);
                    command.Parameters.AddWithValue("@TeacherName", item.Name);
                    command.Parameters.AddWithValue("@Mail", item.Mail);
                    command.Parameters.AddWithValue("@TeacherPassword", item.Password);
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
