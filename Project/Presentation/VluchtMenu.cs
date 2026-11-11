static class VluchtMenu
{
    static private VluchtLogic vluchtLogic = new VluchtLogic();
    static private VliegtuigLogic vliegtuigLogic = new VliegtuigLogic();
    static private VliegveldLogic vliegveldLogic = new VliegveldLogic();

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
            VliegtuigModel? vliegtuig = vliegtuigLogic.GetById(flight.VliegtuigID);
            VliegveldModel? vertrekVliegveld = vliegveldLogic.GetById(flight.VertrekLocatie);
            VliegveldModel? aankomstVliegveld = vliegveldLogic.GetById(flight.AankomstLocatie);
            Console.WriteLine($"ID: {flight.ID}, Vliegtuigmodel: {vliegtuig?.Model ?? "Onbekend"}, Vertrekvliegveld: {vertrekVliegveld?.Afkorting ?? "Onbekend"}, Aankomstvliegveld: {aankomstVliegveld?.Afkorting ?? "Onbekend"}, Vertrek datum: {flight.VertrekDatum}, Vertrek tijd: {flight.VertrekTijd}, Vertraging (HH:mm): {flight.Vertraging}, Vluchtnummer: {flight.VluchtNummer}");
        }
    }

    public static void Toevoegen()
    {
        Console.Write("Vliegtuigmodel: ");
        string vliegtuigModelInput = Console.ReadLine() ?? "";
        Console.Write("Vertrekvliegveld-afkorting: ");
        string vertrekAfkortingInput = Console.ReadLine() ?? "";
        Console.Write("Aankomstvliegveld-afkorting: ");
        string aankomstAfkortingInput = Console.ReadLine() ?? "";
        Console.Write("Vertrek datum (yyyy-MM-dd): ");
        string vertrekDatum = Console.ReadLine() ?? "";
        Console.Write("Vertrek tijd (HH:mm): ");
        string vertrekTijd = Console.ReadLine() ?? "";
        Console.Write("Vertraging (HH:mm): ");
        string vertraging = Console.ReadLine() ?? "";
        Console.Write("Vluchtnummer: ");
        string vluchtNummerInput = Console.ReadLine() ?? "";

        if (CheckFlight(vliegtuigModelInput, vertrekAfkortingInput, aankomstAfkortingInput, vertrekDatum, vertrekTijd, vertraging, vluchtNummerInput, 0, out VluchtModel flight))
        {
            vluchtLogic.Add(flight);
            Console.WriteLine("Vlucht toegevoegd.");
        }
    }

    public static void Aanpassen()
    {
        Console.Write("Vluchtnummer van de vlucht: ");
        string vluchtNummerInput = Console.ReadLine() ?? "";

        if (!int.TryParse(vluchtNummerInput, out int vluchtNummer))
        {
            Console.WriteLine("Voer een geldig vluchtnummer in.");
            return;
        }

        VluchtModel? currentFlight = null;
        foreach (VluchtModel gevondenVlucht in vluchtLogic.GetAll())
        {
            if (gevondenVlucht.VluchtNummer == vluchtNummer)
            {
                currentFlight = gevondenVlucht;
                break;
            }
        }

        if (currentFlight == null)
        {
            Console.WriteLine($"Er bestaat geen vlucht met vluchtnummer {vluchtNummer}.");
            return;
        }
        int id = currentFlight.ID;

        ToonVlucht(currentFlight);

        VliegtuigModel? currentVliegtuig = vliegtuigLogic.GetById(currentFlight.VliegtuigID);
        VliegveldModel? currentVertrekVliegveld = vliegveldLogic.GetById(currentFlight.VertrekLocatie);
        VliegveldModel? currentAankomstVliegveld = vliegveldLogic.GetById(currentFlight.AankomstLocatie);

        Console.Write($"Nieuw vliegtuigmodel (huidig: {currentVliegtuig?.Model}): ");
        string vliegtuigModelInput = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe vertrekvliegveld-afkorting (huidig: {currentVertrekVliegveld?.Afkorting}): ");
        string vertrekAfkortingInput = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe aankomstvliegveld-afkorting (huidig: {currentAankomstVliegveld?.Afkorting}): ");
        string aankomstAfkortingInput = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe vertrek datum (huidig: {currentFlight.VertrekDatum}, yyyy-MM-dd): ");
        string vertrekDatum = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe vertrek tijd (huidig: {currentFlight.VertrekTijd}, HH:mm): ");
        string vertrekTijd = Console.ReadLine() ?? "";
        Console.Write($"Nieuwe vertraging (huidig: {currentFlight.Vertraging}, HH:mm): ");
        string vertraging = Console.ReadLine() ?? "";
        Console.Write($"Nieuw vluchtnummer (huidig: {currentFlight.VluchtNummer}): ");
        string nieuweVluchtNummerInput = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(vliegtuigModelInput)) vliegtuigModelInput = currentVliegtuig?.Model ?? "";
        if (string.IsNullOrWhiteSpace(vertrekAfkortingInput)) vertrekAfkortingInput = currentVertrekVliegveld?.Afkorting ?? "";
        if (string.IsNullOrWhiteSpace(aankomstAfkortingInput)) aankomstAfkortingInput = currentAankomstVliegveld?.Afkorting ?? "";

        if (CheckFlight(vliegtuigModelInput, vertrekAfkortingInput, aankomstAfkortingInput, vertrekDatum, vertrekTijd, vertraging, nieuweVluchtNummerInput, id, out VluchtModel flight))
        {
            if (vluchtLogic.Update(flight))
            {
                Console.WriteLine("Vlucht aangepast.");
            }
            else
            {
                Console.WriteLine($"Er bestaat geen vlucht met vluchtnummer {vluchtNummer}.");
            }
        }
        return;
    }

    private static bool CheckFlight(string vliegtuigModelInput, string vertrekAfkortingInput, string aankomstAfkortingInput,
        string vertrekDatum, string vertrekTijd, string vertraging, string vluchtNummerInput, int id,
        out VluchtModel flight)
    {
        bool isValid = true;
        List<VliegtuigModel> gevondenVliegtuigen = new List<VliegtuigModel>();
        string gezochtVliegtuigModel = vliegtuigModelInput.Trim().ToLower();
        foreach (VliegtuigModel vliegtuig in vliegtuigLogic.GetAll())
        {
            if (vliegtuig.Model.ToLower() == gezochtVliegtuigModel)
            {
                gevondenVliegtuigen.Add(vliegtuig);
            }
        }

        List<VliegveldModel> gevondenVertrekVliegvelden = new List<VliegveldModel>();
        string gezochtVertrekAfkorting = vertrekAfkortingInput.Trim().ToLower();
        foreach (VliegveldModel vliegveld in vliegveldLogic.GetAll())
        {
            if (vliegveld.Afkorting.ToLower() == gezochtVertrekAfkorting)
            {
                gevondenVertrekVliegvelden.Add(vliegveld);
            }
        }

        List<VliegveldModel> gevondenAankomstVliegvelden = new List<VliegveldModel>();
        string gezochtAankomstAfkorting = aankomstAfkortingInput.Trim().ToLower();
        foreach (VliegveldModel vliegveld in vliegveldLogic.GetAll())
        {
            if (vliegveld.Afkorting.ToLower() == gezochtAankomstAfkorting)
            {
                gevondenAankomstVliegvelden.Add(vliegveld);
            }
        }

        bool hasVluchtNummer = int.TryParse(vluchtNummerInput, out int vluchtNummer);
        int vliegtuigID = gevondenVliegtuigen.Count == 1 ? gevondenVliegtuigen[0].ID : 0;
        int vertrekLocatie = gevondenVertrekVliegvelden.Count == 1 ? gevondenVertrekVliegvelden[0].ID : 0;
        int aankomstLocatie = gevondenAankomstVliegvelden.Count == 1 ? gevondenAankomstVliegvelden[0].ID : 0;

        if (gevondenVliegtuigen.Count != 1)
        {
            Console.WriteLine("Voer een bestaand en uniek vliegtuigmodel in.");
            isValid = false;
        }

        if (gevondenVertrekVliegvelden.Count != 1)
        {
            Console.WriteLine("Voer een bestaande vertrekvliegveld-afkorting in.");
            isValid = false;
        }

        if (gevondenAankomstVliegvelden.Count != 1)
        {
            Console.WriteLine("Voer een bestaande aankomstvliegveld-afkorting in.");
            isValid = false;
        }
        else if (gevondenVertrekVliegvelden.Count == 1 && !vluchtLogic.AreDifferentLocations(vertrekLocatie, aankomstLocatie))
        {
            Console.WriteLine("De vertrek- en aankomstvliegvelden moeten verschillend zijn.");
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
    private static VluchtModel? ZoekVlucht()
    {
        Console.Write("Zoek vlucht op vluchtnummer: ");
        string zoekterm = Console.ReadLine() ?? "";

        List<VluchtModel> gevonden = vluchtLogic.Zoek(zoekterm);

        if (gevonden.Count == 0)
        {
            Console.WriteLine($"Geen vlucht gevonden met \"{zoekterm}\". Controleer de zoekterm en probeer het opnieuw.");
            return null;
        }

        if (gevonden.Count == 1)
        {
            return gevonden[0];
        }

        Console.WriteLine("Meerdere vluchten gevonden:");
        for (int i = 0; i < gevonden.Count; i += 1)
        {
            Console.WriteLine($"{i + 1}. Vlucht {gevonden[i].VluchtNummer} op {gevonden[i].VertrekDatum} om {gevonden[i].VertrekTijd}");
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
    private static void ToonVlucht(VluchtModel vlucht)
    {
        VliegtuigModel? vliegtuig = vliegtuigLogic.GetById(vlucht.VliegtuigID);
        VliegveldModel? vertrekVliegveld = vliegveldLogic.GetById(vlucht.VertrekLocatie);
        VliegveldModel? aankomstVliegveld = vliegveldLogic.GetById(vlucht.AankomstLocatie);

        Console.WriteLine("");
        Console.WriteLine("Gevonden vlucht:");
        Console.WriteLine($"  Vluchtnummer:          {vlucht.VluchtNummer}");
        Console.WriteLine($"  Vliegtuigmodel:        {vliegtuig?.Model ?? "Onbekend"}");
        Console.WriteLine($"  Vertrekvliegveld:      {vertrekVliegveld?.Afkorting ?? "Onbekend"}");
        Console.WriteLine($"  Aankomstvliegveld:     {aankomstVliegveld?.Afkorting ?? "Onbekend"}");
        Console.WriteLine($"  Vertrekdatum:          {vlucht.VertrekDatum}");
        Console.WriteLine($"  Vertrektijd:           {vlucht.VertrekTijd}");
        Console.WriteLine($"  Vertraging:            {vlucht.Vertraging}");
        Console.WriteLine($"  Prijs:                 {vlucht.Prijs}");
        Console.WriteLine("");
    }
}