using Dapper;
using Microsoft.Data.Sqlite;

public class StoelAccess
{
    private string Table = "Stoel";
    private static readonly string DatabaseLoc = Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

    public List<StoelModel> GetAll()
    {
        string sql = $"SELECT ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse FROM {Table} ORDER BY ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<StoelModel> seats = connection.Query<StoelModel>(sql).ToList();

        return seats;
    }

    public StoelModel? GetById(int id)
    {
        string sql = $"SELECT ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse FROM {Table} WHERE ID = @Id";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<StoelModel> seats = connection.Query<StoelModel>(sql, new { Id = id }).ToList();

        if (seats.Count == 0)
            return null;

        return seats[0];
    }

    public int GetTotalSeats(int VliegtuigID)
    {
        string sql = "SELECT COUNT(*) FROM Stoel WHERE vliegtuigID = @VliegtuigID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int totalSeats = connection.QueryFirst<int>(sql, new { VliegtuigID = VliegtuigID });

        return totalSeats;
    }

    public void Add(StoelModel seat)
    {
        string sql = $"INSERT INTO {Table} (VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (@VliegtuigID, @StoelNummer, @RijNummer, @StoelKlasse)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        connection.Execute(sql, seat);
    }

    public bool Update(StoelModel seat)
    {
        string sql = $"UPDATE {Table} SET VliegtuigID = @VliegtuigID, StoelNummer = @StoelNummer, RijNummer = @RijNummer, StoelKlasse = @StoelKlasse WHERE ID = @ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int rowsAffected = connection.Execute(sql, seat);

        return rowsAffected >= 1;
    }
}
