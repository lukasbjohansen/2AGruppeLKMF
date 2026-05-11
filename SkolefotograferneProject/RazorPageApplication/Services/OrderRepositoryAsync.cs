using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;
using System.Security.Cryptography.Xml;

namespace RazorPageApplication.Services
{
	public class OrderRepositoryAsync : IRepositoryAsync<Order>
	{
        private IRepositoryAsync<Parent> _parentRepo;
		private IRepositoryAsync<Parent> _orderLines;
        public OrderRepositoryAsync(IRepositoryAsync<Parent> parentRepository, IRepositoryAsync<Parent> orderLines)
        {
            _parentRepo = parentRepository;
            _orderLines = orderLines;
        }
        public async Task CreateAsync(Order item)
		{
			string query = "INSERT INTO PhotoOrder(PhotoOrderDate, TotalPrice, ParentID) Values (@PhotoOrderDate, @TotalPrice, @ParentID)";
			await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
			{
				try
				{
					await connection.OpenAsync();
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@PhotoOrderDate", item.Date);
					command.Parameters.AddWithValue("@TotalPrice", item.TotalPrice);
					command.Parameters.AddWithValue("@ParentID", item.Parent);
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

		public async Task DeleteAsync(Order item) //FEJL HER
		{
			string query = "DELETE FROM PhotoOrder WHERE PhotoOrderID = @PhotoOrderID";
			using(SqlConnection connection = new SqlConnection(Secret.ConnectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					await connection.OpenAsync();
					command.Parameters.AddWithValue("@PhotoOrderID", item.Id); //DEN SIGER UGYLDIG ID
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

		public async Task<List<Order>> FilterAsync(string filterCriteria) //tjek metoden og alt andet
		{
            //mangler
            string query = @"
                SELECT * FROM PhotoOrder 
                WHERE PhotoOrderID LIKE @filterCriteria 
                OR PhotoOrderDate LIKE @filterCriteria 
                OR TotalPrice LIKE @filterCriteria
                OR ParentID LIKE @filterCriteria";
			List<Order> filteredOrder = new List<Order>();
			using(SqlConnection connection = new SqlConnection(Secret.ConnectionString))
			{
				try
				{
					await connection.OpenAsync();
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@filterCriteria", $"%{filterCriteria}%");
					SqlDataReader reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						int orderID = reader.GetInt32("PhotoOrderID");
						DateTime orderDate = reader.GetDateTime("PhotoOrderDate"); //ER DET DateTime og GetDateTime???
						double totalPrice = reader.GetDouble("TotalPrice");
						int parentID = reader.GetInt32("ParentID");

						Order order = new Order(orderID, orderDate, totalPrice, await _parentRepo.GetAsync(parentID), null);
						filteredOrder.Add(order);
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
            }
			return filteredOrder;
		}

        public async Task<Order> GetAsync(int id)
        {
			string query = "SELECT * FROM PhotoOrder WHERE PhotoOrderID = @PhotoOrderID";
			Parent? parent = await _parentRepo.GetAsync(id);
			using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@PhotoOrderID", id);
					await connection.OpenAsync();
					SqlDataReader reader = await command.ExecuteReaderAsync();
					if (await reader.ReadAsync())
					{
						return new Order(id,
							reader.GetDateTime("PhotoOrderDate"),
							reader.GetDouble("TotalPrice"),
							parent, null); 
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
            }
			return null;
        }

        public async Task<List<Order>> GetAllAsync()
		{
			string query = "SELECT * FROM PhotoOrder";
			List<Order> orders = new List<Order>();
			using(SqlConnection connection = new SqlConnection(Secret.ConnectionString))
			{
				try
				{
					await connection.OpenAsync();
					SqlCommand command = new SqlCommand(query, connection);
					SqlDataReader reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						int orderID = reader.GetInt32("PhotoOrderID");
						DateTime orderDate = reader.GetDateTime("PhotoOrderDate");
						double totalPrice = reader.GetDouble("TotalPrice");
						int parentID = reader.GetInt32("ParentID"); 
						
						Order order = new Order(orderID, orderDate, totalPrice, await _parentRepo.GetAsync(parentID), null);
						orders.Add(order);
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
                return orders;

            }
		}

		public async Task UpdateAsync(Order item)
		{
			string query = "UPDATE PhotoOrder SET PhotoOrderDate = @PhotoOrderDate, TotalPrice = @TotalPrice, ParentID = @ParentID, PhotoOrderID = @PhotoOrderID";
			using(SqlConnection connection = new SqlConnection(Secret.ConnectionString))
			{
				try
				{
					await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PhotoOrderID", item.Id);
                    command.Parameters.AddWithValue("@PhotoOrderDate", item.Date);
                    command.Parameters.AddWithValue("@TotalPrice", item.TotalPrice);
                    command.Parameters.AddWithValue("@ParentID", item.Parent);
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
