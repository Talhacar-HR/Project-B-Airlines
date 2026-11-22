static class FlightOverview
{
    static private FlightLogic flightLogic = new FlightLogic();

    private static void ShowFlight(FlightModel flight)
    {
        Console.WriteLine(flight.FlightNumber + " | " + flight.Origin + " | " + flight.Destination + " | " + flight.DepartureDate + " | " + flight.DepartureTime);
    }

    public static void Start()
    {
        // temp test
        List<FlightModel> availableFlights = flightLogic.GetAvailableFlights();
        Console.WriteLine("Beschikbare vluchten:");
        Console.WriteLine("Vluchtnummer | Vertrek | Bestemming | Datum | Vertrektijd");
        Console.WriteLine("---------------------------------------------------------");
        foreach (FlightModel flight in availableFlights)
        {
            ShowFlight(flight);
        }
        // temp test

        // Elk veld wordt meteen gecontroleerd en opnieuw gevraagd tot het goed is
        string departureDate = Invoer.VraagTotGeldig("Vertrekdatum (dd-mm-jjjj): ", flightLogic.IsValidZoekDatum,
            "Ongeldige datum, gebruik dd-mm-jjjj (bijvoorbeeld 15-12-2026) en geen datum in het verleden.");
        string origin = Invoer.VraagTotGeldig("Vertrekplaats: ", flightLogic.IsValidLocatie,
            "Vul een vertrekplaats in.");
        string destination = Invoer.VraagTotGeldig("Bestemming: ", flightLogic.IsValidLocatie,
            "Vul een bestemming in.");

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
            Console.WriteLine("Welk vluchtnummer wil je selecteren?");
            string flightNumberInput = Console.ReadLine() ?? "";
            if (!long.TryParse(flightNumberInput, out long flightNumber))
            {
                Console.WriteLine("Ongeldig vluchtnummer.");
                return;
            }

            FlightModel? selectedFlight = flights.FirstOrDefault(flight => flight.FlightNumber == flightNumber);
            if (selectedFlight == null)
            {
                Console.WriteLine("Ongeldig vluchtnummer.");
                return;
            }

            if (!flightLogic.SelectFlight(selectedFlight.ID))
            {
                Console.WriteLine("Deze vlucht is niet beschikbaar.");
                return;
            }

            // ???
            VluchtBoeken.Start(selectedFlight.ID, selectedFlight.FlightNumber);
        }
    }
}
