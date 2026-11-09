using Dapper;
using Microsoft.Data.Sqlite;

public class ReserveringAccess
{
    public void Add(ReserveringModel reservering)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            // Een transaction zorgt ervoor dat alles samen wordt opgeslagen.
            // Gaat er iets mis, dan wordt er niets opgeslagen.
            using (SqliteTransaction transaction = connection.BeginTransaction())
            {
                // 1. Sla de reservering op en vraag het nieuwe ID op
                string sqlReservering = @"
                    INSERT INTO Reservering (GebruikersID, VluchtID, Status, BoekingsCode, AanmaakDatum, LaatsteWijziging)
                    VALUES (@GebruikersID, @VluchtID, @Status, @BoekingsCode, @AanmaakDatum, @LaatsteWijziging);
                    SELECT last_insert_rowid();";

                int reserveringId = connection.ExecuteScalar<int>(sqlReservering, reservering, transaction);

                // 2. Sla elke stoel op met het ID van de reservering
                string sqlStoel = @"
                    INSERT INTO ReserveringsStoel (PassagierID, ReserveringID, StoelID, ExtraBagage)
                    VALUES (@PassagierID, @ReserveringID, @StoelID, @ExtraBagage)";

                foreach (ReserveringsStoelModel stoel in reservering.Stoelen)
                {
                    stoel.ReserveringID = reserveringId;
                    connection.Execute(sqlStoel, stoel, transaction);
                }

                // 3. Alles is gelukt, dus nu echt opslaan
                transaction.Commit();
            }
        }
    }

    public List<ReserveringModel> GetByGebruikerId(int gebruikerId)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = @"
                SELECT ID, GebruikersID, VluchtID, Status, BoekingsCode, AanmaakDatum, LaatsteWijziging
                FROM Reservering
                WHERE GebruikersID = @GebruikersID";

            return connection.Query<ReserveringModel>(sql, new { GebruikersID = gebruikerId }).AsList();
        }
    }

    public List<int> GetBezetteStoelIds(int vluchtId)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            // Alleen stoelen van deze vlucht, en alleen van reserveringen die niet geannuleerd zijn
            string sql = @"
                SELECT ReserveringsStoel.StoelID
                FROM ReserveringsStoel
                JOIN Reservering ON ReserveringsStoel.ReserveringID = Reservering.ID
                WHERE Reservering.VluchtID = @VluchtID
                AND Reservering.Status != 'Geannuleerd'";

            return connection.Query<int>(sql, new { VluchtID = vluchtId }).AsList();
        }
    }

    public bool BestaatBoekingsCode(int boekingsCode)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT COUNT(*) FROM Reservering WHERE BoekingsCode = @BoekingsCode";
            int aantal = connection.ExecuteScalar<int>(sql, new { BoekingsCode = boekingsCode });

            return aantal > 0;
        }
    }

    public List<ReserveringsStoelModel> GetStoelenByReserveringId(int reserveringId)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = @"
                SELECT ID, PassagierID, ReserveringID, StoelID, ExtraBagage
                FROM ReserveringsStoel
                WHERE ReserveringID = @ReserveringID";

            return connection.Query<ReserveringsStoelModel>(sql, new { ReserveringID = reserveringId }).AsList();
        }
    }
}
