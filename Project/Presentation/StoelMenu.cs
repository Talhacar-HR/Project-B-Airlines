static class StoelMenu
{
    static private StoelLogic stoelLogic = new StoelLogic();

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
        Console.WriteLine("Stoelen beheren");
        Console.WriteLine("1 - Overzicht");
        Console.WriteLine("2 - Toevoegen");
        Console.WriteLine("3 - Aanpassen");
        Console.WriteLine("0 - Terug");
        return Console.ReadLine() ?? "";
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

        if (int.TryParse(vliegtuigIDInput, out int vliegtuigID) &&
            int.TryParse(stoelKlasseInput, out int stoelKlasse))
        {
            try
            {
                stoelLogic.Add(new StoelModel(0, vliegtuigID, stoelNummer, rijNummer, stoelKlasse));
                Console.WriteLine("Stoel toegevoegd.");
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
        Console.Write("ID van de stoel: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Ongeldige invoer.");
            return;
        }

        StoelModel? currentSeat = stoelLogic.GetAll().Find(seat => seat.ID == id);
        if (currentSeat is null)
        {
            Console.WriteLine($"Er bestaat geen stoel met ID {id}.");
            return;
        }

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

        if (int.TryParse(vliegtuigIDInput, out int vliegtuigID) &&
            int.TryParse(stoelKlasseInput, out int stoelKlasse))
        {
            try
            {
                stoelLogic.Update(new StoelModel(id, vliegtuigID, stoelNummer, rijNummer, stoelKlasse));
                Console.WriteLine("Stoel aangepast.");
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
