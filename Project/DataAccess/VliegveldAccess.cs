using Dapper;
using Microsoft.Data.Sqlite;

public class VliegveldAccess
{
    private string Table = "Vliegveld";
    private static readonly string DatabaseLoc = Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

    public List<VliegveldModel> GetAll()
    {
        string sql = $"SELECT ID, Naam, Stad, Land, Afkorting FROM {Table} ORDER BY ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<VliegveldModel> airports = connection.Query<VliegveldModel>(sql).ToList();

        return airports;
    }

    public bool Exists(int id)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM {Table} WHERE ID = @Id)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int count = connection.QueryFirst<int>(sql, new { Id = id });

        return count > 0;
    }

    public VliegveldModel? GetById(int id)
    {
        string sql = $"SELECT ID, Naam, Stad, Land, Afkorting FROM {Table} WHERE ID = @Id";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<VliegveldModel> airports = connection.Query<VliegveldModel>(sql, new { Id = id }).ToList();

        if (airports.Count == 0)
            return null;

        return airports[0];
    }

    public void Add(VliegveldModel airport)
    {
        string sql = $"INSERT INTO {Table} (Naam, Stad, Land, Afkorting) VALUES (@Naam, @Stad, @Land, @Afkorting)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        connection.Execute(sql, airport);
    }

    public bool Update(VliegveldModel airport)
    {
        string sql = $"UPDATE {Table} SET Naam = @Naam, Stad = @Stad, Land = @Land, Afkorting = @Afkorting WHERE ID = @ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int rowsAffected = connection.Execute(sql, airport);

        return rowsAffected >= 1;
    }
}
