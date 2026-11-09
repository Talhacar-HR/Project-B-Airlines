namespace UnitTests;

[TestClass]
public sealed class PrijsTests
{
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

    [TestMethod]
    [DataRow(1, 859.98)] // reservering uit testdata
    [DataRow(9999, 0)] // bestaat niet
    public void BerekenTotaalprijs_Reservering_JuisteTotaal(int id, double verwacht)
    {
        // Arrange
        PrijsLogic logic = new();

        // Act
        double totaal = logic.BerekenTotaalprijs(id);

        // Assert
        Assert.AreEqual(verwacht, totaal, 0.01);
    }
}
