using Dapper;
using Microsoft.Data.Sqlite;

namespace UnitTests;


[TestClass]
public sealed class PlattegrondTests
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

            connection.Execute("UPDATE Vliegtuig SET Merk = 'Boeing', Model = '737-800' WHERE ID = 1");

            connection.Execute("INSERT INTO Stoel (ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (2, 1, '1A', '1', 1)");
            connection.Execute("INSERT INTO Stoel (ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (3, 1, '1B', '1', 1)");
            connection.Execute("INSERT INTO Stoel (ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (4, 1, '1C', '1', 1)");
            connection.Execute("INSERT INTO Stoel (ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (5, 1, '2A', '2', 1)");

            connection.Execute("INSERT INTO Passagier (ID, GebruikerID, Naam, Achternaam, Email, PaspoortNummer, GeboorteDatum, Telefoonnummer, PaspoortVerloopDatum) VALUES (1, 2, 'Jan', 'Jansen', 'jan@test.nl', 'NL1111111', '1990-01-01', '0611111111', '2030-01-01')");
            connection.Execute("INSERT INTO Reservering (ID, GebruikersID, VluchtID, Status, BoekingsCode, AanmaakDatum, LaatsteWijziging) VALUES (1, 2, 7, 'Bevestigd', 100001, '2026-11-01 10:00', '2026-11-01 10:00')");
            connection.Execute("INSERT INTO ReserveringsStoel (PassagierID, ReserveringID, StoelID) VALUES (1, 1, 2)");
        }
    }

    [TestMethod]
    [DataRow("737-800", 198)]
    [DataRow("A330-200", 347)]
    [DataRow("787-9", 243)]
    public void MaakStoelLijst_ElkVliegtuig_JuistAantalStoelen(string model, int verwachtAantal)
    {
        StoelkaartLogic logic = new StoelkaartLogic();

        List<StoelModel> stoelen = logic.MaakStoelLijst(1, model);

        Assert.AreEqual(verwachtAantal, stoelen.Count);
    }


    [TestMethod]
    public void GetIndeling_DrieVliegtuigen_ZijnVerschillend()
    {
        StoelkaartLogic logic = new StoelkaartLogic();

        string letters737 = logic.GetIndeling("737-800")[0].Letters;
        string letters330 = logic.GetIndeling("A330-200")[1].Letters;
        string letters787 = logic.GetIndeling("787-9")[1].Letters;

        Assert.AreEqual("ABC DEF", letters737);
        Assert.AreEqual("AC DFG HK", letters330);
        Assert.AreEqual("ABC DEF JKL", letters787);
    }


    [TestMethod]
    public void GetIndeling_OnbekendModel_LegeLijst()
    {
        StoelkaartLogic logic = new StoelkaartLogic();

        IndelingStoelenModel[] indeling = logic.GetIndeling("Dummy Test");

        Assert.AreEqual(0, indeling.Length);
    }


    [TestMethod]
    public void MaakStoelLijst_Gangpad_GeenStoel()
    {
        StoelkaartLogic logic = new StoelkaartLogic();

        List<StoelModel> stoelen = logic.MaakStoelLijst(1, "737-800");

        List<string> rij1 = new List<string>();
        foreach (StoelModel stoel in stoelen)
        {
            if (stoel.RijNummer == "1")
            {
                rij1.Add(stoel.StoelNummer);
            }
        }

        CollectionAssert.AreEqual(new List<string> { "1A", "1B", "1C", "1D", "1E", "1F" }, rij1);
    }


    [TestMethod]
    [DataRow(2, "bezet")]
    [DataRow(3, "gekozen")]
    [DataRow(4, "vrij")]
    public void GetStatus_Stoel_JuisteStatus(int stoelId, string verwachteStatus)
    {
        StoelkaartLogic logic = new StoelkaartLogic();
        List<int> bezet = new List<int> { 2 };
        List<int> gekozen = new List<int> { 3 };

        string status = logic.GetStatus(stoelId, bezet, gekozen);

        Assert.AreEqual(verwachteStatus, status);
    }


    [TestMethod]
    public void ZoekStoel_KleineLetters_StoelGevonden()
    {
        StoelkaartLogic logic = new StoelkaartLogic();
        List<StoelModel> stoelen = logic.MaakStoelLijst(1, "737-800");

        StoelModel? stoel = logic.ZoekStoel(stoelen, " 12c ");

        Assert.IsNotNull(stoel);
        Assert.AreEqual("12C", stoel.StoelNummer);
    }


    [TestMethod]
    public void ZoekStoel_BestaatNiet_Null()
    {
        StoelkaartLogic logic = new StoelkaartLogic();
        List<StoelModel> stoelen = logic.MaakStoelLijst(1, "737-800");

        StoelModel? stoel = logic.ZoekStoel(stoelen, "99Z");

        Assert.IsNull(stoel);
    }

    [TestMethod]
    public void GetPlattegrond_Vlucht_RijenInVolgordeVanIndeling()
    {
        StoelkaartLogic logic = new StoelkaartLogic();

        List<PlattegrondRijModel> plattegrond = logic.GetPlattegrond(7);

        Assert.AreEqual(2, plattegrond.Count);
        Assert.AreEqual(1, plattegrond[0].RijNummer);
        Assert.AreEqual(2, plattegrond[1].RijNummer);

        List<string> rij1 = new List<string>();
        foreach (PlattegrondStoelModel plattegrondStoel in plattegrond[0].Stoelen)
        {
            rij1.Add(plattegrondStoel.Stoel.StoelNummer);
        }

        CollectionAssert.AreEqual(new List<string> { "1A", "1B", "1C" }, rij1);
    }


    [TestMethod]
    [DataRow(0, "Bezet")]
    [DataRow(1, "Beschikbaar")]
    [DataRow(2, "Beschikbaar")]
    public void GetPlattegrond_Stoel_JuisteStatus(int positieInRij, string verwachteStatus)
    {
        StoelkaartLogic logic = new StoelkaartLogic();

        List<PlattegrondRijModel> plattegrond = logic.GetPlattegrond(7);
        string status = plattegrond[0].Stoelen[positieInRij].Status;

        Assert.AreEqual(verwachteStatus, status);
    }


    [TestMethod]
    public void GetPlattegrond_VliegtuigZonderIndeling_LegePlattegrond()
    {
        StoelkaartLogic logic = new StoelkaartLogic();

        List<PlattegrondRijModel> plattegrond = logic.GetPlattegrond(2);

        Assert.AreEqual(0, plattegrond.Count);
    }


    [TestMethod]
    [DataRow(2, false)]
    [DataRow(3, true)]
    [DataRow(5, true)]
    [DataRow(999, false)]
    public void KanStoelKiezen_Stoel_JuisteUitkomst(int stoelId, bool verwacht)
    {
        StoelkaartLogic logic = new StoelkaartLogic();

        bool kanKiezen = logic.KanStoelKiezen(7, stoelId);

        Assert.AreEqual(verwacht, kanKiezen);
    }


    [TestMethod]
    public void KanStoelKiezen_ZelfdeStoelAndereVlucht_True()
    {
        StoelkaartLogic logic = new StoelkaartLogic();

        bool kanKiezen = logic.KanStoelKiezen(1, 2);

        Assert.IsTrue(kanKiezen);
    }
}
