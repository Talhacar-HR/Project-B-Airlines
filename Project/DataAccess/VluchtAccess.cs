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

    public bool VluchtNummerExists(int vluchtNummer, int id)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM {Table} WHERE VluchtNummer = @VluchtNummer AND ID != @Id)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int count = connection.QueryFirst<int>(sql, new { VluchtNummer = vluchtNummer, Id = id });

        return count > 0;
    }

    public List<VluchtModel> GetAll()
    {
        string sql = $"SELECT ID, VliegtuigID, AankomstLocatie, VertrekLocatie, VertrekDatum, VertrekTijd, Vertraging, VluchtNummer FROM {Table} ORDER BY ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<VluchtModel> flights = connection.Query<VluchtModel>(sql).ToList();

        return flights;
    }

    public VluchtModel? GetById(int id)
    {
        string sql = $"SELECT ID, VliegtuigID, AankomstLocatie, VertrekLocatie, VertrekDatum, VertrekTijd, Vertraging, VluchtNummer FROM {Table} WHERE ID = @Id";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<VluchtModel> flights = connection.Query<VluchtModel>(sql, new { Id = id }).ToList();

        if (flights.Count == 0)
            return null;

        return flights[0];
    }

    public List<FlightModel> GetAllFlights()
    {
        string sql = @"
            SELECT Vlucht.ID AS ID, Vlucht.VliegtuigID AS VliegtuigID,
                Vlucht.VluchtNummer AS FlightNumber,
                Vertrek.Stad AS Origin,
                Aankomst.Stad AS Destination,
                Vlucht.VertrekDatum AS DepartureDate,
                Vlucht.VertrekTijd AS DepartureTime,
                Vlucht.Prijs AS Price
            FROM Vlucht
            JOIN Vliegveld AS Vertrek ON Vlucht.VertrekLocatie = Vertrek.ID
            JOIN Vliegveld AS Aankomst ON Vlucht.AankomstLocatie = Aankomst.ID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<FlightModel> flights = connection.Query<FlightModel>(sql).ToList();

        return flights;
    }

    public FlightModel? getById(int id)
    {
        string sql = @"SELECT vlucht.ID as ID, Vlucht.VliegtuigID AS VliegtuigID, vlucht.VluchtNummer AS FlightNumber,
            vertrek.Stad AS Origin,
            Aankomst.Stad AS Destination,
            Vlucht.VertrekDatum AS DepartureDate,
            Vlucht.VertrekTijd AS DepartureTime,
            Vlucht.Prijs AS Price
            FROM vlucht
            JOIN Vliegveld AS Vertrek ON vlucht.VertrekLocatie = Vertrek.ID
            JOIN Vliegveld AS Aankomst ON Vlucht.AankomstLocatie = Aankomst.ID
            WHERE Vlucht.ID = @Id";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<FlightModel> flights = connection.Query<FlightModel>(sql, new { Id = id }).ToList();

        if (flights.Count == 0)
            return null;

        return flights[0];
    }
}
