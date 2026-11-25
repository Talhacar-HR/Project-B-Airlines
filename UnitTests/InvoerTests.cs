namespace UnitTests;

[TestClass]
public sealed class InvoerTests
{
    [TestMethod]
    [DataRow("jan@test.nl", true)] // goed
    [DataRow("jantest.nl", false)] // geen @
    [DataRow("jan@test", false)] // geen punt na @
    [DataRow("jan@@test.nl", false)] // twee keer @
    [DataRow("jan @test.nl", false)] // spatie
    [DataRow("", false)] // leeg
    public void IsValidEmail_Invoer_JuisteUitkomst(string email, bool verwacht)
    {
        // Arrange
        UserLogic logic = new();

        // Act
        bool uitkomst = logic.IsValidEmail(email);

        // Assert
        Assert.AreEqual(verwacht, uitkomst);
    }

    [TestMethod]
    [DataRow("geheim1", true)] // 7 tekens
    [DataRow("abc123", true)] // precies 6
    [DataRow("abc12", false)] // te kort
    [DataRow("geheim 1", false)] // spatie
    [DataRow("", false)] // leeg
    public void IsValidWachtwoord_Invoer_JuisteUitkomst(string wachtwoord, bool verwacht)
    {
        // Arrange
        UserLogic logic = new();

        // Act
        bool uitkomst = logic.IsValidWachtwoord(wachtwoord);

        // Assert
        Assert.AreEqual(verwacht, uitkomst);
    }

    [TestMethod]
    [DataRow("admin@admin.com", false)] // bestaat al
    [DataRow("bestaatniet123@test.nl", true)] // nog vrij
    public void IsEmailBeschikbaar_Email_JuisteUitkomst(string email, bool verwacht)
    {
        // Arrange
        UserLogic logic = new();

        // Act
        bool uitkomst = logic.IsEmailBeschikbaar(email);

        // Assert
        Assert.AreEqual(verwacht, uitkomst);
    }

    [TestMethod]
    public void IsValidPassagier_AlleGegevensGoed_True()
    {
        // Arrange
        PassagierLogic logic = new();
        PassagierModel passagier = MaakPassagier();

        // Act
        bool uitkomst = logic.IsValidPassagier(passagier);

        // Assert
        Assert.IsTrue(uitkomst);
    }

    [TestMethod]
    [DataRow("", "jan@test.nl", "0612345678")] // voornaam leeg
    [DataRow("Jan", "", "0612345678")] // e-mail leeg
    [DataRow("Jan", "jan@test.nl", "")] // telefoonnummer leeg
    public void IsValidPassagier_VeldLeeg_False(string naam, string email, string telefoon)
    {
        // Arrange
        PassagierLogic logic = new();
        PassagierModel passagier = MaakPassagier();
        passagier.Naam = naam;
        passagier.Email = email;
        passagier.Telefoonnummer = telefoon;

        // Act
        bool uitkomst = logic.IsValidPassagier(passagier);

        // Assert
        Assert.IsFalse(uitkomst);
    }

    [TestMethod]
    public void IsValidPassagier_Null_False()
    {
        // Arrange
        PassagierLogic logic = new();

        // Act
        bool uitkomst = logic.IsValidPassagier(null!);

        // Assert
        Assert.IsFalse(uitkomst);
    }

    // Een passagier waarbij alles goed is ingevuld
    private static PassagierModel MaakPassagier()
    {
        return new PassagierModel
        {
            GebruikerID = 2,
            Naam = "Jan",
            Achternaam = "Jansen",
            Email = "jan@test.nl",
            PaspoortNummer = "NL1234567",
            GeboorteDatum = "12-05-1990",
            Telefoonnummer = "0612345678",
            PaspoortVerloopDatum = "01-01-2035"
        };
    }
}
