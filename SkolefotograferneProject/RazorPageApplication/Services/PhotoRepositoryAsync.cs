using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
    public class PhotoRepositoryAsync : IRepositoryAsync<Photo>
	{
        private IRepositoryAsync<Photographer> _photographerRepo;
        private IRepositoryAsync<Student> _studentRepo;
        public PhotoRepositoryAsync(IRepositoryAsync<Photographer> photographerRepository, IRepositoryAsync<Student> studentRepository)
        {
            _photographerRepo = photographerRepository;
            _studentRepo = studentRepository;
        }
        public async Task CreateAsync(Photo item)
		{
            // Added OUTPUT INSERTED.PhotoID.
            // INSERTED is the item that is inserted in the SQL.
            // OUTPUT makes it available in ExecuteScalarAsync as a return value.
            string query = "INSERT INTO Photo(FilePath,PhotoDate,PhotographerID,StudentID) OUTPUT INSERTED.PhotoID Values(@FilePath,@PhotoDate,@PhotographerID,@StudentID)";
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FilePath", item.FilePath);
                    command.Parameters.AddWithValue("@PhotoDate", item.Date);
                    command.Parameters.AddWithValue("@PhotographerID", item.Photographer.Id);
                    command.Parameters.AddWithValue("@StudentID", item.Student.Id);
                    // Changed to ExecuteScalarAsync to get the outputted value.
                    int newId = (int)await command.ExecuteScalarAsync();
                    // The argument which is a reference type gets updated with the new Id,
                    // so it is available within the model page.
                    item.Id = newId;
                    // Alle de steder, hvor vi kalder CreateAsync, smides et item ind med et ukendt Id, da det genereres automatisk.
                    // Derfor er vi nødt til at 'sette' id'et her, så vi kan bruge det, der hvor vi kalder CreateAsync.
                    // Fx i vores CreatePhoto C# side, hvor vi skulle bruge Id'et i vores billedsti, men det kunne vi ikke, fordi Id'et ikke var sat endnu.
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

		public async Task DeleteAsync(Photo item)
		{
            string query = "DELETE FROM Photo WHERE PhotoID = @PhotoID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@PhotoID", item.Id);
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

		public Task<List<Photo>> FilterAsync(string filterCriteria)
		{
			throw new NotImplementedException();
		}

        public async Task<Photo> GetAsync(int id)
        {
            string query = "SELECT * FROM Photo WHERE PhotoID = @PhotoID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PhotoID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    string filePath = reader.GetString("FilePath");
                    DateTime date = reader.GetDateTime("PhotoDate");
                    int photographerId = reader.GetInt32("PhotographerID");
                    int studentId = reader.GetInt32("StudentID");
                    Photographer photographer = await _photographerRepo.GetAsync(photographerId);
                    Student student = await _studentRepo.GetAsync(studentId);

                    return new Photo(id, filePath, date, photographer, student);
                }
                await reader.CloseAsync();

            }
            return null;
        }

        public async Task<List<Photo>> GetAllAsync()
		{
            //throw new NotImplementedException();
            string query = "SELECT * FROM Photo";
            List<Photo> photos = new List<Photo>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int photoId = reader.GetInt32("PhotoID");
                        string filePath = reader.GetString("FilePath");
                        DateTime date = reader.GetDateTime("PhotoDate");
                        int photographerId = reader.GetInt32("PhotographerID");
                        int studentId = reader.GetInt32("StudentID");
                        Photo photo = new Photo(photoId, filePath, date, await _photographerRepo.GetAsync(photographerId), await _studentRepo.GetAsync(studentId));
                        photos.Add(photo);
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
                return photos;
            }
        }

		public async Task UpdateAsync(Photo item)
		{
            string query = "UPDATE Photo SET FilePath = @FilePath, PhotoDate = @PhotoDate, PhotographerID = @PhotographerID, StudentID = @StudentID WHERE PhotoID = @PhotoID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PhotoID", item.Id);
                    command.Parameters.AddWithValue("@FilePath", item.FilePath);
                    command.Parameters.AddWithValue("@PhotoDate", item.Date);
                    command.Parameters.AddWithValue("@PhotographerID", item.Photographer.Id);
                    command.Parameters.AddWithValue("@StudentID", item.Student.Id);
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
