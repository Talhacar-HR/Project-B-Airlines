using Dapper;
using Microsoft.Data.Sqlite;

public class UserAccess
{
    private string Table = "Users";
 
    public void AddUser(UserModel user)
    {
        // "using" sluit de verbinding automatisch weer als de methode klaar is
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();
 
            string sql = $"INSERT INTO {Table} (Email, Password) VALUES (@Email, @Password)";
            connection.Execute(sql, user);
        }
    }
 
    public UserModel? GetUserByEmail(string email)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();
 
            string sql = $"SELECT Id, Email, Password FROM {Table} WHERE Email = @Email";
 
            // Geeft null terug als er geen gebruiker met dit e-mailadres is
            return connection.QueryFirstOrDefault<UserModel>(sql, new { Email = email });
        }
    }
}