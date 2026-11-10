using Dapper;
using Microsoft.Data.Sqlite;

public class ReserveringAccess
{
    private static readonly string DatabaseLoc = Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

    public void Add(ReserveringModel reservering)
    {
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");
        connection.Open();

        // Een transaction zorgt ervoor dat alles samen wordt opgeslagen.
        // Gaat er iets mis, dan wordt er niets opgeslagen.
        using (SqliteTransaction transaction = connection.BeginTransaction())
        {
            // Sla de reservering op en vraag het nieuwe ID op
            string sqlReservering = @"
                INSERT INTO Reservering (GebruikersID, VluchtID, Status, BoekingsCode, AanmaakDatum, LaatsteWijziging)
                VALUES (@GebruikersID, @VluchtID, @Status, @BoekingsCode, @AanmaakDatum, @LaatsteWijziging)";

            connection.Execute(sqlReservering, reservering, transaction);
            int reserveringId = connection.QueryFirst<int>("SELECT last_insert_rowid()", transaction: transaction);

            // Sla elke stoel op met het ID van de reservering
            string sqlStoel = @"
                INSERT INTO ReserveringsStoel (PassagierID, ReserveringID, StoelID)
                VALUES (@PassagierID, @ReserveringID, @StoelID)";

            foreach (ReserveringsStoelModel stoel in reservering.Stoelen)
            {
                stoel.ReserveringID = reserveringId;
                connection.Execute(sqlStoel, stoel, transaction);
            }

            // Alles is gelukt, dus nu echt opslaan
            transaction.Commit();
        }
    }

    public List<ReserveringModel> GetByGebruikerId(int gebruikerId)
    {
        string sql = @"
            SELECT ID, GebruikersID, VluchtID, Status, BoekingsCode, AanmaakDatum, LaatsteWijziging
            FROM Reservering
            WHERE GebruikersID = @GebruikersID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<ReserveringModel> reservations = connection.Query<ReserveringModel>(sql, new { GebruikersID = gebruikerId }).ToList();

        return reservations;
    }

    public List<int> GetBezetteStoelIds(int vluchtId)
    {
        // Alleen stoelen van deze vlucht, en alleen van reserveringen die niet geannuleerd zijn
        string sql = @"
            SELECT ReserveringsStoel.StoelID
            FROM ReserveringsStoel
            JOIN Reservering ON ReserveringsStoel.ReserveringID = Reservering.ID
            WHERE Reservering.VluchtID = @VluchtID
            AND Reservering.Status != 'Geannuleerd'";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<int> seatIds = connection.Query<int>(sql, new { VluchtID = vluchtId }).ToList();

        return seatIds;
    }

    public bool BestaatBoekingsCode(int boekingsCode)
    {
        string sql = "SELECT COUNT(*) FROM Reservering WHERE BoekingsCode = @BoekingsCode";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        int aantal = connection.QueryFirst<int>(sql, new { BoekingsCode = boekingsCode });

        return aantal > 0;
    }

    public List<ReserveringsStoelModel> GetStoelenByReserveringId(int reserveringId)
    {
        string sql = @"
            SELECT ID, PassagierID, ReserveringID, StoelID
            FROM ReserveringsStoel
            WHERE ReserveringID = @ReserveringID";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<ReserveringsStoelModel> seats = connection.Query<ReserveringsStoelModel>(sql, new { ReserveringID = reserveringId }).ToList();

        return seats;
    }
}
