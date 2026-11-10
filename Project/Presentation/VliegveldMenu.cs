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

        try
        {
            vliegveldLogic.Add(new VliegveldModel(0, naam, stad, land, afkorting));
            Console.WriteLine("Vliegveld toegevoegd.");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    public static void Aanpassen()
    {
        Console.Write("ID van het vliegveld: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Ongeldige invoer.");
            return;
        }

        VliegveldModel? currentAirport = vliegveldLogic.GetAll().Find(airport => airport.ID == id);
        if (currentAirport is null)
        {
            Console.WriteLine($"Er bestaat geen vliegveld met ID {id}.");
            return;
        }

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

        try
        {
            vliegveldLogic.Update(new VliegveldModel(id, naam, stad, land, afkorting));
            Console.WriteLine("Vliegveld aangepast.");
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
}
