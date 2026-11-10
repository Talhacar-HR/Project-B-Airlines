static class VluchtMenu
{
    static private VluchtLogic vluchtLogic = new VluchtLogic();

    public static void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            Console.WriteLine("");
            string menuInput = MenuOptions();
            Console.WriteLine("");

            if (menuInput == "1")
            {
                Overzicht();
            }
            else if (menuInput == "2")
            {
                Toevoegen();
            }
            else if (menuInput == "3")
            {
                Aanpassen();
            }
            else if (menuInput == "0")
            {
                menuRunning = false;
            }
            else
            {
                Console.WriteLine("Ongeldige keuze.");
            }
        }
    }

    
    public static string MenuOptions()
    {
        Console.WriteLine("Vluchten beheren");
        Console.WriteLine("1 - Overzicht");
        Console.WriteLine("2 - Toevoegen");
        Console.WriteLine("3 - Aanpassen");
        Console.WriteLine("0 - Terug");
        return Console.ReadLine() ?? "";
    }

    public static void Overzicht()
    {
        List<VluchtModel> flights = vluchtLogic.GetAll();
        if (flights.Count == 0)
        {
            Console.WriteLine("Geen vluchten gevonden.");
        }

        foreach (VluchtModel flight in flights)
        {
            Console.WriteLine($"ID: {flight.ID}, Vliegtuig-ID: {flight.VliegtuigID}, Vertrekluchthaven-ID: {flight.VertrekLocatie}, Aankomstluchthaven-ID: {flight.AankomstLocatie}, Vertrek datum: {flight.VertrekDatum}, Vertrek tijd: {flight.VertrekTijd}, Vertraging (HH:mm): {flight.Vertraging}, Vluchtnummer: {flight.VluchtNummer}");
        }
    }

    public static void Toevoegen()
    {
        Console.Write("Vliegtuig-ID: ");
        string vliegtuigIDInput = Console.ReadLine() ?? "";
        Console.Write("Vertrekluchthaven-ID: ");
        string vertrekLocatieInput = Console.ReadLine() ?? "";
        Console.Write("Aankomstluchthaven-ID: ");
        string aankomstLocatieInput = Console.ReadLine() ?? "";
        Console.Write("Vertrek datum (yyyy-MM-dd): ");
        string vertrekDatum = Console.ReadLine() ?? "";
        Console.Write("Vertrek tijd (HH:mm): ");
        string vertrekTijd = Console.ReadLine() ?? "";
        Console.Write("Vertraging (HH:mm): ");
        string vertraging = Console.ReadLine() ?? "";
        Console.Write("Vluchtnummer: ");
        string vluchtNummerInput = Console.ReadLine() ?? "";

        if (CheckFlight(vliegtuigIDInput, vertrekLocatieInput, aankomstLocatieInput, vertrekDatum, vertrekTijd, vertraging, vluchtNummerInput, 0, out VluchtModel flight))
        {
            vluchtLogic.Add(flight);
            Console.WriteLine("Vlucht toegevoegd.");
        }
    }

    public static void Aanpassen()
    {
        Console.Write("ID van de vlucht: ");
        string idInput = Console.ReadLine() ?? "";

        if (!int.TryParse(idInput, out int id))
        {
            Console.WriteLine("Voer een geldig vlucht-ID in.");
            return;
        }

        VluchtModel? currentFlight = vluchtLogic.GetById(id);
        if (currentFlight == null)
        {
            Console.WriteLine($"Er bestaat geen vlucht met ID {id}.");
            return;
        }

        Console.Write($"Nieuw vliegtuig-ID (huidig: {currentFlight.VliegtuigID}): ");
        string vliegtuigIDInput = Console.ReadLine() ?? "";
        Console.Write($"Nieuw vertrekluchthaven-ID (huidig: {currentFlight.VertrekLocatie}): ");
        string vertrekLocatieInput = Console.ReadLine() ?? "";
        Console.Write($"Nieuw aankomstluchthaven-ID (huidig: {currentFlight.AankomstLocatie}): ");
        string aankomstLocatieInput = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe vertrek datum (huidig: {currentFlight.VertrekDatum}, yyyy-MM-dd): ");
        string vertrekDatum = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe vertrek tijd (huidig: {currentFlight.VertrekTijd}, HH:mm): ");
        string vertrekTijd = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe vertraging (huidig: {currentFlight.Vertraging}, HH:mm): ");
        string vertraging = Console.ReadLine() ?? "";
        Console.Write($"Nieuw vluchtnummer (huidig: {currentFlight.VluchtNummer}): ");
        string vluchtNummerInput = Console.ReadLine() ?? "";

        if (CheckFlight(vliegtuigIDInput, vertrekLocatieInput, aankomstLocatieInput, vertrekDatum, vertrekTijd, vertraging, vluchtNummerInput, id, out VluchtModel flight))
        {
            if (vluchtLogic.Update(flight))
            {
                Console.WriteLine("Vlucht aangepast.");
            }
            else
            {
                Console.WriteLine($"Er bestaat geen vlucht met ID {id}.");
            }
        }
        return;
    }

    private static bool CheckFlight(string vliegtuigIDInput, string vertrekLocatieInput, string aankomstLocatieInput,
        string vertrekDatum, string vertrekTijd, string vertraging, string vluchtNummerInput, int id,
        out VluchtModel flight)
    {
        bool isValid = true;
        bool hasVliegtuigID = int.TryParse(vliegtuigIDInput, out int vliegtuigID);
        bool hasVertrekLocatie = int.TryParse(vertrekLocatieInput, out int vertrekLocatie);
        bool hasAankomstLocatie = int.TryParse(aankomstLocatieInput, out int aankomstLocatie);
        bool hasVluchtNummer = int.TryParse(vluchtNummerInput, out int vluchtNummer);

        if (!hasVliegtuigID || !vluchtLogic.IsValidVliegtuigID(vliegtuigID))
        {
            Console.WriteLine("Voer een geldig bestaand vliegtuig-ID in.");
            isValid = false;
        }

        if (!hasVertrekLocatie || !vluchtLogic.IsValidVertrekLocatie(vertrekLocatie))
        {
            Console.WriteLine("Voer een geldig bestaand vertrekluchthaven-ID in.");
            isValid = false;
        }

        if (!hasAankomstLocatie || !vluchtLogic.IsValidAankomstLocatie(aankomstLocatie))
        {
            Console.WriteLine("Voer een geldig bestaand aankomstluchthaven-ID in.");
            isValid = false;
        }
        else if (hasVertrekLocatie && !vluchtLogic.AreDifferentLocations(vertrekLocatie, aankomstLocatie))
        {
            Console.WriteLine("De vertrek- en aankomst luchthaven moeten verschillend zijn.");
            isValid = false;
        }

        if (!vluchtLogic.IsValidVertrekDatum(vertrekDatum))
        {
            Console.WriteLine("De vertrekdatum moet geldig zijn en het formaat yyyy-MM-dd hebben.");
            isValid = false;
        }

        if (!vluchtLogic.IsValidVertrekTijd(vertrekTijd))
        {
            Console.WriteLine("De vertrektijd moet geldig zijn en het formaat HH:mm hebben.");
            isValid = false;
        }

        if (!vluchtLogic.IsValidVertraging(vertraging))
        {
            Console.WriteLine("De vertraging moet geldig zijn en het formaat HH:mm hebben.");
            isValid = false;
        }

        if (!hasVluchtNummer || !vluchtLogic.IsValidVluchtNummer(vluchtNummer, id))
        {
            Console.WriteLine("Voer een uniek vluchtnummer groter dan nul in.");
            isValid = false;
        }

        flight = new VluchtModel(id, vliegtuigID, vertrekLocatie, aankomstLocatie, vertrekDatum, vertrekTijd, vertraging, vluchtNummer);
        return isValid;
    }
}