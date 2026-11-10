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

        if (int.TryParse(baggageRuimteInput, out int baggageRuimte))
        {
            try
            {
                vliegtuigLogic.Add(new VliegtuigModel(0, baggageRuimte, model, merk));
                Console.WriteLine("Vliegtuig toegevoegd.");
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        else
        {
            Console.WriteLine("Ongeldige invoer.");
        }
    }

    public static void Aanpassen()
    {
        Console.Write("ID van het vliegtuig: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Ongeldige invoer.");
            return;
        }

        VliegtuigModel? currentPlane = vliegtuigLogic.GetAll().Find(plane => plane.ID == id);
        if (currentPlane is null)
        {
            Console.WriteLine($"Er bestaat geen vliegtuig met ID {id}.");
            return;
        }

        Console.Write($"Nieuwe bagageruimte (huidig: {currentPlane.BaggageRuimte}): ");
        string baggageRuimteInput = Console.ReadLine() ?? "";
        Console.Write($"Nieuw model (huidig: {currentPlane.Model}): ");
        string model = Console.ReadLine() ?? "";
        Console.Write($"Nieuw merk (huidig: {currentPlane.Merk}): ");
        string merk = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(baggageRuimteInput)) baggageRuimteInput = currentPlane.BaggageRuimte.ToString();
        if (string.IsNullOrWhiteSpace(model)) model = currentPlane.Model;
        if (string.IsNullOrWhiteSpace(merk)) merk = currentPlane.Merk;

        if (int.TryParse(baggageRuimteInput, out int baggageRuimte))
        {
            try
            {
                vliegtuigLogic.Update(new VliegtuigModel(id, baggageRuimte, model, merk));
                Console.WriteLine("Vliegtuig aangepast.");
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (KeyNotFoundException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        else
        {
            Console.WriteLine("Ongeldige invoer.");
        }
    }
}
