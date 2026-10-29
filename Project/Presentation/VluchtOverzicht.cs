static class FlightOverview
{
    static private FlightLogic flightLogic = new FlightLogic();


    public static void Start()
    {
        List<FlightModel> flights = flightLogic.GetAvailableFlights();

        if (flights.Count == 0)
        {
            Console.WriteLine("Er zijn geen beschikbare vluchten");
        }
        else
        {
            // Kop van de tabel
            Console.WriteLine("Vluchtnummer | Vertrek | Bestemming | Datum | Vertrektijd");
            Console.WriteLine("-------------------------------------------------------");

            foreach (FlightModel flight in flights)
            {
                ShowFlight(flight);
            }
        }

        Console.WriteLine();
        Console.WriteLine("Druk op Enter om terug te gaan naar het menu");
        Console.ReadLine();
    }

    private static void ShowFlight(FlightModel flight)
    {
        Console.WriteLine(flight.FlightNumber + " | " + flight.Origin + " | " + flight.Destination + " | " + flight.DepartureDate + " | " + flight.DepartureTime);
    }
}
