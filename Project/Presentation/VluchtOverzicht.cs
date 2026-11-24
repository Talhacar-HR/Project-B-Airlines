static class FlightOverview
{
    static private FlightLogic flightLogic = new FlightLogic();

    public static string Start()
    {
        string choices = "";

        Menu.PrintTitel();
        string? departureCountry = SelectMenuChoice(
            "Kies het land van vertrek:",
            CreateMenuChoices("land", ""));
        if (departureCountry == null) {return "Er zijn geen landen beschikbaar.";}

        Menu.PrintTitel();
        string? departureCity = SelectMenuChoice(
            "Kies het vliegveld van vertrek:",
            CreateMenuChoices("vliegveld", departureCountry));
        if (departureCity == null) {return $"Er zijn geen steden beschikbaar in {departureCountry}.";}
        choices += $"Plaats van vertrek: {departureCity}, {departureCountry}.\n";

        Menu.PrintTitel();
        Console.WriteLine(choices);
        string? arrivalCountry = SelectMenuChoice(
            "Kies het land van aankomst:",
            CreateMenuChoices("land", ""));
        if (arrivalCountry == null) {return "Er zijn geen landen beschikbaar.";}

        string? arrivalCity = SelectMenuChoice(
            "Kies het vliegveld van aankomst:",
            CreateMenuChoices("vliegveld", arrivalCountry));
        if (arrivalCity == null) {return $"Er zijn geen steden beschikbaar in {arrivalCountry}.";}
        choices += $"Plaats van aankomst: {arrivalCity}, {arrivalCountry}.\n";

        List<FlightModel> flights = GetSpecificFlight(departureCity, arrivalCity);

        Menu.PrintTitel();
        Console.WriteLine(choices);

        Console.WriteLine();
        if (flights.Count == 0)
        {
            return "Er zijn geen beschikbare vluchten gevonden op die datum.";
        }

        string? sortChoice = SelectMenuChoice(
            "Hoe wilt u de gevonden vluchten sorteren?",
            new List<string> { "Prijs (laag naar hoog)", "Vertrektijd (vroeg naar laat)" });
        if (sortChoice == "Prijs (laag naar hoog)")
        {
            flights = flightLogic.SortFlights(flights, "price");
        }
        else if (sortChoice == "Vertrektijd (vroeg naar laat)")
        {
            flights = flightLogic.SortFlights(flights, "departure");
        }

        Menu.PrintTitel();

        string statement = "Welke vlucht wilt u selecteren?";
        statement += "\n  Vluchtnummer | Vertrek | Bestemming | Datum | Vertrektijd | Prijs";
        statement += "\n##-------------+---------+------------+-------+-------------+------";

        List<string> allFlights = [];
        foreach (FlightModel flight in flights)
        {
            allFlights.Add($"{flight.FlightNumber} | {flight.Origin} | {flight.Destination} | {flight.DepartureDate} | {flight.DepartureTime} | €{flight.Price}");
        }

        string? flightNumberInput = SelectMenuChoice(statement, allFlights);
        if (flightNumberInput == null)
        {
            return "Er zijn geen vluchten beschikbaar.";
        }

        int selectedIndex = allFlights.IndexOf(flightNumberInput);
        if (selectedIndex < 0)
        {
            return "Ongeldige vluchtselectie.";
        }

        FlightModel selectedFlight = flights[selectedIndex];

        if (!flightLogic.SelectFlight(selectedFlight.ID))
        {
            return "Deze vlucht is niet beschikbaar.";
        }

        VluchtBoeken.Start(selectedFlight.ID, selectedFlight.FlightNumber);

        return "Uw boeking is voltooid.";
    }

    private static string? SelectMenuChoice(string title, List<string> choices)
    {
        if (choices.Count == 0)
        {
            return null;
        }

        int selectedIndex = 0;
        bool choosing = true;
        while (choosing)
        {
            Menu.PrintTitel();
            Console.WriteLine(title);

            for (int i = 0; i < choices.Count; i++)
            {
                string marker = i == selectedIndex ? "> " : "  ";
                Console.WriteLine($"{marker}{choices[i]}");
            }

            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(choices.Count - 1, selectedIndex + 1);
                    break;

                case ConsoleKey.Enter:
                    choosing = false;
                    break;
            }
        }

        return choices[selectedIndex];
    }

    static public List<string> CreateMenuChoices(string type, string keyword)
    {
        if (type == "land")
        {
            return flightLogic.GetAllCountries();
        }

        if (type == "vliegveld")
        {
            return flightLogic.GetAirportsByCountry(keyword);
        }

        return [];
    }

    private static List<FlightModel> GetSpecificFlight(string departureCity, string arrivalCity)
    {
        List<string> dates = new();
        foreach (FlightModel flight in flightLogic.GetAvailableFlights())
        {
            if (flight.Origin == departureCity &&
                flight.Destination == arrivalCity &&
                !dates.Contains(flight.DepartureDate))
            {
                dates.Add(flight.DepartureDate);
            }
        }

        string? departureDate = SelectMenuChoice("Kies de vertrekdatum:", dates);
        if (departureDate == null)
        {
            return [];
        }

        return flightLogic.FlightFilter(departureDate, departureCity, arrivalCity);
    }
}