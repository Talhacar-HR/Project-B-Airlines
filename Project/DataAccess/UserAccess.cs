using Dapper;
using Microsoft.Data.Sqlite;

public class UserAccess
{
    private string Table = "Gebruikers";
    private static readonly string DatabaseLoc = Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");
 
    // Geeft true terug als de gebruiker is opgeslagen
    public bool Add(UserModel user)
    {
        // Dapper opent en sluit de verbinding automatisch tijdens de query.
        // Gebruikersnaam is verplicht in de tabel, daarom gebruiken we het e-mailadres ook als gebruikersnaam
        string sql = $"INSERT INTO {Table} (Email, Gebruikersnaam, Wachtwoord) VALUES (@Email, @Email, @Password)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int rowsAffected = connection.Execute(sql, user);

        return rowsAffected >= 1;
    }
 
    public UserModel? GetUserByEmail(string email)
    {
        string sql = $"SELECT ID AS Id, Email, Wachtwoord AS Password FROM {Table} WHERE Email = @Email";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        // Geeft null terug als er geen gebruiker met dit e-mailadres is
        List<UserModel> users = connection.Query<UserModel>(sql, new { Email = email }).ToList();

        if (users.Count == 0)
            return null;

        return users[0];
    }
}