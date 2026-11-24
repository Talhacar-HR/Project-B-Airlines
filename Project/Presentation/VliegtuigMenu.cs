static class VliegtuigMenu
{
    static private VliegtuigLogic vliegtuigLogic = new VliegtuigLogic();
    static private StoelkaartLogic stoelkaartLogic = new StoelkaartLogic();

    public static void Start()
    {
        string feedback = "";

        bool menuRunning = true;
        while (menuRunning)
        {
            string menuInput = MenuOptions(feedback);

            if (menuInput == "1")
                feedback = StoelMenu.Overzicht();
            else if (menuInput == "2")
                feedback = Toevoegen();
            else if (menuInput == "3")
                feedback = Aanpassen();
            else if (menuInput == "0")
                menuRunning = false;
            else
                feedback = "Ongeldige keuze.";
        }
    }

    public static string MenuOptions(string feedback)
    {
        string[] menuOptions = { "1 - Vliegtuigmodellen", "2 - Toevoegen", "3 - Aanpassen", "0 - Terug" };
        int selectedIndex = 0;

        while (true)
        {
            Menu.PrintTitel();

            if (feedback != "")
                Console.WriteLine(feedback);
                Console.WriteLine("");

            Console.WriteLine("Vliegtuigen beheren");
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

    public static string Toevoegen()
    {
        Console.Write("Bagageruimte: ");
        string baggageRuimteInput = Console.ReadLine() ?? "";
        Console.Write("Model: ");
        string model = Console.ReadLine() ?? "";
        Console.Write("Merk: ");
        string merk = Console.ReadLine() ?? "";

        if (CheckPlane(baggageRuimteInput, model, merk, 0, out VliegtuigModel plane))
        {
            vliegtuigLogic.Add(plane);
            return "Vliegtuig toegevoegd.";
        }
        else
        {
            return "Vliegtuig niet toegevoegd; er ging iets fout.";
        }
    }

    public static string Aanpassen()
    {
        VliegtuigModel? currentPlane = ZoekVliegtuig();
        if (currentPlane is null)
        {
            return "";
        }
        int id = currentPlane.ID;
        ToonVliegtuig(currentPlane);

        string baggageRuimteInput = Invoer.VraagTotGeldig(
            $"Nieuwe bagageruimte [{currentPlane.BaggageRuimte}]: ",
            input => string.IsNullOrWhiteSpace(input) ||
                (int.TryParse(input, out int baggageRuimte) && vliegtuigLogic.IsValidBaggageRuimte(baggageRuimte)),
            "Bagageruimte moet een geldig getal van nul of hoger zijn.");
        string model = Invoer.VraagTotGeldig(
            $"Nieuw model [{currentPlane.Model}]: ",
            input => string.IsNullOrWhiteSpace(input) || vliegtuigLogic.IsValidModel(input),
            "Model is verplicht.");
        string merk = Invoer.VraagTotGeldig(
            $"Nieuw merk [{currentPlane.Merk}]: ",
            input => string.IsNullOrWhiteSpace(input) || vliegtuigLogic.IsValidMerk(input),
            "Merk is verplicht.");

        if (string.IsNullOrWhiteSpace(baggageRuimteInput)) baggageRuimteInput = currentPlane.BaggageRuimte.ToString();
        if (string.IsNullOrWhiteSpace(model)) model = currentPlane.Model;
        if (string.IsNullOrWhiteSpace(merk)) merk = currentPlane.Merk;

        if (CheckPlane(baggageRuimteInput, model, merk, id, out VliegtuigModel plane))
        {
            if (vliegtuigLogic.Update(plane))
            {
                return "Vliegtuig aangepast.";
            }
            else
            {
                return $"Er bestaat geen vliegtuig met ID {id}.";
            }
        }
        return "Vliegtuig niet aangepast; er ging iets fout.";
    }

    private static bool CheckPlane(string baggageRuimteInput, string model, string merk, int id, out VliegtuigModel plane)
    {
        bool isValid = true;
        bool hasBaggageRuimte = int.TryParse(baggageRuimteInput, out int baggageRuimte);

        if (!hasBaggageRuimte || !vliegtuigLogic.IsValidBaggageRuimte(baggageRuimte))
        {
            Console.WriteLine("Bagageruimte moet een geldig getal van nul of hoger zijn.");
            isValid = false;
        }

        if (!vliegtuigLogic.IsValidModel(model))
        {
            Console.WriteLine("Model is verplicht.");
            isValid = false;
        }

        if (!vliegtuigLogic.IsValidMerk(merk))
        {
            Console.WriteLine("Merk is verplicht.");
            isValid = false;
        }

        plane = new VliegtuigModel(id, baggageRuimte, model, merk);
        return isValid;
    }
    private static VliegtuigModel? ZoekVliegtuig()
    {
        Console.Write("Zoek vliegtuig op model: ");
        string zoekterm = Console.ReadLine() ?? "";

        List<VliegtuigModel> gevonden = vliegtuigLogic.Zoek(zoekterm);

        if (gevonden.Count == 0)
        {
            Console.WriteLine($"Geen vliegtuig gevonden met \"{zoekterm}\". Controleer de zoekterm en probeer het opnieuw.");
            return null;
        }

        if (gevonden.Count == 1)
        {
            return gevonden[0];
        }

        Console.WriteLine("Meerdere vliegtuigen gevonden:");
        for (int i = 0; i < gevonden.Count; i += 1)
        {
            Console.WriteLine($"{i + 1}. {gevonden[i].Merk} {gevonden[i].Model}");
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

    private static void ToonVliegtuig(VliegtuigModel vliegtuig)
    {
        Console.WriteLine("");
        Console.WriteLine("Gevonden vliegtuig:");
        Console.WriteLine($"  Merk:         {vliegtuig.Merk}");
        Console.WriteLine($"  Model:        {vliegtuig.Model}");
        Console.WriteLine($"  Bagageruimte: {vliegtuig.BaggageRuimte}");
        Console.WriteLine("");
    }

    public static void StoelenAanmaken()
    {
        VliegtuigModel? vliegtuig = ZoekVliegtuig();
        if (vliegtuig is null)
        {
            return;
        }

        if (stoelkaartLogic.GetIndeling(vliegtuig.Model).Length == 0)
        {
            Console.WriteLine($"Er is geen plattegrond voor model {vliegtuig.Model}.");
            return;
        }

        int aantal = stoelkaartLogic.MaakStoelenAan(vliegtuig);
        Console.WriteLine($"{aantal} stoelen aangemaakt voor {vliegtuig.Merk} {vliegtuig.Model}.");
    }
}
