using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
    public class PhotoEventRepositoryAsync : IRepositoryAsync<PhotoEvent>
    {
        private readonly IRepositoryAsync<Photographer> _photographerRepo;
        private readonly IRepositoryAsync<SchoolClass> _schoolClassRepo;

        public PhotoEventRepositoryAsync(IRepositoryAsync<Photographer> photographerRepo,
                                         IRepositoryAsync<SchoolClass> schoolClassRepo)
        {
            _photographerRepo = photographerRepo;
            _schoolClassRepo = schoolClassRepo;
        }

        public async Task CreateAsync(PhotoEvent photoEvent)
        {
            string query =
                @"INSERT INTO
                PhotoEvent(StartDate, EndDate, PhotoEventLocation, PhotographerID, SchoolClassID) 
                Values(@StartDate, @EndDate, @PhotoEventLocation, @PhotographerID, @SchoolClassID)";
            try
            {
                await using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                await using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@StartDate", photoEvent.StartTime);
                command.Parameters.AddWithValue("@EndDate", photoEvent.EndTime);
                command.Parameters.AddWithValue("@PhotoEventLocation", photoEvent.Location);
                command.Parameters.AddWithValue("@PhotographerID", photoEvent.Photographer.Id);
                command.Parameters.AddWithValue("@SchoolClassID", photoEvent.SchoolClass.Id);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Create, "Ugyldigt input");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Create, e.GetFullMessage());
            }
        }

        public async Task DeleteAsync(PhotoEvent photoEvent)
        {
            string query =
                @"DELETE FROM PhotoEvent
                WHERE PhotoEventID = @PhotoEventID";
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PhotoEventID", photoEvent.Id);
                await command.ExecuteNonQueryAsync();

            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Delete, "Ugyldig ID");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Delete, e.GetFullMessage());
            }
        }

        // TODO
        public async Task<List<PhotoEvent>> FilterAsync(string filterCriteria)
        {
            throw new NotImplementedException();
        }

        public async Task<PhotoEvent> GetAsync(int id)
        {
            string query =
                @"SELECT * FROM PhotoEvent 
                WHERE PhotoEventID = @PhotoEventID";
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PhotoEventID", id);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new(
                        id,
                        reader.GetDateTime("StartDate"),
                        reader.GetDateTime("EndDate"),
                        reader.GetString("PhotoEventLocation"),
                        await _photographerRepo.GetAsync(reader.GetInt32("PhotographerID")),
                        await _schoolClassRepo.GetAsync(reader.GetInt32("SchoolClassID"))
                    );
                }
            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Read, "Indlæsningsfejl");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Delete, e.GetFullMessage());
            }
            return null;
        }

        public async Task<List<PhotoEvent>> GetAllAsync()
        {
            string query = "SELECT * FROM PhotoEvent";
            List<PhotoEvent> photoEvents = new();
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    photoEvents.Add(new(
                        reader.GetInt32("PhotoEventID"),
                        reader.GetDateTime("StartDate"),
                        reader.GetDateTime("EndDate"),
                        reader.GetString("PhotoEventLocation"),
                        await _photographerRepo.GetAsync(reader.GetInt32("PhotographerID")),
                        await _schoolClassRepo.GetAsync(reader.GetInt32("SchoolClassID"))
                    ));
                }
            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Read, "Kan ikke indlæses");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Read, e.GetFullMessage());
            }
            return photoEvents;
        }

        public async Task UpdateAsync(PhotoEvent photoEvent)
        {
            string query =
                @"UPDATE PhotoEvent
                SET StartDate = @StartDate, EndDate = @EndDate, PhotoEventLocation = @PhotoEventLocation, PhotographerID = @PhotographerID, SchoolClassID = @SchoolClassID
                WHERE PhotoEventID = @PhotoEventID";
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@StartDate", photoEvent.StartTime);
                command.Parameters.AddWithValue("@EndDate", photoEvent.EndTime);
                command.Parameters.AddWithValue("@PhotoEventLocation", photoEvent.Location);
                command.Parameters.AddWithValue("@PhotographerID", photoEvent.Photographer.Id);
                command.Parameters.AddWithValue("@SchoolClassID", photoEvent.SchoolClass.Id);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Update, "Ugyldigt input");
            }
            catch (Exception e)
            {
                e.PrintWithType();
                throw new RepositoryException(RepositoryExceptionType.Read, e.GetFullMessage());
            }
        }
    }
}
