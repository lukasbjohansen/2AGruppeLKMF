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
        #region Methods
        /// <summary>
        /// Method used for asynchronously creating and inserting new parent objects into the SQL database.
        /// The method takes parameter 'item' of type Parent.
        /// It builds a parameterized SQL INSERT query to avoid SQL injection.
        /// The method opens a database connection using a secret connection string.
        /// The method assigns properties from the Parent object (item) to SQL parameters.
        /// The method executes the query asynchronously.
        /// </summary>
        public async Task CreateAsync(Parent item)
        {
            string query = "INSERT INTO Parent(ParentName,Mail,PhoneNumber,ParentAddress,ParentPassword,PostalCode) OUTPUT INSERTED.ParentID Values(@ParentName,@Mail,@PhoneNumber,@ParentAddress,@ParentPassword,@PostalCode)";
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
                    int newId = (int)await command.ExecuteScalarAsync();

                    item.Id = newId;
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
        /// Method used for asynchronously deleting parent objects from the SQL database using it's ID.
        /// The method takes parameter 'item' of type Parent.
        /// It builds a parameterized SQL DELETE query to avoid SQL injection.
        /// The method opens a database connection using a secret connection string.
        /// The method assigns the Id from the Parent object to @ParentID in the SQL query.
        /// The method executes the query asynchronously.
        /// </summary>
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

        /// <summary>
        /// Method used for asynchronously filtering parent objects from the SQL database using a filter criteria.
        /// The method takes parameter 'filterCriteria' of type string.
        /// It builds a parameterized SQL SELECT query to avoid SQL injection.
        /// The method opens a database connection using a secret connection string.
        /// If the operation is successful (if the filter criteria matches the columns in the database), a list of parents that match the filter criteria is returned.
        /// Currently not being used, as we made another filter method in Parent index page
        /// </summary>
        public async Task<List<Parent>> FilterAsync(string filterCriteria)
        {
            string query = @"
                SELECT * FROM Parent 
                WHERE ParentID LIKE @filterCriteria 
                OR ParentName LIKE @filterCriteria 
                OR Mail LIKE @filterCriteria
                OR PhoneNumber LIKE @filterCriteria
                OR ParentAddress LIKE @filterCriteria
                OR ParentPassword LIKE @filterCriteria
                OR PostalCode LIKE @filterCriteria";
            List<Parent> parents = new List<Parent>();
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
                        int parentId = reader.GetInt32("ParentID");
                        string parentName = reader.GetString("ParentName");
                        string mail = reader.GetString("Mail");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        string ParentAddress = reader.GetString("ParentAddress");
                        string ParentPassword = reader.GetString("ParentPassword");
                        string postalCode = reader.GetString("PostalCode");
                        Parent parent = new Parent(parentId, mail, ParentPassword, parentName, phoneNumber, ParentAddress, postalCode);
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

        /// <summary>
        /// Method used for asynchronously retrieving a parent object from the SQL database using it's ID.
        /// The method takes parameter 'id' of type int.
        /// It builds a parameterized SQL SELECT query to avoid SQL injection.
        /// The method opens a database connection using a secret connection string.
        /// The method assigns the id from the Parent object to @ParentID in the SQL query.
        /// The method executes the query asynchronously.
        /// If a matching parent is found, a new parent object with given values is created and returned.
        /// </summary>
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

        /// <summary>
        /// Method used for asynchronously retrieving parent objects from the SQL database.
        /// It builds a parameterized SQL SELECT query to avoid SQL injection where all rows and columns from the Parent table are selected.
        /// It creates a list to store parent objects from the database.
        /// The method opens a database connection using a secret connection string.
        /// The method executes the query asynchronously.
        /// The method converts each row into a new parent object and adds it to and returns the list.
        /// </summary>
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

        /// <summary>
        /// Method used for asynchronously updating parent objects from the SQL database.
        /// It builds a parameterized SQL SELECT query to avoid SQL injection.
        /// The method takes parameter 'item' of type Parent.
        /// The method opens a database connection using a secret connection string.
        /// The method assigns values from the parent object (item) to the corresponding placeholders in the SQL query.
        /// The method executes the query asynchronously.
        /// </summary>
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
        #endregion
    }
}
