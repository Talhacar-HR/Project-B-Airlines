using Dapper;
using Microsoft.Data.Sqlite;

namespace UnitTests;


[TestClass]
public sealed class ReserveringTests
{
    private string _connectionString = "Data Source=" + Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

    [TestInitialize]
    public void ZetTestDataKlaar()
    {
        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            connection.Execute("DELETE FROM ReserveringsStoel");
            connection.Execute("DELETE FROM Reservering");
            connection.Execute("DELETE FROM Passagier");
            connection.Execute("DELETE FROM Stoel");

            connection.Execute("INSERT INTO Stoel (ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (2, 1, 'A', '1', 1)");
            connection.Execute("INSERT INTO Stoel (ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (3, 1, 'B', '1', 1)");
            connection.Execute("INSERT INTO Stoel (ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (4, 1, 'A', '20', 1)");
            connection.Execute("INSERT INTO Stoel (ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (5, 1, 'B', '20', 1)");

            connection.Execute("INSERT INTO Passagier (ID, GebruikerID, Naam, Achternaam, Email, PaspoortNummer, GeboorteDatum, Telefoonnummer, PaspoortVerloopDatum) VALUES (1, 2, 'Jan', 'Jansen', 'jan@test.nl', 'NL1111111', '1990-01-01', '0611111111', '2030-01-01')");
            connection.Execute("INSERT INTO Passagier (ID, GebruikerID, Naam, Achternaam, Email, PaspoortNummer, GeboorteDatum, Telefoonnummer, PaspoortVerloopDatum) VALUES (2, 2, 'Deniz', 'Yilmaz', 'deniz@outlook.com', 'NL2222222', '1995-05-05', '0622222222', '2031-05-05')");

            connection.Execute("INSERT INTO Reservering (ID, GebruikersID, VluchtID, Status, BoekingsCode, AanmaakDatum, LaatsteWijziging) VALUES (1, 2, 7, 'Bevestigd', 100001, '2026-11-01 10:00', '2026-11-01 10:00')");

            connection.Execute("INSERT INTO ReserveringsStoel (PassagierID, ReserveringID, StoelID) VALUES (1, 1, 2)");
            connection.Execute("INSERT INTO ReserveringsStoel (PassagierID, ReserveringID, StoelID) VALUES (2, 1, 3)");
        }
    }


    [TestMethod]
    public void H1_NieuweReserveringWordtOpgeslagen()
    {
        ReserveringAccess access = new ReserveringAccess();

        ReserveringModel reservering = new ReserveringModel();
        reservering.GebruikersID = 2;
        reservering.VluchtID = 7;
        reservering.Status = "Bevestigd";
        reservering.BoekingsCode = 100002;
        reservering.AanmaakDatum = "2026-11-09 12:00";
        reservering.LaatsteWijziging = "2026-11-09 12:00";
        reservering.Stoelen.Add(new ReserveringsStoelModel(0, 1, 0, 4));

        access.Add(reservering);

        List<ReserveringModel> reserveringen = new ReserveringAccess().GetByGebruikerId(2);

        bool gevonden = false;
        foreach (ReserveringModel r in reserveringen)
        {
            if (r.BoekingsCode == 100002 && r.VluchtID == 7)
            {
                gevonden = true;
            }
        }

        Assert.IsTrue(gevonden);
    }


    [TestMethod]
    public void H3_GereserveerdeStoelenZijnBezet()
    {
        ReserveringAccess access = new ReserveringAccess();
        List<int> bezetteStoelen = access.GetBezetteStoelIds(7);

        Assert.IsTrue(bezetteStoelen.Contains(2));
        Assert.IsTrue(bezetteStoelen.Contains(3));
        Assert.IsFalse(bezetteStoelen.Contains(4));
        Assert.IsFalse(bezetteStoelen.Contains(5));
    }


   [TestMethod]
    public void H4_AllePassagiersEnStoelenBlijvenBewaard()
    {
        ReserveringAccess access = new ReserveringAccess();
        List<ReserveringsStoelModel> stoelen = access.GetStoelenByReserveringId(1);

        Assert.AreEqual(2, stoelen.Count);

        bool janOpStoel2 = false;
        bool denizOpStoel3 = false;
        foreach (ReserveringsStoelModel stoel in stoelen)
        {
            if (stoel.PassagierID == 1 && stoel.StoelID == 2)
            {
                janOpStoel2 = true;
            }
            if (stoel.PassagierID == 2 && stoel.StoelID == 3)
            {
                denizOpStoel3 = true;
            }
        }

        Assert.IsTrue(janOpStoel2);
        Assert.IsTrue(denizOpStoel3);
    }

    [TestMethod]
    public void S2_BezetteStoelWordtGeweigerd()
    {
        AccountsLogic accountsLogic = new AccountsLogic();
        accountsLogic.CheckLogin("deniz@outlook.com", "denizbaba");

        ReserveringLogic logic = new ReserveringLogic();
        int aantalVoor = new ReserveringAccess().GetByGebruikerId(2).Count;

        List<ReserveringsStoelModel> stoelen = new List<ReserveringsStoelModel>();
        stoelen.Add(new ReserveringsStoelModel(0, 1, 0, 2));

        bool gelukt = logic.MaakReservering(7, stoelen);

        int aantalNa = new ReserveringAccess().GetByGebruikerId(2).Count;

        Assert.IsFalse(gelukt);
        Assert.AreEqual(aantalVoor, aantalNa);
    }

    [TestMethod]
    public void S3_ZelfdeStoelOpAndereVluchtIsVrij()
    {
        ReserveringLogic logic = new ReserveringLogic();

        bool bezetOpVlucht1 = logic.IsStoelBezet(1, 2);
        bool bezetOpVlucht7 = logic.IsStoelBezet(7, 2);

        Assert.IsFalse(bezetOpVlucht1);
        Assert.IsTrue(bezetOpVlucht7);
    }
}