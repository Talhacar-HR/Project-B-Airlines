using Microsoft.Data.Sqlite;

using Dapper;


public class FlightAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/ApplicatieDB.db");

    public List<FlightModel> GetAllFlights()
    {
        string sql = @"
            SELECT
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
}
