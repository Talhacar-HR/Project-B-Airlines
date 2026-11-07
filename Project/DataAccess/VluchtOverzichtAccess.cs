using Microsoft.Data.Sqlite;
using Dapper;
using SQLitePCL;

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
                date(Vlucht.VertreksTijd) AS DepartureDate,
                strftime('%H:%M', Vlucht.VertreksTijd) AS DepartureTime
            FROM Vlucht
            JOIN Vliegveld AS Vertrek ON Vlucht.VertrekLocatie = Vertrek.ID
            JOIN Vliegveld AS Aankomst ON Vlucht.AankomstLocatie = Aankomst.ID";

        return _connection.Query<FlightModel>(sql).ToList();
    }

    public FlightModel? getById(int id)
    {
        string sql = @"Select vlucht.ID as ID, vlucht.VluchtNummer as FlightNumber, Vlucht.Prijs as Price,
        vertrek.Stad as Origin, Aankomst.Stad as Destination, date(vlucht.VertreksTijd) as DepartureDate,
        strftime('%H:%M', Vlucht.VertreksTijd) as DepartureTime 
        From vlucht
        Join Vliegveld AS Vertrek ON vlucht.VertrekLocatie = Vertrek.ID
        JOIN Vliegveld AS Aankomst ON Vlucht.AankomstLocatie = Aankomst.ID
        WHERE Vlucht.ID = @Id";

        return _connection.QueryFirstOrDefault<FlightModel> (sql, new {Id = id}); 
        // QueryFirstOrDefault zoekt eerste resultaat of geeft null als er niks wordt gevonden.
    }

    public bool HasAvailableSeats(int flightId)
    {
        throw new NotImplementedException();
    }









}

