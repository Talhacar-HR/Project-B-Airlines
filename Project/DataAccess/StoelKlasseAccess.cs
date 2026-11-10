using Dapper;
using Microsoft.Data.Sqlite;

public class StoelKlasseAccess
{
    private string Table = "StoelKlasse";
    private static readonly string DatabaseLoc = Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

    public List<StoelKlasseModel> GetAll()
    {
        string sql = $"SELECT ID, Naam, Toeslag FROM {Table} ORDER BY ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<StoelKlasseModel> klassen = connection.Query<StoelKlasseModel>(sql).ToList();

        return klassen;
    }

    public StoelKlasseModel? GetById(int id)
    {
        string sql = $"SELECT ID, Naam, Toeslag FROM {Table} WHERE ID = @Id";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<StoelKlasseModel> klassen = connection.Query<StoelKlasseModel>(sql, new { Id = id }).ToList();

        if (klassen.Count == 0)
            return null;

        return klassen[0];
    }
}
