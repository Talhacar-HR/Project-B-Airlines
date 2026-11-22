static class TestFile
{
    static private FlightLogic flightLogic = new FlightLogic();

    public static string Start()
    {
        Menu.PrintTitel();

        List<string> countries = CreateMenuChoices("land", "");
        if (countries.Count == 0)
        {
            return "Er zijn geen landen beschikbaar.";
        }

        Console.WriteLine("Beschikbare landen:");
        foreach (string country in countries)
        {
            Console.WriteLine(country);
        }

        Console.Write("Voer een land in om de steden te tonen: ");
        string countryKeyword = Console.ReadLine() ?? "";
        List<string> cities = CreateMenuChoices("stad", countryKeyword);

        if (cities.Count == 0)
        {
            return $"Geen steden gevonden voor land '{countryKeyword}'.";
        }

        return $"Steden in {countryKeyword.Trim()}:{Environment.NewLine}{string.Join(Environment.NewLine, cities)}";
    }

    static public List<string> CreateMenuChoices(string type, string keyword)
    {
        if (type == "land")
        {
            return flightLogic.GetAllCountries();
        }

        if (type == "stad")
        {
            return flightLogic.GetCitiesByCountry(keyword);
        }

        return [];
    }
}