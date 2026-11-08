using Dapper;
using Microsoft.Data.Sqlite;

public class ReserveringAccess
{
    private string Table = "Reservering";
    private string StoelTable = "ReserveringsStoel";

    // Haalt 1 reservering op, of null als die niet bestaat.
    public ReserveringModel? GetById(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID, GebruikersID, VluchtID, Status, BoekingsCode, AanmaakDatum, LaatsteWijziging FROM {Table} WHERE ID = @ID";
            return connection.QueryFirstOrDefault<ReserveringModel>(sql, new { ID = id });
        }
    }

    // Haalt alle stoelen (met passagier en extra bagage) op die bij 1 reservering horen.
    // Hier geen berekeningen: prijs + toeslag + bagagekosten gaat in de logic layer.
    public List<ReserveringsStoelModel> GetStoelenByReserveringId(int reserveringId)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = $"SELECT ID, PassagierID, ReserveringID, StoelID, ExtraBagage FROM {StoelTable} WHERE ReserveringID = @ReserveringID";
            return connection.Query<ReserveringsStoelModel>(sql, new { ReserveringID = reserveringId }).AsList();
        }
    }
}
