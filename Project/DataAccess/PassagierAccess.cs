using Microsoft.Data.Sqlite;
using Dapper;

public class PassagierAccess
{
    private static readonly string DatabaseLoc = Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

    public int Add(PassagierModel passagier)
    {
        string sql = @"
            INSERT INTO Passagier
                (GebruikerID, Naam, Achternaam, Email, PaspoortNummer, GeboorteDatum, Telefoonnummer, PaspoortVerloopDatum)
            VALUES
                (@GebruikerID, @Naam, @Achternaam, @Email, @PaspoortNummer, @GeboorteDatum, @Telefoonnummer, @PaspoortVerloopDatum)";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        connection.Execute(sql, passagier);

        int id = connection.QueryFirst<int>("SELECT last_insert_rowid()");

        return id;
    }

    public PassagierModel? GetById(int id)
    {
        string sql = @"
            SELECT ID, GebruikerID, Naam, Achternaam, Email, PaspoortNummer,
                   GeboorteDatum, Telefoonnummer, PaspoortVerloopDatum
            FROM Passagier
            WHERE ID = @Id";
        SqliteConnection connection = new($"Data Source={DatabaseLoc}");

        List<PassagierModel> passengers = connection.Query<PassagierModel>(sql, new { Id = id }).ToList();

        if (passengers.Count == 0)
            return null;

        return passengers[0];
    }
}