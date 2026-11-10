namespace UnitTests;

[TestClass]
public sealed class VluchtOverichtTest1
{
    [DataTestMethod]
    [DataRow(100,200)]

   public void SortFlightsByPrice(int price1, int price2)
    {
        FlightLogic flightLogic = new(); // zorgter voor dat we SortFlights kunnen aanroepen.

        FlightModel flight1 = new(); // zorgt dat we nieuwe vlucht kunnen aanmaken.
        flight1.Price = price1;
        
        FlightModel flight2 = new();
        flight2.Price = price2;

        List<FlightModel> flights = [];
        flights.Add(flight1);
        flights.Add(flight2);

        
        

    }


}