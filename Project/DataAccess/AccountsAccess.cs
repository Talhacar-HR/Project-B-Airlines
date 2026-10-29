using Microsoft.Data.Sqlite;

using Dapper;

public class AccountsAccess
{
    private string Table = "Gebruikers";

    public void Write(AccountModel account)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"INSERT INTO {Table} (Email, Wachtwoord, Gebruikersnaam) VALUES (@EmailAddress, @Password, @FullName)";
            connection.Execute(sql, account);
        }
    }

    public AccountModel? GetByEmail(string email)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID AS Id, Email, Wachtwoord AS Password, Gebruikersnaam AS FullName, Admin AS IsAdmin FROM {Table} WHERE Email = @Email";
            return connection.QueryFirstOrDefault<AccountModel>(sql, new { Email = email });
        }
    }

    public void Update(AccountModel account)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"UPDATE {Table} SET Email = @EmailAddress, Wachtwoord = @Password, Gebruikersnaam = @FullName WHERE ID = @Id";
            connection.Execute(sql, account);
        }
    }

    public void Delete(AccountModel account)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"DELETE FROM {Table} WHERE ID = @Id";
            connection.Execute(sql, new { Id = account.Id });
        }
    }
}