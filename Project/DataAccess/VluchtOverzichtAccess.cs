using Microsoft.Data.Sqlite;
using Dapper;
using SQLitePCL;
using System.Data.Common;

public class FlightAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/ApplicatieDB.db");

    

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
