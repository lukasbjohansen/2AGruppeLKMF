using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Collections;
using System.Data;

namespace RazorPageApplication.Services
{
    public class PhotoEventRepositoryAsync : IRepositoryAsync<PhotoEvent>
    {
        #region Instance Fields
        private readonly IRepositoryAsync<Photographer> _photographerRepo;
        private readonly IRepositoryAsync<SchoolClass> _schoolClassRepo;
        #endregion

        #region Constructors
        public PhotoEventRepositoryAsync(IRepositoryAsync<Photographer> photographerRepo,
                                         IRepositoryAsync<SchoolClass> schoolClassRepo)
        {
            _photographerRepo = photographerRepo;
            _schoolClassRepo = schoolClassRepo;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handles creating/adding a PhotoEvent to the database asynchronously.
        /// Takes a photoEvent reference as an argument.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
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

        /// <summary>
        /// Handles deleting an existing PhotoEvent from the database asynchronously.
        /// Takes a photoEvent reference as an argument.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
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

        /// <summary>
        /// Handles filtering PhotoEvents from the database asynchronously.
        /// Takes a string, filterCriteria, as an argument, and returns a List of PhotoEvents matching the criteria.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
        public async Task<List<PhotoEvent>> FilterAsync(string filterCriteria)
        {
            string query =
                @"SELECT * FROM Photographer
                WHERE PhotoEventID LIKE @filterCriteria
                OR StartDate LIKE @filterCriteria
                OR EndDate LIKE @filterCriteria
                OR PhotoEventLocation LIKE @filterCriteria
                OR PhotographerID LIKE @filterCriteria
                OR SchoolClassID LIKE @filterCriteria";
            List<PhotoEvent> photoEvents = new();
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@filterCriteria", $"%{filterCriteria}%");
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
                return photoEvents;
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
        }

        /// <summary>
        /// Handles retrieving and returns PhotoEvent reference from  the database asynchronously.
        /// Catches an SqlException, in which case a RepositoryException is thrown.
        /// </summary>
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

        /// <summary>
        /// Handles retrieving all PhotoEvents from the database asynchronously and returning them as a List.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
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
                return photoEvents;
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
        }

        /// <summary>
        /// Handles updating all PhotoEvents from the database asynchronously.
        /// Catches an SqlException, in which case a RepositoryException is thrown
        /// </summary>
        public async Task UpdateAsync(PhotoEvent photoEvent)
        {
            string query =
                @"UPDATE PhotoEvent
                SET StartDate = @StartDate,
                    EndDate = @EndDate,
                    PhotoEventLocation = @PhotoEventLocation,
                    PhotographerID = @PhotographerID,
                    SchoolClassID = @SchoolClassID
                WHERE PhotoEventID = @PhotoEventID";
            try
            {
                using SqlConnection connection = new(Secret.ConnectionString);
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@PhotoEventID", photoEvent.Id);
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
        #endregion
    }
}
