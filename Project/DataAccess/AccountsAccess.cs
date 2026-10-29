using Microsoft.Data.Sqlite;

using Dapper;

public class AccountsAccess
{
    private SqliteConnection _connection = new SqliteConnection("Data Source=DataSources/ApplicatieDB.db");

    private string Table = "Gebruikers";

    public void Write(AccountModel account)
    {
        string sql = $"INSERT INTO {Table} (Email, Wachtwoord, Gebruikersnaam) VALUES (@EmailAddress, @Password, @FullName)";
        _connection.Execute(sql, account);
    }

    public AccountModel? GetByEmail(string email)
    {
        string sql = $"SELECT ID AS Id, Email, Wachtwoord AS Password, Gebruikersnaam AS FullName, Admin AS IsAdmin FROM {Table} WHERE Email = @Email";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { Email = email });
    }

    public void Update(AccountModel account)
    {
        string sql = $"UPDATE {Table} SET Email = @EmailAddress, Wachtwoord = @Password, Gebruikersnaam = @FullName WHERE ID = @Id";
        _connection.Execute(sql, account);
    }

    public void Delete(AccountModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE ID = @Id";
        _connection.Execute(sql, new { Id = account.Id });
    }
}