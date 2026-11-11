static class VliegveldMenu
{
    static private VliegveldLogic vliegveldLogic = new VliegveldLogic();

    public static void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            Console.WriteLine("");
            string input = MenuOptions();
            Console.WriteLine("");

            if (input == "1")
                Overzicht();
            else if (input == "2")
                Toevoegen();
            else if (input == "3")
                Aanpassen();
            else if (input == "0")
                menuRunning = false;
            else
                Console.WriteLine("Ongeldige keuze.");
        }
    }

    public static string MenuOptions()
    {
        Console.WriteLine("Vliegvelden beheren");
        Console.WriteLine("1 - Overzicht");
        Console.WriteLine("2 - Toevoegen");
        Console.WriteLine("3 - Aanpassen");
        Console.WriteLine("0 - Terug");
        return Console.ReadLine() ?? "";
    }

    public static void Overzicht()
    {
        List<VliegveldModel> airports = vliegveldLogic.GetAll();
        if (airports.Count == 0)
            Console.WriteLine("Geen vliegvelden gevonden.");

        foreach (VliegveldModel airport in airports)
            Console.WriteLine($"ID: {airport.ID}, Naam: {airport.Naam}, Stad: {airport.Stad}, Land: {airport.Land}, Afkorting: {airport.Afkorting}");
    }

    public static void Toevoegen()
    {
        Console.Write("Naam: ");
        string naam = Console.ReadLine() ?? "";
        Console.Write("Stad: ");
        string stad = Console.ReadLine() ?? "";
        Console.Write("Land: ");
        string land = Console.ReadLine() ?? "";
        Console.Write("Afkorting: ");
        string afkorting = Console.ReadLine() ?? "";

        if (CheckAirport(0, naam, stad, land, afkorting, out VliegveldModel airport))
        {
            vliegveldLogic.Add(airport);
            Console.WriteLine("Vliegveld toegevoegd.");
        }
    }

    public static void Aanpassen()
    {
        VliegveldModel? currentAirport = ZoekVliegveld();
        if (currentAirport is null)
        {
            return;
        }
        int id = currentAirport.ID;

        ToonVliegveld(currentAirport);

        Console.Write($"Nieuwe naam (huidig: {currentAirport.Naam}): ");
        string naam = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe stad (huidig: {currentAirport.Stad}): ");
        string stad = Console.ReadLine() ?? "";
        Console.Write($"Nieuw land (huidig: {currentAirport.Land}): ");
        string land = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe afkorting (huidig: {currentAirport.Afkorting}): ");
        string afkorting = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(naam)) naam = currentAirport.Naam;
        if (string.IsNullOrWhiteSpace(stad)) stad = currentAirport.Stad;
        if (string.IsNullOrWhiteSpace(land)) land = currentAirport.Land;
        if (string.IsNullOrWhiteSpace(afkorting)) afkorting = currentAirport.Afkorting;

        if (!CheckAirport(id, naam, stad, land, afkorting, out VliegveldModel airport))
            return;

        if (vliegveldLogic.Update(airport))
        {
            Console.WriteLine("Vliegveld aangepast.");
        }
        else
        {
            Console.WriteLine($"Er bestaat geen vliegveld met ID {id}.");
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
    private static VliegveldModel? ZoekVliegveld()
    {
        Console.Write("Zoek vliegveld op afkorting: ");
        string zoekterm = Console.ReadLine() ?? "";

        List<VliegveldModel> gevonden = vliegveldLogic.Zoek(zoekterm);

        if (gevonden.Count == 0)
        {
            Console.WriteLine($"Geen vliegveld gevonden met \"{zoekterm}\". Controleer de zoekterm en probeer het opnieuw.");
            return null;
        }

        if (gevonden.Count == 1)
        {
            return gevonden[0];
        }

        Console.WriteLine("Meerdere vliegvelden gevonden:");
        for (int i = 0; i < gevonden.Count; i += 1)
        {
            Console.WriteLine($"{i + 1}. {gevonden[i].Naam} ({gevonden[i].Stad})");
        }

        Console.Write("Kies een nummer: ");
        string keuze = Console.ReadLine() ?? "";

        if (int.TryParse(keuze, out int nummer) && nummer >= 1 && nummer <= gevonden.Count)
        {
            return gevonden[nummer - 1];
        }

        Console.WriteLine("Ongeldige keuze.");
        return null;
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
