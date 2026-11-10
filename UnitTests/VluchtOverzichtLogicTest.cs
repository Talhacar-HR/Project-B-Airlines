namespace UnitTests;

[TestClass]
public sealed class VluchtOverichtTest1
{
    [DataTestMethod]
    [DataRow(100,200)]

   public void SortFlightsByPrice(int price1, int price2)
    {
        //arrange
        FlightLogic flightLogic = new(); // zorgt er voor dat we SortFlights kunnen aanroepen.

        FlightModel flight1 = new(); // zorgt dat we nieuwe vlucht kunnen aanmaken.
        flight1.Price = price1; // 100
        
        FlightModel flight2 = new();
        flight2.Price = price2; // 200

        List<FlightModel> flights = [];
        flights.Add(flight1);
        flights.Add(flight2);

        //act
        List<FlightModel> result = flightLogic.SortFlights(flights, "price"); // sorteert de prijs.

        // assert
        Assert.AreEqual(price1, result[0].Price); // checkt laagste vlucht in prijs
        Assert.AreEqual(price2, result[0].Price); // checkt hoogste vlucht in prijs
    }


}