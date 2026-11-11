static class VliegtuigMenu
{
    static private VliegtuigLogic vliegtuigLogic = new VliegtuigLogic();

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
        Console.WriteLine("Vliegtuigen beheren");
        Console.WriteLine("1 - Overzicht");
        Console.WriteLine("2 - Toevoegen");
        Console.WriteLine("3 - Aanpassen");
        Console.WriteLine("4 - Stoelen beheren");
        Console.WriteLine("0 - Terug");
        return Console.ReadLine() ?? "";
    }

    public static void Overzicht()
    {
        List<VliegtuigModel> planes = vliegtuigLogic.GetAll();
        if (planes.Count == 0)
            Console.WriteLine("Geen vliegtuigen gevonden.");

        foreach (VliegtuigModel plane in planes)
            Console.WriteLine($"ID: {plane.ID}, Bagageruimte: {plane.BaggageRuimte}, Model: {plane.Model}, Merk: {plane.Merk}");
    }

    public static void Toevoegen()
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
            Console.WriteLine("Vliegtuig toegevoegd.");
        }
    }

    public static void Aanpassen()
    {
        VliegtuigModel? currentPlane = ZoekVliegtuig();
        if (currentPlane is null)
        {
            return;
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
                Console.WriteLine("Vliegtuig aangepast.");
            }
            else
            {
                Console.WriteLine($"Er bestaat geen vliegtuig met ID {id}.");
            }
        }
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
}
