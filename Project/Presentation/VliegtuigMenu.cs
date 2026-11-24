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
            Console.WriteLine("");
            string input = MenuOptions();
            Console.WriteLine("");

            if (feedback != "")
                Console.WriteLine(feedback);
                Console.WriteLine("");

            if (input == "1")
                Overzicht();
            else if (input == "2")
                feedback = Toevoegen();
            else if (input == "3")
                feedback = Aanpassen();
            else if (input == "4")
                StoelMenu.Start();
            else if (input == "0")
                menuRunning = false;
            else
                Console.WriteLine("Ongeldige keuze.");
        }
    }

    public static string MenuOptions()
    {
        string[] menuOptions = { "1 - Overzicht", "2 - Toevoegen", "3 - Aanpassen", "4 - Stoelen beheren", "0 - Terug" };
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
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

    public static void Overzicht()
    {
        List<VliegtuigModel> planes = vliegtuigLogic.GetAll();
        if (planes.Count == 0)
            Console.WriteLine("Geen vliegtuigen gevonden.");

        foreach (VliegtuigModel plane in planes)
            Console.WriteLine($"ID: {plane.ID}, Bagageruimte: {plane.BaggageRuimte}, Model: {plane.Model}, Merk: {plane.Merk}");
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

        Console.Write($"Nieuwe bagageruimte (huidig: {currentPlane.BaggageRuimte}): ");
        string baggageRuimteInput = Console.ReadLine() ?? "";
        Console.Write($"Nieuw model (huidig: {currentPlane.Model}): ");
        string model = Console.ReadLine() ?? "";
        Console.Write($"Nieuw merk (huidig: {currentPlane.Merk}): ");
        string merk = Console.ReadLine() ?? "";

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
        return "";
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
