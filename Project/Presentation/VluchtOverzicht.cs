static class FlightOverview
{
    static private FlightLogic flightLogic = new FlightLogic();

    public static string Start()
    {
        string mijnKeuzes = "";

        string? departureCountry = SelectMenuChoice(
            "Kies het land van vertrek:",
            CreateMenuChoices("land", ""),
            mijnKeuzes);
        if (departureCountry == null) {return "Er zijn geen landen beschikbaar.";}
        mijnKeuzes += $"Plaats van vertrek: {departureCountry}";

        string? departureCity = SelectMenuChoice(
            "\nKies het vliegveld van vertrek:",
            CreateMenuChoices("vliegveld", departureCountry),
            mijnKeuzes);
        if (departureCity == null) {return $"Er zijn geen steden beschikbaar in {departureCountry}.";}
        mijnKeuzes += $", {departureCity}\n";

        string? arrivalCountry = SelectMenuChoice(
            "Kies het land van aankomst:",
            CreateMenuChoices("land", ""),
            mijnKeuzes);
        if (arrivalCountry == null) {return "Er zijn geen landen beschikbaar.";}
        mijnKeuzes += $"Plaats van aankomst: {arrivalCountry}";

        string? arrivalCity = SelectMenuChoice(
            "\nKies het vliegveld van aankomst:",
            CreateMenuChoices("vliegveld", arrivalCountry),
            mijnKeuzes);
        if (arrivalCity == null) {return $"Er zijn geen steden beschikbaar in {arrivalCountry}.";}
        mijnKeuzes += $", {arrivalCity}\n";

        List<FlightModel> flights = GetSpecificFlight(
            departureCity,
            arrivalCity,
            mijnKeuzes,
            out string? departureDate);

        Menu.PrintTitel();
        Console.WriteLine(mijnKeuzes);

        Console.WriteLine();
        if (flights.Count == 0)
        {
            return "Er zijn geen beschikbare vluchten gevonden op die datum.";
        }

        string? sortChoice = SelectMenuChoice(
            "Hoe wilt u de gevonden vluchten sorteren?",
            new List<string> { "Prijs (laag naar hoog)", "Vertrektijd (vroeg naar laat)" },
            mijnKeuzes);
        if (sortChoice is not null)

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
            allFlights.Add($"{flight.FlightNumber} | {flight.Origin} | {flight.Destination} | {flight.DepartureDate} | {flight.DepartureTime} | {PrijsLogic.FormatBedrag(flight.Price)}");
        }

        string? flightNumberInput = SelectMenuChoice(statement, allFlights, mijnKeuzes);
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
        mijnKeuzes += $"Datum: {selectedFlight.DepartureDate} {selectedFlight.DepartureTime}\n";
        mijnKeuzes += $"Vluchtprijs: {PrijsLogic.FormatBedrag(selectedFlight.Price)}";

        if (!flightLogic.SelectFlight(selectedFlight.ID))
        {
            return "Deze vlucht is niet beschikbaar.";
        }

        return VluchtBoeken.Start(selectedFlight.ID, selectedFlight.FlightNumber, mijnKeuzes);
    }

    private static string? SelectMenuChoice(string title, List<string> choices, string mijnKeuzes = "")
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
            if (mijnKeuzes != "")
            {
                Console.WriteLine(mijnKeuzes);
            }
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

    private static List<FlightModel> GetSpecificFlight(
        string departureCity,
        string arrivalCity,
        string mijnKeuzes,
        out string? departureDate)
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

        departureDate = SelectMenuChoice("Kies de vertrekdatum:", dates, mijnKeuzes);
        if (departureDate == null)
        {
            return [];
        }

        return flightLogic.FlightFilter(departureDate, departureCity, arrivalCity);
    }
}