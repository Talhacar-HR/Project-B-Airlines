using Dapper;
using Microsoft.Data.Sqlite;

public class VliegveldAccess
{
    private string Table = "Vliegveld";

    public List<VliegveldModel> GetAll()
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID, Naam, Stad, Land, Afkorting FROM {Table} ORDER BY ID";
            return connection.Query<VliegveldModel>(sql).AsList();
        }
    }

    public bool Exists(long id)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT EXISTS(SELECT 1 FROM {Table} WHERE ID = @Id)";
            return connection.ExecuteScalar<bool>(sql, new { Id = id });
        }
    }

    public void Add(VliegveldModel airport)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"INSERT INTO {Table} (Naam, Stad, Land, Afkorting) VALUES (@Naam, @Stad, @Land, @Afkorting)";
            connection.Execute(sql, airport);
        }
    }

    public bool Update(VliegveldModel airport)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"UPDATE {Table} SET Naam = @Naam, Stad = @Stad, Land = @Land, Afkorting = @Afkorting WHERE ID = @ID";
            return connection.Execute(sql, airport) > 0;
        }
    }
}
