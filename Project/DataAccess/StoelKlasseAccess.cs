using Dapper;
using Microsoft.Data.Sqlite;

public class StoelKlasseAccess
{
    private string Table = "StoelKlasse";

    public List<StoelKlasseModel> GetAll()
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID, Naam, Toeslag FROM {Table} ORDER BY ID";
            return connection.Query<StoelKlasseModel>(sql).AsList();
        }
    }

    public StoelKlasseModel? GetById(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID, Naam, Toeslag FROM {Table} WHERE ID = @ID";
            return connection.QueryFirstOrDefault<StoelKlasseModel>(sql, new { ID = id });
        }
    }
}
