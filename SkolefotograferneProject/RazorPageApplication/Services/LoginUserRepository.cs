using Microsoft.Data.SqlClient;

namespace RazorPageApplication.Services
{
    public class LoginUserRepository
    {
        public async Task<int> CreateAsync(string username, string password)
        {
            string query =
                @"INSERT INTO LoginUser (Username, Password)
                  VALUES(@Username, @Password)
                  SELECT SCOPE_IDENTITY()";
            await using SqlConnection connection = new(Secret.ConnectionString);
            await connection.OpenAsync();
            await using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            return (int)await command.ExecuteScalarAsync();
        }

        public async Task UpdateAsync(int userId, string username, string password)
        {
            string query =
                @"UPDATE LoginUser 
                  SET Username = @Username, Password = @Password 
                  WHERE UserID = @UserID";
            await using SqlConnection connection = new(Secret.ConnectionString);
            await connection.OpenAsync();
            await using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int userId)
        {
            string query = "DELETE FROM LoginUser WHERE UserID = @UserID";
            await using SqlConnection connection = new(Secret.ConnectionString);
            await connection.OpenAsync();
            await using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);
            await command.ExecuteNonQueryAsync();
        }
    }
}
