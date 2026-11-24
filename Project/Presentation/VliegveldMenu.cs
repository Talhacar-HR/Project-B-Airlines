static class VliegveldMenu
{
    static private VliegveldLogic vliegveldLogic = new VliegveldLogic();

    public static void Start()
    {
        string feedback = "";

        bool menuRunning = true;
        while (menuRunning)
        {
            string menuInput = MenuOptions(feedback);

            if (menuInput == "1")
                feedback = Overzicht();
            else if (menuInput == "2")
                feedback = Toevoegen();
            else if (menuInput == "3")
                feedback = Aanpassen();
            else if (menuInput == "0")
                menuRunning = false;
            else
                Console.WriteLine("Ongeldige keuze.");
        }
    }

    public static string MenuOptions(string feedback)
    {
        string[] menuOptions = { "1 - Overzicht", "2 - Toevoegen", "3 - Aanpassen", "0 - Terug" };
        int selectedIndex = 0;

        while (true)
        {
            Menu.PrintTitel();

            if (feedback != "")
                Console.WriteLine(feedback);
                Console.WriteLine("");

            Console.WriteLine("Vliegvelden beheren");
            for (int i = 0; i < menuOptions.Length; i++)
            {
                string marker = i == selectedIndex ? "> " : "  ";
                Console.WriteLine($"{marker}{menuOptions[i]}");
            }

            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(menuOptions.Length - 1, selectedIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    return menuOptions[selectedIndex][0].ToString();
                case ConsoleKey.Escape:
                    return "0";
            }
        }
    }

    public static string Overzicht()
    {
        List<VliegveldModel> airports = vliegveldLogic.GetAll();
        if (airports.Count == 0)
            return "Geen vliegvelden gevonden";

        string allAirports = "";
        foreach (VliegveldModel airport in airports)
            allAirports += $"ID: {airport.ID}, Naam: {airport.Naam}, Stad: {airport.Stad}, Land: {airport.Land}, Afkorting: {airport.Afkorting}\n";
        return allAirports;
    }

    public static string Toevoegen()
    {
        string naam = Invoer.VraagTotGeldig(
            "Vliegveld naam: ",
            vliegveldLogic.IsValidNaam,
            "Naam is verplicht.");
        string stad = Invoer.VraagTotGeldig(
            "Stad: ",
            vliegveldLogic.IsValidStad,
            "Stad is verplicht.");
        string land = Invoer.VraagTotGeldig(
            "Land: ",
            vliegveldLogic.IsValidLand,
            "Land is verplicht.");
        string afkorting = Invoer.VraagTotGeldig(
            "Afkorting: ",
            vliegveldLogic.IsValidAfkorting,
            "Afkorting is verplicht.");

        if (CheckAirport(0, naam, stad, land, afkorting, out VliegveldModel airport))
        {
            vliegveldLogic.Add(airport);
            return "Vliegveld toegevoegd.";
        }
        return "Vliegveld niet toegevoegd; er ging iets fout.";
    }

    public static string Aanpassen()
    {
        VliegveldModel? currentAirport = KeuzeMenu.KiesVliegveld("Kies het vliegveld");
        if (currentAirport is null)
        {
            return "Geen vliegveld gekozen; er is niets aangepast.";
        }
        int id = currentAirport.ID;

        ToonVliegveld(currentAirport);
        Console.WriteLine("Laat een veld leeg als u de huidige invoer wilt gebruiken.");

        string naam = Invoer.VraagTotGeldig(
            $"Nieuwe naam [{currentAirport.Naam}]: ",
            input => string.IsNullOrWhiteSpace(input) || vliegveldLogic.IsValidNaam(input),
            "Naam is verplicht.");
        if (string.IsNullOrWhiteSpace(naam)) naam = currentAirport.Naam;

        string stad = Invoer.VraagTotGeldig(
            $"Nieuwe stad [{currentAirport.Stad}]: ",
            input => string.IsNullOrWhiteSpace(input) || vliegveldLogic.IsValidStad(input),
            "Stad is verplicht.");
        if (string.IsNullOrWhiteSpace(stad)) stad = currentAirport.Stad;

        string land = Invoer.VraagTotGeldig(
            $"Nieuw land [{currentAirport.Land}]: ",
            input => string.IsNullOrWhiteSpace(input) || vliegveldLogic.IsValidLand(input),
            "Land is verplicht.");
        if (string.IsNullOrWhiteSpace(land)) land = currentAirport.Land;

        string afkorting = Invoer.VraagTotGeldig(
            $"Nieuwe afkorting [{currentAirport.Afkorting}]: ",
            input => string.IsNullOrWhiteSpace(input) || vliegveldLogic.IsValidAfkorting(input),
            "Afkorting is verplicht.");
        if (string.IsNullOrWhiteSpace(afkorting)) afkorting = currentAirport.Afkorting;

        if (!CheckAirport(id, naam, stad, land, afkorting, out VliegveldModel airport))
            return "Vliegveld niet aangepast; er is iets foutgegaan tijdens het checken.";

        if (vliegveldLogic.Update(airport))
        {
            return "Vliegveld aangepast.";
        }
        else
        {
            return $"Er bestaat geen vliegveld {id}.";
        }
    }

    private static bool CheckAirport(int id, string naam, string stad, string land, string afkorting, out VliegveldModel airport)
    {
        bool isValid = true;

        if (!vliegveldLogic.IsValidNaam(naam))
        {
            Console.WriteLine("Naam is verplicht.");
            isValid = false;
        }

        if (!vliegveldLogic.IsValidStad(stad))
        {
            Console.WriteLine("Stad is verplicht.");
            isValid = false;
        }

        if (!vliegveldLogic.IsValidLand(land))
        {
            Console.WriteLine("Land is verplicht.");
            isValid = false;
        }

        if (!vliegveldLogic.IsValidAfkorting(afkorting))
        {
            Console.WriteLine("Afkorting is verplicht.");
            isValid = false;
        }

        airport = new VliegveldModel(id, naam, stad, land, afkorting);
        return isValid;
    }
    private static void ToonVliegveld(VliegveldModel vliegveld)
    {
        Console.WriteLine("");
        Console.WriteLine("Gevonden vliegveld:");
        Console.WriteLine($"  Naam:      {vliegveld.Naam}");
        Console.WriteLine($"  Stad:      {vliegveld.Stad}");
        Console.WriteLine($"  Land:      {vliegveld.Land}");
        Console.WriteLine($"  Afkorting: {vliegveld.Afkorting}");
        Console.WriteLine("");
    }
}
