using Microsoft.Data.Sqlite;

using Dapper;

public class AccountsAccess
{
    private string Table = "Gebruikers";
    private static readonly string DatabaseLoc = Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

    public void Write(AccountModel account)
    {
        string sql = $"INSERT INTO {Table} (Email, Wachtwoord, Gebruikersnaam) VALUES (@EmailAddress, @Password, @FullName)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        connection.Execute(sql, account);
    }

    public AccountModel? GetByEmail(string email)
    {
        string sql = $"SELECT ID AS Id, Email AS EmailAddress, Wachtwoord AS Password, Gebruikersnaam AS FullName, Admin AS IsAdmin FROM {Table} WHERE Email = @Email";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<AccountModel> accounts = connection.Query<AccountModel>(sql, new { Email = email }).ToList();

        if (accounts.Count == 0)
            return null;

        return accounts[0];
    }

    public void Update(AccountModel account)
    {
        string sql = $"UPDATE {Table} SET Email = @EmailAddress, Wachtwoord = @Password, Gebruikersnaam = @FullName WHERE ID = @Id";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        connection.Execute(sql, account);
    }

    public void Delete(AccountModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE ID = @Id";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        connection.Execute(sql, new { Id = account.Id });
    }
}