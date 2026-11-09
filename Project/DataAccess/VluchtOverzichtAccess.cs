using Microsoft.Data.Sqlite;
using Dapper;
using SQLitePCL;
using System.Data.Common;

public class FlightAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/ApplicatieDB.db");

    public List<FlightModel> GetAllFlights()
    {
        string sql = @"
            SELECT vlucht.ID as ID,
                Vlucht.VluchtNummer AS FlightNumber,
                Vertrek.Stad AS Origin,
                Aankomst.Stad AS Destination,
                Vlucht.VertrekDatum AS DepartureDate,
                Vlucht.VertrekTijd AS DepartureTime
            FROM Vlucht
            JOIN Vliegveld AS Vertrek ON Vlucht.VertrekLocatie = Vertrek.ID
            JOIN Vliegveld AS Aankomst ON Vlucht.AankomstLocatie = Aankomst.ID";

        return _connection.Query<FlightModel>(sql).ToList();
    }

    public FlightModel? getById(int id)
    {
        string sql = @"SELECT vlucht.ID as ID, vlucht.VluchtNummer AS FlightNumber,
        vertrek.Stad AS Origin, Aankomst.Stad AS Destination, vlucht.VertrekDatum AS DepartureDate,
        vlucht.VertrekTijd AS DepartureTime
        FROM vlucht
        Join Vliegveld AS Vertrek ON vlucht.VertrekLocatie = Vertrek.ID
        JOIN Vliegveld AS Aankomst ON Vlucht.AankomstLocatie = Aankomst.ID
        WHERE Vlucht.ID = @Id";

        return _connection.QueryFirstOrDefault<FlightModel> (sql, new {Id = id}); 
        // QueryFirstOrDefault zoekt eerste resultaat of geeft null als er niks wordt gevonden.
    }

    public bool HasAvailableSeats(int flightId)
    {
        // telt alle stoelen van het vliegtuig
        string sql = @"SELECT COUNT(*) FROM Stoel 
        JOIN Vlucht ON Stoel.VliegtuigID = Vlucht.VliegtuigID
        WHERE Vlucht.ID = @Id";
        int totaleStoelen = _connection.ExecuteScalar<int>(sql, new {Id = flightId}); // slaat aanal stoelen op. flightId wordt doorgegeven aan @Id

         // telt hoeveel verschillende stoelen zijn geresveeerd op een spcifieke (DISTINCT) vlucht.
        string sql2 = @"SELECT COUNT(DISTINCT ReserveringsStoel.StoelID) FROM ReserveringsStoel
        JOIN Reservering ON ReserveringsStoel.ReserveringID = Reservering.ID
        WHERE Reservering.VluchtID = @Id";
        int gereserveerdeStoelen = _connection.ExecuteScalar<int>(sql2, new {Id = flightId});

        if (totaleStoelen > gereserveerdeStoelen)
            return true;
        else
            return false;
        
        
    }
}
