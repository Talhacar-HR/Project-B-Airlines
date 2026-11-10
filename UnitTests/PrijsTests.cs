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
}
