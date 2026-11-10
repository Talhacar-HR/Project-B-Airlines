using Dapper;
using Microsoft.Data.Sqlite;

public class VliegtuigAccess
{
    private string Table = "Vliegtuig";
    private static readonly string DatabaseLoc = Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

    public List<VliegtuigModel> GetAll()
    {
        string sql = $"SELECT ID, BaggageRuimte, Model, Merk FROM {Table} ORDER BY ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<VliegtuigModel> planes = connection.Query<VliegtuigModel>(sql).ToList();

        return planes;
    }

    public bool Exists(int id)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM {Table} WHERE ID = @Id)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int count = connection.QueryFirst<int>(sql, new { Id = id });

        return count > 0;
    }

    public VliegtuigModel? GetById(int id)
    {
        string sql = $"SELECT ID, BaggageRuimte, Model, Merk FROM {Table} WHERE ID = @Id";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<VliegtuigModel> planes = connection.Query<VliegtuigModel>(sql, new { Id = id }).ToList();

        if (planes.Count == 0)
            return null;

        return planes[0];
    }

    public void Add(VliegtuigModel plane)
    {
        string sql = $"INSERT INTO {Table} (BaggageRuimte, Model, Merk) VALUES (@BaggageRuimte, @Model, @Merk)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        connection.Execute(sql, plane);
    }

    public bool Update(VliegtuigModel plane)
    {
        string sql = $"UPDATE {Table} SET BaggageRuimte = @BaggageRuimte, Model = @Model, Merk = @Merk WHERE ID = @ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int rowsAffected = connection.Execute(sql, plane);

        return rowsAffected >= 1;
    }
}
