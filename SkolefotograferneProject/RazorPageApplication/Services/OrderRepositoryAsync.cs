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
        #region Instance fields
        private IRepositoryAsync<Parent> _parentRepo;
		private IRepositoryAsync<OrderLine> _orderLines;
        #endregion

        #region Constructor
        public OrderRepositoryAsync(IRepositoryAsync<Parent> parentRepository, IRepositoryAsync<OrderLine> orderLines)
        {
            _parentRepo = parentRepository;
            _orderLines = orderLines;
        }
        #endregion

        #region Metoder
        //<Summary>
        //I metoden bruges der INSERT query til at kunne indsætte de korrekte dataer ind i databasen i de korrekte kolonner
        //Vi laver en ny instans af SqlConnection og putter vores ConnectionString inde i den argument
        //under try åbner vi op for databasen, laves en ny instans af SqlCommand og sætter qurey og connection inde i den parameter
        //Executer metoden ExecuteNonQueryAsync(); efter at have match de rigtige properties med query Values. vi har adgang til properties fra metodens parameter
        //Der er 2 forskellige exception hvis udførelsen af oprettelse ikke er korrekt
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

        //<Summary>
        //I metoden bruges der DELETE query til at kunne slette data ud fra dens ID række
        //Vi laver en ny instans af SqlConnection og putter vores ConnectionString inde i den argument
        //under try bliver der sagt await, hvor den vil udføre noget andet kode imens fra hvor metoden blev kaldt og vil vende tilbage
        //Der bliver åbnet op for databasen, laves en ny instans af SqlCommand og sætter qurey og connection inde i den parameter
        //Executer metoden ExecuteNonQueryAsync(); efter at have match med den rigtige property og query value. vi har adgang til property fra metodens parameter
        //Der er 2 forskellige exception hvis udførelsen af oprettelse ikke er korrekt
        public async Task DeleteAsync(Order item) //FEJL HER
		{
			string query = "DELETE FROM PhotoOrder WHERE PhotoOrderID = @PhotoOrderID";
			using(SqlConnection connection = new SqlConnection(Secret.ConnectionString))
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					await connection.OpenAsync();
					command.Parameters.AddWithValue("@PhotoOrderID", item.Parent); //DEN SIGER UGYLDIG ID
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

        //<Summary>
        //I metoden bruges der SELECT query til at kunne hente data ud fra en betingelse som er @filterCriteria på en hver række
        //Der bliver instaseret en lokal liste af sekretær
        //Vi laver en ny instans af SqlConnection og putter vores ConnectionString inde i den argument
        //Under bliver der sagt await og åbnet for databasen
        //Der bliver lavet en ny instans af SqlCommand
        //Der bliver sat metodens parameter og @FilterCriteria til metode kald AddWithValue
        //ExcecuteReaderAsync() bliver kørt som await og sættes på reader
        //Whileloop køre så længe den kan hente én skekretær ad gangen fra den filtrerede resultat
        //den læser hver række og sætter dem ind i deres respektive variabler, men skolens ID skulle hentes via skole instance field og bliver konverteret til int
        //der laves en ny instant af sekretær, bliver tilføjet til listen og så lukker for databasen til sidst
        //returner den filtrerede liste
        //Der er 2 forskellige exception hvis udførelsen af oprettelse ikke er korrekt
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

        //<Summary>
        //SELECT query henteren sekreæter ud fra dens ID
        //Vi laver en ny instans af SqlConnection og putter vores ConnectionString inde i den argument
        //Der bliver lavet en ny instans af SqlCommand som får query og connection i den argument
        //Der bliver sagt await og åbnet for databasen
        //ExecuteReaderAsync() bliver kørt og gemt under reader variablen
        //en if-statement bliver excecute, hvis condition er sandt
        //Den vil udføre en returnering af en ny sekretær, hvor der bliver puttet dens arguementer i den rigtige rækkefølge
        //Lukker ned for databasen efter og hvis condition var false, vil den returner null
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

        //<Summary>
        //SELECT * henter alle sekretærerne fra databasen
        //en ny instans af en lokal liste af typen Secretary til at returner efter at have tilføjet sekretæerene
        //Vi laver en ny instans af SqlConnection og putter vores ConnectionString inde i den argument
        //Der bliver sagt await og åbnet for databasen
        //Der bliver lavet en ny instans af SqlCommand som får query og connection i den argument
        //ExecuteReaderAsync() bliver kørt og gemt under reader variablen
        //Whileloop køre så længe den kan hente én skekretær ad gangen
        //den læser hver række og sætter dem ind i deres respektive variabler
        //ny instanse med variablerne som arguementer
        //Tilføjer secretary til listen, lukker ned for databasen og returner listen
        //Der er 2 forskellige exception hvis udførelsen af oprettelse ikke er korrekt
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

        //UPDATE query opdater værdierne der er i de rækker der er blevet sat til med SET ud fra sekretærens ID
        //Vi laver en ny instans af SqlConnection og putter vores ConnectionString inde i den argument
        //Der bliver sagt await og åbnet for databasen
        //Der bliver lavet en ny instans af SqlCommand som får query og connection i den argument
        //Executer metoden ExecuteNonQueryAsync(); efter at have match de rigtige properties med query Values. vi har adgang til properties fra metodens parameter
        //Der er 1 exception til hvis try delen ikke bliver korrekt udført

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
        #endregion
    }
}
