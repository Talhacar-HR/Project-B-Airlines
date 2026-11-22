namespace UnitTests;


[TestClass]
public sealed class PlattegrondTests
{
    // Elk vliegtuigtype heeft een eigen plattegrond met een vast aantal stoelen
    [TestMethod]
    [DataRow("737-800", 198)]   // 33 rijen x 6 stoelen
    [DataRow("A330-200", 347)]  // 3 rijen x 6 + 47 rijen x 7
    [DataRow("787-9", 243)]     // 6 rijen x 6 + 23 rijen x 9
    public void MaakStoelLijst_ElkVliegtuig_JuistAantalStoelen(string model, int verwachtAantal)
    {
        // Arrange
        PlattegrondLogic logic = new PlattegrondLogic();

        // Act
        List<StoelModel> stoelen = logic.MaakStoelLijst(1, model);

        // Assert
        Assert.AreEqual(verwachtAantal, stoelen.Count);
    }


    // De 3 vliegtuigen hebben ieder een andere indeling
    [TestMethod]
    public void GetIndeling_DrieVliegtuigen_ZijnVerschillend()
    {
        // Arrange
        PlattegrondLogic logic = new PlattegrondLogic();

        // Act
        string letters737 = logic.GetIndeling("737-800")[0].Letters;
        string letters330 = logic.GetIndeling("A330-200")[1].Letters;
        string letters787 = logic.GetIndeling("787-9")[1].Letters;

        // Assert
        Assert.AreEqual("ABC DEF", letters737);
        Assert.AreEqual("AC DFG HK", letters330);
        Assert.AreEqual("ABC DEF JKL", letters787);
    }


    // Een onbekend vliegtuig heeft geen plattegrond
    [TestMethod]
    public void GetIndeling_OnbekendModel_LegeLijst()
    {
        // Arrange
        PlattegrondLogic logic = new PlattegrondLogic();

        // Act
        IndelingStoelenModel[] indeling = logic.GetIndeling("Dummy Test");

        // Assert
        Assert.AreEqual(0, indeling.Length);
    }


    // In het gangpad (de spatie) staat geen stoel
    [TestMethod]
    public void MaakStoelLijst_Gangpad_GeenStoel()
    {
        // Arrange
        PlattegrondLogic logic = new PlattegrondLogic();

        // Act
        List<StoelModel> stoelen = logic.MaakStoelLijst(1, "737-800");

        // Assert: rij 1 heeft precies A t/m F, geen spatie
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


    // Bezette, gekozen en vrije stoelen krijgen een andere status (en dus een andere kleur)
    [TestMethod]
    [DataRow(2, "bezet")]
    [DataRow(3, "gekozen")]
    [DataRow(4, "vrij")]
    public void GetStatus_Stoel_JuisteStatus(int stoelId, string verwachteStatus)
    {
        // Arrange
        PlattegrondLogic logic = new PlattegrondLogic();
        List<int> bezet = new List<int> { 2 };
        List<int> gekozen = new List<int> { 3 };

        // Act
        string status = logic.GetStatus(stoelId, bezet, gekozen);

        // Assert
        Assert.AreEqual(verwachteStatus, status);
    }


    // Een stoel zoeken werkt ook met kleine letters en spaties
    [TestMethod]
    public void ZoekStoel_KleineLetters_StoelGevonden()
    {
        // Arrange
        PlattegrondLogic logic = new PlattegrondLogic();
        List<StoelModel> stoelen = logic.MaakStoelLijst(1, "737-800");

        // Act
        StoelModel? stoel = logic.ZoekStoel(stoelen, " 12c ");

        // Assert
        Assert.IsNotNull(stoel);
        Assert.AreEqual("12C", stoel.StoelNummer);
    }


    // Een stoel die niet in het vliegtuig zit, wordt niet gevonden
    [TestMethod]
    public void ZoekStoel_BestaatNiet_Null()
    {
        // Arrange
        PlattegrondLogic logic = new PlattegrondLogic();
        List<StoelModel> stoelen = logic.MaakStoelLijst(1, "737-800");

        // Act
        StoelModel? stoel = logic.ZoekStoel(stoelen, "99Z");

        // Assert
        Assert.IsNull(stoel);
    }
}