using Dapper;
using Microsoft.Data.Sqlite;

public class VluchtOverzichtAccess
{
    private static readonly string DatabaseLoc = Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

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
        using SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        return connection.Query<FlightModel>(sql).ToList();
    }

    public FlightModel? GetById(int id)
    {
        string sql = @"
            SELECT vlucht.ID as ID, Vlucht.VliegtuigID AS VliegtuigID,
                vlucht.VluchtNummer AS FlightNumber,
                Vertrek.Stad AS Origin,
                Aankomst.Stad AS Destination,
                Vlucht.VertrekDatum AS DepartureDate,
                Vlucht.VertrekTijd AS DepartureTime,
                Vlucht.Prijs AS Price
            FROM vlucht
                JOIN Vliegveld AS Vertrek ON vlucht.VertrekLocatie = Vertrek.ID
                JOIN Vliegveld AS Aankomst ON Vlucht.AankomstLocatie = Aankomst.ID
            WHERE Vlucht.ID = @Id";
        using SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        return connection.QueryFirstOrDefault<FlightModel>(sql, new { Id = id });
    }

    public List<string> GetAllCountries()
    {
        string sql = @"
            SELECT DISTINCT Land
            FROM Vliegveld
            WHERE Land IS NOT NULL AND TRIM(Land) != ''
            ORDER BY Land";
        using SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        return connection.Query<string>(sql).ToList();
    }

    public List<string> GetAirportsByCountry(string country)
    {
        string sql = @"
            SELECT DISTINCT Stad
            FROM Vliegveld
            WHERE Land = @Land AND Stad IS NOT NULL AND TRIM(Stad) != ''
            ORDER BY Stad";
        using SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        return connection.Query<string>(sql, new { Land = country }).ToList();
    }
}
