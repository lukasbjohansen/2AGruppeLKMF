using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
	public class ParentRepositoryAsync : IRepositoryAsync<Parent>
	{
		public async Task CreateAsync(Parent item)
		{
            string query = "INSERT INTO Parent(ParentName,Mail,PhoneNumber,ParentAddress,ParentPassword,PostalCode) Values(@ParentName,@Mail,@PhoneNumber,@ParentAddress,@ParentPassword,@PostalCode)";
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ParentName", item.Name);
                    command.Parameters.AddWithValue("@Mail", item.Mail);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    command.Parameters.AddWithValue("@ParentAddress", item.Address);
                    command.Parameters.AddWithValue("@ParentPassword", item.Password);
                    command.Parameters.AddWithValue("@PostalCode", item.PostalCode);
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

        public async Task DeleteAsync(Parent item)
        {
            string query = "DELETE FROM Parent WHERE ParentID = @ParentID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@ParentID", item.Id);
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

		public Task<List<Parent>> FilterAsync(string filterCriteria)
		{
			throw new NotImplementedException();
		}

        public async Task<Parent> GetAsync(int id)
        {
            string query = "SELECT * FROM Parent WHERE ParentID = @ParentID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParentID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Parent(id,
                        reader.GetString("Mail"),
                        reader.GetString("ParentPassword"),
                        reader.GetString("ParentName"),
                        reader.GetString("PhoneNumber"),
                        reader.GetString("ParentAddress"),
                        reader.GetString("PostalCode"));
                }
                await reader.CloseAsync();
            }
            return null;
        }

        public async Task<List<Parent>> GetAllAsync()
		{
            string query = "SELECT * FROM Parent";
            List<Parent> parents = new List<Parent>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int parentId = reader.GetInt32("ParentID");
                        string parentName = reader.GetString("ParentName");
                        string mail = reader.GetString("Mail");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        string parentAddress = reader.GetString("ParentAddress");
                        string parentPassword = reader.GetString("ParentPassword");
                        string postalCode = reader.GetString("PostalCode");
                        Parent parent = new Parent(parentId, mail, parentPassword, parentName, phoneNumber, parentAddress, postalCode);
                        parents.Add(parent);
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
                return parents;
            }
        }

		public async Task UpdateAsync(Parent item)
		{
            string query = "UPDATE Parent SET ParentName = @ParentName, Mail = @Mail, PhoneNumber = @PhoneNumber, ParentAddress = @ParentAddress, ParentPassword = @ParentPassword, PostalCode = @PostalCode WHERE ParentID = @ParentID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ParentID", item.Id);
                    command.Parameters.AddWithValue("@ParentName", item.Name);
                    command.Parameters.AddWithValue("@Mail", item.Mail);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    command.Parameters.AddWithValue("@ParentAddress", item.Address);
                    command.Parameters.AddWithValue("@ParentPassword", item.Password);
                    command.Parameters.AddWithValue("@PostalCode", item.PostalCode);
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
