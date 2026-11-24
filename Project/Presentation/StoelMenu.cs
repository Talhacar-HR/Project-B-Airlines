static class StoelMenu
{
    static private StoelLogic stoelLogic = new StoelLogic();

    public static void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            Menu.PrintTitel();

            Console.WriteLine("");
            string input = MenuOptions();
            Console.WriteLine("");

            if (input == "1")
                Overzicht();
            else if (input == "0")
                menuRunning = false;
            else
                Console.WriteLine("Ongeldige keuze.");
        }
    }

    public static string MenuOptions()
    {
        string[] menuOptions = { "1 - Overzicht", "0 - Terug" };
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Stoelen beheren");
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

    public static void Overzicht()
    {
        List<StoelModel> seats = stoelLogic.GetAll();
        if (seats.Count == 0)
            Console.WriteLine("Geen stoelen gevonden.");

        foreach (StoelModel seat in seats)
            Console.WriteLine($"ID: {seat.ID}, Vliegtuig-ID: {seat.VliegtuigID}, Stoelnummer: {seat.StoelNummer}, Rijnummer: {seat.RijNummer}, Stoelklasse: {seat.StoelKlasse}");
    }

    public static void Toevoegen()
    {
        Console.Write("Vliegtuig-ID: ");
        string vliegtuigIDInput = Console.ReadLine() ?? "";
        Console.Write("Stoelnummer: ");
        string stoelNummer = Console.ReadLine() ?? "";
        Console.Write("Rijnummer: ");
        string rijNummer = Console.ReadLine() ?? "";
        Console.Write("Stoelklasse: ");
        string stoelKlasseInput = Console.ReadLine() ?? "";

        if (CheckSeat(vliegtuigIDInput, stoelNummer, rijNummer, stoelKlasseInput, 0, out StoelModel seat))
        {
            stoelLogic.Add(seat);
            Console.WriteLine("Stoel toegevoegd.");
        }
    }

    public static void Aanpassen()
    {
        StoelModel? currentSeat = ZoekStoel();
        if (currentSeat is null)
        {
            return;
        }
        int id = currentSeat.ID;

        ToonStoel(currentSeat);

        Console.Write($"Nieuw vliegtuig-ID (huidig: {currentSeat.VliegtuigID}): ");
        string vliegtuigIDInput = Console.ReadLine() ?? "";
        Console.Write($"Nieuw stoelnummer (huidig: {currentSeat.StoelNummer}): ");
        string stoelNummer = Console.ReadLine() ?? "";
        Console.Write($"Nieuw rijnummer (huidig: {currentSeat.RijNummer}): ");
        string rijNummer = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe stoelklasse (huidig: {currentSeat.StoelKlasse}): ");
        string stoelKlasseInput = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(vliegtuigIDInput)) vliegtuigIDInput = currentSeat.VliegtuigID.ToString();
        if (string.IsNullOrWhiteSpace(stoelNummer)) stoelNummer = currentSeat.StoelNummer;
        if (string.IsNullOrWhiteSpace(rijNummer)) rijNummer = currentSeat.RijNummer;
        if (string.IsNullOrWhiteSpace(stoelKlasseInput)) stoelKlasseInput = currentSeat.StoelKlasse.ToString();

        if (CheckSeat(vliegtuigIDInput, stoelNummer, rijNummer, stoelKlasseInput, id, out StoelModel seat))
        {
            if (stoelLogic.Update(seat))
            {
                Console.WriteLine("Stoel aangepast.");
            }
            else
            {
                Console.WriteLine($"Er bestaat geen stoel met ID {id}.");
            }
        }
    }

    private static bool CheckSeat(string vliegtuigIDInput, string stoelNummer, string rijNummer,
        string stoelKlasseInput, int id, out StoelModel seat)
    {
        bool isValid = true;
        bool hasVliegtuigID = int.TryParse(vliegtuigIDInput, out int vliegtuigID);
        bool hasStoelKlasse = int.TryParse(stoelKlasseInput, out int stoelKlasse);

        if (!hasVliegtuigID || !stoelLogic.IsValidVliegtuigID(vliegtuigID))
        {
            Console.WriteLine("Voer een bestaand vliegtuig-ID in.");
            isValid = false;
        }

        if (!stoelLogic.IsValidStoelNummer(stoelNummer))
        {
            Console.WriteLine("Stoelnummer is verplicht.");
            isValid = false;
        }

        if (!stoelLogic.IsValidRijNummer(rijNummer))
        {
            Console.WriteLine("Rijnummer is verplicht.");
            isValid = false;
        }

        if (!hasStoelKlasse || !stoelLogic.IsValidStoelKlasse(stoelKlasse))
        {
            Console.WriteLine("Stoelklasse moet een getal groter dan nul zijn.");
            isValid = false;
        }

        seat = new StoelModel(id, vliegtuigID, stoelNummer, rijNummer, stoelKlasse);
        return isValid;
    }
    private static StoelModel? ZoekStoel()
    {
        Console.Write("Zoek stoel op stoelnummer (bijvoorbeeld 1A): ");
        string zoekterm = Console.ReadLine() ?? "";

        List<StoelModel> gevonden = stoelLogic.Zoek(zoekterm);

        if (gevonden.Count == 0)
        {
            Console.WriteLine($"Geen stoel gevonden met \"{zoekterm}\". Controleer de zoekterm en probeer het opnieuw.");
            return null;
        }

        if (gevonden.Count == 1)
        {
            return gevonden[0];
        }

        Console.WriteLine("Meerdere stoelen gevonden:");
        for (int i = 0; i < gevonden.Count; i += 1)
        {
            Console.WriteLine($"{i + 1}. Stoel {gevonden[i].StoelNummer} in vliegtuig {gevonden[i].VliegtuigID}");
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

    private static void ToonStoel(StoelModel stoel)
    {
        Console.WriteLine("");
        Console.WriteLine("Gevonden stoel:");
        Console.WriteLine($"  Stoelnummer:  {stoel.StoelNummer}");
        Console.WriteLine($"  Rijnummer:    {stoel.RijNummer}");
        Console.WriteLine($"  Stoelklasse:  {stoel.StoelKlasse}");
        Console.WriteLine($"  Vliegtuig-ID: {stoel.VliegtuigID}");
        Console.WriteLine("");
    }
}
