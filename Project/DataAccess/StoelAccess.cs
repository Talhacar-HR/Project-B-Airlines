using Dapper;
using Microsoft.Data.Sqlite;

public class StoelAccess
{
    private string Table = "Stoel";

    public List<StoelModel> GetAll()
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse FROM {Table} ORDER BY ID";
            return connection.Query<StoelModel>(sql).AsList();
        }
    }

    public void Add(StoelModel seat)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"INSERT INTO {Table} (VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (@VliegtuigID, @StoelNummer, @RijNummer, @StoelKlasse)";
            connection.Execute(sql, seat);
        }
    }

    public bool Update(StoelModel seat)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"UPDATE {Table} SET VliegtuigID = @VliegtuigID, StoelNummer = @StoelNummer, RijNummer = @RijNummer, StoelKlasse = @StoelKlasse WHERE ID = @ID";
            return connection.Execute(sql, seat) > 0;
        }
    }
}
