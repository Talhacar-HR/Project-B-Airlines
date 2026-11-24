using Dapper;
using Microsoft.Data.Sqlite;

namespace UnitTests;

[TestClass]
public sealed class PrijsTests
{
    private string _connectionString = "Data Source=" + Path.Combine(AppContext.BaseDirectory, "DataSources", "ApplicatieDB.db");

    // Business-stoel 900 (toeslag 150) en economy-stoel 901 (toeslag 0) op het vliegtuig van vlucht 1
    private void ZetStoelenKlaar()
    {
        using (SqliteConnection connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            connection.Execute("INSERT OR REPLACE INTO Stoel (ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (900, 1, '1A', '1', 2)");
            connection.Execute("INSERT OR REPLACE INTO Stoel (ID, VliegtuigID, StoelNummer, RijNummer, StoelKlasse) VALUES (901, 1, '4A', '4', 1)");
        }
    }

    [TestMethod]
    [DataRow(1234.56, "€ 1.234,56")]
    [DataRow(129.99, "€ 129,99")]
    [DataRow(0, "€ 0,00")]
    public void FormatBedrag_NederlandseNotatie(double bedrag, string verwacht)
    {
        Assert.AreEqual(verwacht, PrijsLogic.FormatBedrag(bedrag));
    }

    [TestMethod]
    public void MaakBevestigingsTekst_BevatBoekingscodeStoelBagageEnTotaal()
    {
        // Arrange
        ZetStoelenKlaar();
        PrijsLogic logic = new();
        double vluchtPrijs = new VluchtLogic().GetById(1)!.Prijs;
        double totaal = (vluchtPrijs + 150 + 50) + vluchtPrijs;
        List<ReserveringsStoelModel> stoelen = new()
        {
            new ReserveringsStoelModel { PassagierID = 1, StoelID = 900, ExtraBagage = 1 },
            new ReserveringsStoelModel { PassagierID = 2, StoelID = 901, ExtraBagage = 0 }
        };

        // Act
        string tekst = logic.MaakBevestigingsTekst(1, stoelen, 123456);

        // Assert
        StringAssert.Contains(tekst, "Uw reservering is bevestigd!");
        StringAssert.Contains(tekst, "123456");
        StringAssert.Contains(tekst, "Passagier 1: stoel 1A, extra bagage: ja, " + PrijsLogic.FormatBedrag(vluchtPrijs + 150 + 50));
        StringAssert.Contains(tekst, "Passagier 2: stoel 4A, extra bagage: nee, " + PrijsLogic.FormatBedrag(vluchtPrijs));
        StringAssert.Contains(tekst, "Totaalprijs: " + PrijsLogic.FormatBedrag(totaal));
    }

    [TestMethod]
    [DataRow(false, 529.99)] // geen bagage
    [DataRow(true, 579.99)] // wel bagage, +50
    public void BerekenPrijsPerPassagier_Bagage_JuistePrijs(bool bagage, double verwacht)
    {
        // Arrange
        PrijsLogic logic = new();

        // Act
        double prijs = logic.BerekenPrijsPerPassagier(129.99, 400, bagage);

        // Assert
        Assert.AreEqual(verwacht, prijs, 0.01);
    }
}
