using Dapper;
using Microsoft.Data.Sqlite;

public class VluchtAccess
{
    private string Table = "Vlucht";
    private static readonly string DatabaseLoc = Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

    public void Add(VluchtModel flight)
    {
        string sql = $"INSERT INTO {Table} (VliegtuigID, VertrekLocatie, AankomstLocatie, VertrekDatum, VertrekTijd, Vertraging, VluchtNummer, Prijs) VALUES (@VliegtuigID, @VertrekLocatie, @AankomstLocatie, @VertrekDatum, @VertrekTijd, @Vertraging, @VluchtNummer, @Prijs)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        connection.Execute(sql, flight);
    }

    public bool Update(VluchtModel flight)
    {
        string sql = $"UPDATE {Table} SET VliegtuigID = @VliegtuigID, VertrekLocatie = @VertrekLocatie, AankomstLocatie = @AankomstLocatie, VertrekDatum = @VertrekDatum, VertrekTijd = @VertrekTijd, Vertraging = @Vertraging, VluchtNummer = @VluchtNummer, Prijs = @Prijs WHERE ID = @ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int rowsAffected = connection.Execute(sql, flight);

        return rowsAffected >= 1;
    }

    public bool Exists(int id)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM {Table} WHERE ID = @Id)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int count = connection.QueryFirst<int>(sql, new { Id = id });

        return count > 0;
    }

    public List<VluchtModel> GetAll()
    {
        string sql = $"SELECT ID, VliegtuigID, AankomstLocatie, VertrekLocatie, VertrekDatum, VertrekTijd, Vertraging, VluchtNummer, Prijs FROM {Table} ORDER BY ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<VluchtModel> flights = connection.Query<VluchtModel>(sql).ToList();

        return flights;
    }

    public VluchtModel? GetById(int id)
    {
        string sql = $"SELECT ID, VliegtuigID, AankomstLocatie, VertrekLocatie, VertrekDatum, VertrekTijd, Vertraging, VluchtNummer, Prijs FROM {Table} WHERE ID = @Id";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<VluchtModel> flights = connection.Query<VluchtModel>(sql, new { Id = id }).ToList();

        if (flights.Count == 0)
            return null;

        return flights[0];
    }

    public bool VluchtNummerExists(int vluchtNummer, int id)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM {Table} WHERE VluchtNummer = @VluchtNummer AND ID != @Id)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int count = connection.QueryFirst<int>(sql, new { VluchtNummer = vluchtNummer, Id = id });

        return count > 0;
    }

}
