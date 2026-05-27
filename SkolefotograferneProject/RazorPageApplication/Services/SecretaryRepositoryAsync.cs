using Microsoft.Data.SqlClient;
using RazorPageApplication.Exceptions;
using RazorPageApplication.Helpers;
using RazorPageApplication.Interfaces;
using RazorPageApplication.Models;
using System.Data;

namespace RazorPageApplication.Services
{
    public class SecretaryRepositoryAsync : IRepositoryAsync<Secretary>
    {
        #region Instance field
        //Instance field til at kunne bruge resourcerne for Skole class
        private IRepositoryAsync<School> _schoolRepo;
        #endregion
        #region Constructor
        //den bliver gemt under instance field
        public SecretaryRepositoryAsync(IRepositoryAsync<School> schoolRepo)
        {
            _schoolRepo = schoolRepo;
        }
        #endregion
        #region Metoder
        //<Summary>
        //I metoden bruges der INSERT query til at kunne indsætte de korrekte dataer ind i databasen i de korrekte kolonner
        //Vi laver en ny instans af SqlConnection og putter vores ConnectionString inde i den argument
        //under try åbner vi op for databasen, laves en ny instans af SqlCommand og sætter qurey og connection inde i den parameter
        //Executer metoden ExecuteNonQueryAsync(); efter at have match de rigtige properties med query Values. vi har adgang til properties fra metodens parameter
        //Der er 2 forskellige exception hvis udførelsen af oprettelse ikke er korrekt
        public async Task CreateAsync(Secretary item)
        {
            string query = "INSERT INTO Secretary(SecretaryName, PhoneNumber, Mail, SecretaryPassword, SchoolID) Values (@SecretaryName, @PhoneNumber, @Mail, @SecretaryPassword, @SchoolID)";
            await using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SecretaryName", item.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    command.Parameters.AddWithValue("@Mail", item.Mail);
                    command.Parameters.AddWithValue("@SecretaryPassword", item.Password);
                    command.Parameters.AddWithValue("@SchoolID", item.School.Id);
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
        public async Task DeleteAsync(Secretary item) 
        {
            string query = "DELETE FROM Secretary WHERE SecretaryID = @SecretaryID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@SecretaryID", item.Id);
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
        public async Task<List<Secretary>> FilterAsync(string filterCriteria)
        {
            string query = @"
                SELECT * FROM Secretary 
                WHERE SecretaryID LIKE @filterCriteria 
                OR SecretaryName LIKE @filterCriteria 
                OR PhoneNumber LIKE @filterCriteria
                OR Mail LIKE @filterCriteria
                OR SecretaryPassword LIKE @filterCriteria
                OR SchoolID LIKE @filterCriteria";
            List<Secretary> Secretaries = new List<Secretary>();
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
                        int secretaryID = reader.GetInt32("SecretaryID");
                        string secretaryName = reader.GetString("SecretaryName");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        string mail = reader.GetString("Mail");
                        string secretaryPassword = reader.GetString("SecretaryPassword");
                        School schoolID = await _schoolRepo.GetAsync(reader.GetInt32("SchoolID"));
                        Secretary secretary = new Secretary(secretaryID, mail, secretaryPassword, secretaryName, phoneNumber, schoolID);
                        Secretaries.Add(secretary);
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
                return Secretaries;
            }
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

        public async Task<Secretary?> GetAsync(int id)
        {
            string query = "SELECT * FROM Secretary WHERE SecretaryID = @SecretaryID";
            
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SecretaryID", id);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Secretary(id,
                        reader.GetString("Mail"),
                        reader.GetString("SecretaryPassword"),
                        reader.GetString("SecretaryName"),
                        reader.GetString("PhoneNumber"),
                        await _schoolRepo.GetAsync(reader.GetInt32("SchoolID")));
                }
                await reader.CloseAsync();
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
        public async Task<List<Secretary>> GetAllAsync()
        {
            string query = "SELECT * FROM Secretary";
            List<Secretary> secretaries = new List<Secretary>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int secretaryID = reader.GetInt32("SecretaryID");
                        string secretaryName = reader.GetString("SecretaryName");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        string mail = reader.GetString("Mail");
                        string secretaryPassword = reader.GetString("SecretaryPassword");
                        int schoolID = reader.GetInt32("SchoolID");

                        Secretary secretary = new Secretary(secretaryID, mail, secretaryPassword, secretaryName, phoneNumber, await _schoolRepo.GetAsync(schoolID));
                        secretaries.Add(secretary);
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
                return secretaries;

            }
        }

        //UPDATE query opdater værdierne der er i de rækker der er blevet sat til med SET ud fra sekretærens ID
        //Vi laver en ny instans af SqlConnection og putter vores ConnectionString inde i den argument
        //Der bliver sagt await og åbnet for databasen
        //Der bliver lavet en ny instans af SqlCommand som får query og connection i den argument
        //Executer metoden ExecuteNonQueryAsync(); efter at have match de rigtige properties med query Values. vi har adgang til properties fra metodens parameter
        //Der er 1 exception til hvis try delen ikke bliver korrekt udført

        public async Task UpdateAsync(Secretary item)
        {
            string query = "UPDATE Secretary SET SecretaryName = @SecretaryName, PhoneNumber = @PhoneNumber, Mail = @Mail, SecretaryPassword = @SecretaryPassword, SchoolID = @SchoolID WHERE SecretaryID = @SecretaryID";
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SecretaryID", item.Id);
                    command.Parameters.AddWithValue("@SecretaryName", item.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    command.Parameters.AddWithValue("@Mail", item.Mail);
                    command.Parameters.AddWithValue("@SecretaryPassword", item.Password);
                    command.Parameters.AddWithValue("@SchoolID", item.School.Id);
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
