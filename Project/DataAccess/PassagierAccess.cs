using Microsoft.Data.Sqlite;
using Dapper;

public class PassagierAccess
{
    public int Add(PassagierModel passagier)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = @"
                INSERT INTO Passagier
                    (GebruikerID, Naam, Achternaam, Email, PaspoortNummer, GeboorteDatum, Telefoonnummer, PaspoortVerloopDatum)
                VALUES
                    (@GebruikerID, @Naam, @Achternaam, @Email, @PaspoortNummer, @GeboorteDatum, @Telefoonnummer, @PaspoortVerloopDatum);
                SELECT last_insert_rowid();";

            return connection.QuerySingle<int>(sql, passagier);
        }
    }

    public PassagierModel? GetById(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(DatabaseLocation.ConnectionString))
        {
            connection.Open();

            string sql = @"
                SELECT ID, GebruikerID, Naam, Achternaam, Email, PaspoortNummer,
                       GeboorteDatum, Telefoonnummer, PaspoortVerloopDatum
                FROM Passagier
                WHERE ID = @Id";

            return connection.QueryFirstOrDefault<PassagierModel>(sql, new { Id = id });
        }
    }
}