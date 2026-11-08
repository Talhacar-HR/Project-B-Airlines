static class FlightOverview
{
    static private FlightLogic flightLogic = new FlightLogic();

    private static void ShowFlight(FlightModel flight)
    {
        Console.WriteLine(flight.FlightNumber + " | " + flight.Origin + " | " + flight.Destination + " | " + flight.DepartureDate + " | " + flight.DepartureTime);
    }

    public static void Start()
    {
        Console.WriteLine("Vertrekdatum (yyyy-MM-dd): ");
        var departureDate = Console.ReadLine(); // var omdat het ook null kan terugegeven als ik string gebruik.

        Console.WriteLine("Vertrekplaats: ");
        var origin = Console.ReadLine();

        Console.WriteLine("Bestemming: ");
        var destination = Console.ReadLine();

        if (departureDate == null || origin == null || destination == null)
        {
            Console.WriteLine("Ongeldige invoer.");
            return;
        }

        List<FlightModel> flights = flightLogic.FlightFilter(departureDate, origin, destination);

        Console.WriteLine();
        if (flights.Count == 0)
        {
            Console.WriteLine("Er zijn geen beschikbare vluchten");
        }
        else
        {
            Console.WriteLine("Hoe wil je sorteren?");
            Console.WriteLine("1. Prijs (laag naar hoog)");
            Console.WriteLine("2. Vertrektijd (vroeg naar laat)");

            var keuze  = Console.ReadLine();

            if (keuze == "1")
                flights = flightLogic.SortFlights(flights, "price");
            else if (keuze == "2")
                flights = flightLogic.SortFlights(flights, "departure");

            // Kop van de tabel
            Console.WriteLine("Vluchtnummer | Vertrek | Bestemming | Datum | Vertrektijd");
            Console.WriteLine("-------------------------------------------------------");

            foreach (FlightModel flight in flights)
            {
                ShowFlight(flight);
            }
        }
    }
}
