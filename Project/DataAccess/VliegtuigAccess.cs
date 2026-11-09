using Dapper;
using Microsoft.Data.Sqlite;

public class VliegtuigAccess
{
    private string Table = "Vliegtuig";

    public List<VliegtuigModel> GetAll()
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID, BaggageRuimte, Model, Merk FROM {Table} ORDER BY ID";
            return connection.Query<VliegtuigModel>(sql).AsList();
        }
    }

    public bool Exists(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT EXISTS(SELECT 1 FROM {Table} WHERE ID = @Id)";
            return connection.ExecuteScalar<bool>(sql, new { Id = id });
        }
    }

    public void Add(VliegtuigModel plane)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"INSERT INTO {Table} (BaggageRuimte, Model, Merk) VALUES (@BaggageRuimte, @Model, @Merk)";
            connection.Execute(sql, plane);
        }
    }

    public bool Update(VliegtuigModel plane)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"UPDATE {Table} SET BaggageRuimte = @BaggageRuimte, Model = @Model, Merk = @Merk WHERE ID = @ID";
            return connection.Execute(sql, plane) > 0;
        }
    }
}
