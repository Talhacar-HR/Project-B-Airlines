using Dapper;
using Microsoft.Data.Sqlite;

public class ReserveringAccess
{
    private string StoelTable = "ReserveringsStoel";

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
