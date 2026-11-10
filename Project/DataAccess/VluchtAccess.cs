using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

public class VluchtAccess
{
    private string Table = "Vlucht";

    public List<VluchtModel> GetAll()
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID, VliegtuigID, AankomstLocatie, VertrekLocatie, VertrekDatum, VertrekTijd, Vertraging, VluchtNummer FROM {Table} ORDER BY ID";
            return connection.Query<VluchtModel>(sql).AsList();
        }
    }

     public List<FlightModel> GetAllFlights()
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();
        string sql = @"
            SELECT Vlucht.ID AS ID, Vlucht.VliegtuigID AS VliegtuigID,
                Vlucht.VluchtNummer AS FlightNumber,
                Vertrek.Stad AS Origin,
                Aankomst.Stad AS Destination,
                Vlucht.VertrekDatum AS DepartureDate,
                Vlucht.VertrekTijd AS DepartureTime,
                Vlucht.Prijs As Price
            FROM Vlucht
            JOIN Vliegveld AS Vertrek ON Vlucht.VertrekLocatie = Vertrek.ID
            JOIN Vliegveld AS Aankomst ON Vlucht.AankomstLocatie = Aankomst.ID";

        return connection.Query<FlightModel>(sql).ToList();
        }
        
    }

    public FlightModel? getById(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();
            string sql = @"SELECT vlucht.ID as ID, Vlucht.VliegtuigID AS VliegtuigID, vlucht.VluchtNummer AS FlightNumber,
        vertrek.Stad AS Origin, 
        Aankomst.Stad AS Destination, 
        Vlucht.VertrekDatum AS DepartureDate,
        Vlucht.VertrekTijd AS DepartureTime,
        Vlucht.Prijs AS Price
        FROM vlucht
        Join Vliegveld AS Vertrek ON vlucht.VertrekLocatie = Vertrek.ID
        JOIN Vliegveld AS Aankomst ON Vlucht.AankomstLocatie = Aankomst.ID
        WHERE Vlucht.ID = @Id";

        return connection.QueryFirstOrDefault<FlightModel> (sql, new {Id = id}); 
        // QueryFirstOrDefault zoekt eerste resultaat of geeft null als er niks wordt gevonden.
        }
        
    }

    public void Add(VluchtModel flight)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"INSERT INTO {Table} (VliegtuigID, VertrekLocatie, AankomstLocatie, VertrekDatum, VertrekTijd, Vertraging, VluchtNummer) VALUES (@VliegtuigID, @VertrekLocatie, @AankomstLocatie, @VertrekDatum, @VertrekTijd, @Vertraging, @VluchtNummer)";
            connection.Execute(sql, flight);
        }
    }

    public bool Update(VluchtModel flight)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"UPDATE {Table} SET VliegtuigID = @VliegtuigID, VertrekLocatie = @VertrekLocatie, AankomstLocatie = @AankomstLocatie, VertrekDatum = @VertrekDatum, VertrekTijd = @VertrekTijd, Vertraging = @Vertraging, VluchtNummer = @VluchtNummer WHERE ID = @ID";
            return connection.Execute(sql, flight) > 0;
        }
    }
}
