using System.Data;

namespace UnitTests;

[TestClass]
public sealed class VluchtOverichtTest1
{
    [DataTestMethod]
    [DataRow(100,200)]
    [DataRow(200,100)]
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
        Assert.AreEqual(Math.Min(price1, price2), result[0].Price); // checkt laagste vlucht in prijs
        Assert.AreEqual(Math.Max(price1, price2), result[1].Price); // checkt hoogste vlucht in prijs
    }

    [DataTestMethod]
    [DataRow("09:00", "14:00")]
    [DataRow("14:00", "09:00")]

    public void SortFlightsByDeparture(string time1, string time2)
    {
        // arrange
       FlightLogic flightLogic = new();

       FlightModel flight1 = new();
       flight1.DepartureDate = "2026-04-26";
       flight1.DepartureTime = time1; // 09:00 en daarna 14:00

       FlightModel flight2 = new();
       flight2.DepartureDate = "2026-04-26";
       flight2.DepartureTime = time2; // 14:00 en daarna 09:00

      List<FlightModel> flights = [];
      flights.Add(flight1);
      flights.Add(flight2);

      // act
      List<FlightModel> result = flightLogic.SortFlights(flights, "departure");

      // assert
      Assert.AreEqual("09:00", result[0].DepartureTime);  // kijkt of 09:00 als eerst komt
      Assert.AreEqual("14:00", result[1].DepartureTime); // kijkt of 14:00 als laatst komt
    }

    [DataTestMethod]
    [DataRow("2026-12-03", "Amsterdam", "Dubai")]
    [DataRow("2026-12-10", "Dubai", "Amsterdam")]
    public void FlightFilterValidData(string departureDate, string origin, string destination)
    {
        // arrange
        FlightLogic flightLogic = new();

        // act
        List<FlightModel> result = flightLogic.FlightFilter(departureDate,origin,destination);
        // nieuwe list result krijgt antwoord van de methode

        // assert
        Assert.AreEqual(true,result.Count > 0); // checkt of er vluchten zijn

        foreach(FlightModel flight in result)
        {
            Assert.AreEqual(departureDate, flight.DepartureDate); // checkt datum
            Assert.AreEqual(origin, flight.Origin); // checkt vertrekplaats
            Assert.AreEqual(destination, flight.Destination); // checkt bestemming
        }
    }

    [DataTestMethod]
    [DataRow(1,false)]
    [DataRow(10,true)]
    public void GetAvailableFlightsTest(int flightid, bool expected)
    {
        // arrange
       FlightLogic flightLogic = new();

       // act
       List<FlightModel> result = flightLogic.GetAvailableFlights();

       // assert
       bool flag = false;

        foreach(FlightModel flight in result)
        {
            if (flight.ID == flightid)
            {
                flag = true;
            }
        }
        Assert.AreEqual(expected, flag); // is de wel of niet is gevonden
    }

    [DataTestMethod]
    [DataRow(1,false)]
    [DataRow(10,true)]

    public void SelectedFlightTest(int flightId, bool expected)
    {
        throw new NotImplementedException();
    }




}