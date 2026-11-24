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
        string[] menuOptions = { "1 - Vliegtuigmodellen Overzicht", "2 - Toevoegen", "3 - Aanpassen", "0 - Terug" };
        int selectedIndex = 0;

        while (true)
        {
            Menu.PrintTitel();

            if (feedback != "")
            {
                Console.WriteLine(feedback);
                Console.WriteLine("");
            }

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
        string mijnKeuzes = "";
        Console.Write("Model: ");
        string model = Console.ReadLine() ?? "";
        mijnKeuzes += $"Gekozen model: {model}\n";
        Console.Write("Merk: ");
        string merk = Console.ReadLine() ?? "";
        mijnKeuzes += $"Gekozen merk: {merk}\n";
        Console.Write("Bagageruimte: ");
        string baggageRuimteInput = Console.ReadLine() ?? "";
        mijnKeuzes += $"Gekozen bagageruimte: {baggageRuimteInput}\n";

        if (CheckPlane(baggageRuimteInput, model, merk, 0, out VliegtuigModel plane))
        {
            vliegtuigLogic.Add(plane);
            return $"Vliegtuig toegevoegd.\n{mijnKeuzes}";
        }
        else
        {
            return "Vliegtuig niet toegevoegd; er ging iets fout.";
        }
    }

    public static string Aanpassen()
    {
        string mijnKeuzes = "";

        VliegtuigModel? currentPlane = KeuzeMenu.KiesVliegtuig(aanpassen: true);
        if (currentPlane is null)
        {
            return "";
        }
        int id = currentPlane.ID;

        ToonVliegtuig(currentPlane, mijnKeuzes);
        VliegtuigModel? gekozenModel = KeuzeMenu.KiesVliegtuig(currentPlane.ID, aanpassen: true);
        if (gekozenModel is null)
            return "Vliegtuig niet aangepast; er is geen model gekozen.";
        string model = gekozenModel.Model;
        mijnKeuzes += $"Gekozen model: {model}\n";

        ToonVliegtuig(currentPlane, mijnKeuzes);
        string merk = Invoer.VraagTotGeldig(
            $"Mogelijk nieuw merk [{currentPlane.Merk}]: ",
            input => string.IsNullOrWhiteSpace(input) || vliegtuigLogic.IsValidMerk(input),
            "Merk is verplicht.");
        if (string.IsNullOrWhiteSpace(merk)) merk = currentPlane.Merk;
        mijnKeuzes += $"Gekozen merk: {merk}\n";

        ToonVliegtuig(currentPlane, mijnKeuzes);
        string baggageRuimteInput = Invoer.VraagTotGeldig(
            $"Mogelijk nieuwe bagageruimte [{currentPlane.BaggageRuimte}]: ",
            input => string.IsNullOrWhiteSpace(input) ||
                (int.TryParse(input, out int baggageRuimte) && vliegtuigLogic.IsValidBaggageRuimte(baggageRuimte)),
            "Bagageruimte moet een geldig getal van nul of hoger zijn.");
        if (string.IsNullOrWhiteSpace(baggageRuimteInput)) baggageRuimteInput = currentPlane.BaggageRuimte.ToString();
        mijnKeuzes += $"Gekozen bagageruimte: {baggageRuimteInput}\n";

        ToonVliegtuig(currentPlane, mijnKeuzes);
        Console.WriteLine("Wilt u deze aanpassingen bevestigen? (J/N)");
        if (Console.ReadLine()?.Trim().ToLowerInvariant() != "j")
            return "Vliegtuig niet aangepast; bevestiging geannuleerd.";

        if (CheckPlane(baggageRuimteInput, model, merk, id, out VliegtuigModel plane))
        {
            if (vliegtuigLogic.Update(plane))
            {
                return $"Vliegtuig aangepast.\n{mijnKeuzes}";
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

        if (!hasBaggageRuimte || !vliegtuigLogic.IsValidBaggageRuimte(baggageRuimte))
        {
            Console.WriteLine("Bagageruimte moet een geldig getal van nul of hoger zijn.");
            isValid = false;
        }

        plane = new VliegtuigModel(id, baggageRuimte, model, merk);
        return isValid;
    }
    private static void ToonVliegtuig(VliegtuigModel vliegtuig, string mijnKeuzes)
    {
        Menu.PrintTitel();
        Console.WriteLine("Gevonden vliegtuig:");
        Console.WriteLine($"  Merk:         {vliegtuig.Merk}");
        Console.WriteLine($"  Model:        {vliegtuig.Model}");
        Console.WriteLine($"  Bagageruimte: {vliegtuig.BaggageRuimte}");
        Console.WriteLine("");
        if (mijnKeuzes != "")
        {
            Console.WriteLine("Gekozen wijzigingen:");
            Console.WriteLine(mijnKeuzes);
        }
    }

    public static void StoelenAanmaken()
    {
        VliegtuigModel? vliegtuig = KeuzeMenu.KiesVliegtuig();
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
