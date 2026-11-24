static class VluchtMenu
{
    static private VluchtLogic vluchtLogic = new VluchtLogic();
    static private VliegtuigLogic vliegtuigLogic = new VliegtuigLogic();
    static private VliegveldLogic vliegveldLogic = new VliegveldLogic();

    public static void Start()
    {
        string feedback = "";

        bool menuRunning = true;
        while (menuRunning)
        {
            string menuInput = MenuOptions(feedback);

            if (menuInput == "1")
                feedback = Overzicht();
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
        string[] menuOptions = { "1 - Overzicht", "2 - Toevoegen", "3 - Aanpassen", "0 - Terug" };
        int selectedIndex = 0;

        while (true)
        {
            Menu.PrintTitel();

            if (feedback != "")
                Console.WriteLine(feedback);
                Console.WriteLine("");

            Console.WriteLine("Vluchten beheren");
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

    public static string Overzicht()
    {
        List<VluchtModel> flights = vluchtLogic.GetAll();
        if (flights.Count == 0)
        {
            return "Geen vluchten gevonden.";
        }

        string allFlights = "";
        foreach (VluchtModel flight in flights)
        {
            VliegtuigModel? vliegtuig = vliegtuigLogic.GetById(flight.VliegtuigID);
            VliegveldModel? vertrekVliegveld = vliegveldLogic.GetById(flight.VertrekLocatie);
            VliegveldModel? aankomstVliegveld = vliegveldLogic.GetById(flight.AankomstLocatie);
            allFlights += $"Vliegtuigmodel: {vliegtuig?.Model ?? "Onbekend"}, Vertrekvliegveld: {vertrekVliegveld?.Afkorting ?? "Onbekend"}, Aankomstvliegveld: {aankomstVliegveld?.Afkorting ?? "Onbekend"}, Vertrek datum: {flight.VertrekDatum}, Vertrek tijd: {flight.VertrekTijd}, Vertraging (HH:mm): {flight.Vertraging}, Vluchtnummer: {flight.VluchtNummer}\n";
        }
        return allFlights;
    }

    public static string Toevoegen()
    {
        Console.Write("Vliegtuigmodel: ");
        string vliegtuigModelInput = Console.ReadLine() ?? "";
        Console.Write("Vertrekvliegveld-afkorting: ");
        string vertrekAfkortingInput = Console.ReadLine() ?? "";
        Console.Write("Aankomstvliegveld-afkorting: ");
        string aankomstAfkortingInput = Console.ReadLine() ?? "";
        Console.Write("Vertrek datum (dd-MM-yyyy): ");
        string vertrekDatum = Console.ReadLine() ?? "";
        Console.Write("Vertrek tijd (HH:mm): ");
        string vertrekTijd = Console.ReadLine() ?? "";
        Console.Write("Vertraging (HH:mm): ");
        string vertraging = Console.ReadLine() ?? "";
        Console.Write("Vluchtnummer: ");
        string vluchtNummerInput = Console.ReadLine() ?? "";
        Console.Write("Prijs: ");
        string prijsNummerInput = Console.ReadLine() ?? "";

        if (CheckFlight(vliegtuigModelInput, vertrekAfkortingInput, aankomstAfkortingInput, vertrekDatum, vertrekTijd, vertraging, vluchtNummerInput, prijsNummerInput, 0, out VluchtModel flight))
        {
            vluchtLogic.Add(flight);
            return "Vlucht toegevoegd.";
        }
        return "Vlucht niet toegevoegd; er ging iets fout.";
    }

    public static string Aanpassen()
    {
        List<VluchtModel> flights = vluchtLogic.GetAll();
        string vluchtNummerInput = Invoer.VraagTotGeldig(
            "Vluchtnummer van de vlucht: ",
            input => int.TryParse(input, out int nummer) && nummer > 0 &&
                flights.Any(flight => flight.VluchtNummer == nummer),
            "Voer een bestaand vluchtnummer in.");
        int.TryParse(vluchtNummerInput, out int vluchtNummer);

        VluchtModel? currentFlight = null;
        foreach (VluchtModel gevondenVlucht in flights)
        {
            if (gevondenVlucht.VluchtNummer == vluchtNummer)
            {
                currentFlight = gevondenVlucht;
                break;
            }
        }

        if (currentFlight == null)
        {
            return $"Er bestaat geen vlucht met vluchtnummer {vluchtNummer}.";
        }
        int id = currentFlight.ID;

        ToonVlucht(currentFlight);

        VliegtuigModel? currentVliegtuig = vliegtuigLogic.GetById(currentFlight.VliegtuigID);
        VliegveldModel? currentVertrekVliegveld = vliegveldLogic.GetById(currentFlight.VertrekLocatie);
        VliegveldModel? currentAankomstVliegveld = vliegveldLogic.GetById(currentFlight.AankomstLocatie);

        string vliegtuigModelInput = Invoer.VraagTotGeldig(
            $"Nieuw vliegtuigmodel [{currentVliegtuig?.Model}]: ",
            input => string.IsNullOrWhiteSpace(input) ||
                vliegtuigLogic.GetAll().Count(vliegtuig =>
                    string.Equals(vliegtuig.Model, input, StringComparison.OrdinalIgnoreCase)) == 1,
            "Voer een bestaand en uniek vliegtuigmodel in.");
        string vertrekAfkortingInput = Invoer.VraagTotGeldig(
            $"Nieuwe vertrekvliegveld-afkorting [{currentVertrekVliegveld?.Afkorting}]: ",
            input => string.IsNullOrWhiteSpace(input) || vliegveldLogic.Zoek(input).Count == 1,
            "Voer een bestaande vertrekvliegveld-afkorting in.");
        string aankomstAfkortingInput = Invoer.VraagTotGeldig(
            $"Nieuwe aankomstvliegveld-afkorting [{currentAankomstVliegveld?.Afkorting}]: ",
            input => string.IsNullOrWhiteSpace(input) || vliegveldLogic.Zoek(input).Count == 1,
            "Voer een bestaande aankomstvliegveld-afkorting in.");
        string vertrekDatum = Invoer.VraagTotGeldig(
            $"Nieuwe vertrekdatum [{currentFlight.VertrekDatum}] (dd-MM-yyyy): ",
            input => string.IsNullOrWhiteSpace(input) || vluchtLogic.IsValidVertrekDatum(input),
            "De vertrekdatum moet geldig zijn en het formaat dd-MM-yyyy hebben.");
        string vertrekTijd = Invoer.VraagTotGeldig(
            $"Nieuwe vertrektijd [{currentFlight.VertrekTijd}] (HH:mm): ",
            input => string.IsNullOrWhiteSpace(input) || vluchtLogic.IsValidVertrekTijd(input),
            "De vertrektijd moet geldig zijn en het formaat HH:mm hebben.");
        string vertraging = Invoer.VraagTotGeldig(
            $"Nieuwe vertraging [{currentFlight.Vertraging}] (HH:mm): ",
            input => string.IsNullOrWhiteSpace(input) || vluchtLogic.IsValidVertraging(input),
            "De vertraging moet geldig zijn en het formaat HH:mm hebben.");
        string nieuweVluchtNummerInput = Invoer.VraagTotGeldig(
            $"Nieuw vluchtnummer [{currentFlight.VluchtNummer}]: ",
            input => string.IsNullOrWhiteSpace(input) ||
                (int.TryParse(input, out int nummer) && vluchtLogic.IsValidVluchtNummer(nummer, id)),
            "Voer een uniek vluchtnummer groter dan nul in.");
        string nieuwePrijsNummerInput = Invoer.VraagTotGeldig(
            $"Nieuwe prijs [{currentFlight.Prijs}]: ",
            input => string.IsNullOrWhiteSpace(input) ||
                (double.TryParse(input, out double prijs) && double.IsFinite(prijs) && prijs >= 0),
            "Voer een geldige prijs van nul of hoger in.");

        if (string.IsNullOrWhiteSpace(vliegtuigModelInput)) vliegtuigModelInput = currentVliegtuig?.Model ?? "";
        if (string.IsNullOrWhiteSpace(vertrekAfkortingInput)) vertrekAfkortingInput = currentVertrekVliegveld?.Afkorting ?? "";
        if (string.IsNullOrWhiteSpace(aankomstAfkortingInput)) aankomstAfkortingInput = currentAankomstVliegveld?.Afkorting ?? "";
        if (string.IsNullOrWhiteSpace(vertrekDatum)) vertrekDatum = currentFlight.VertrekDatum;
        if (string.IsNullOrWhiteSpace(vertrekTijd)) vertrekTijd = currentFlight.VertrekTijd;
        if (string.IsNullOrWhiteSpace(vertraging)) vertraging = currentFlight.Vertraging;
        if (string.IsNullOrWhiteSpace(nieuweVluchtNummerInput)) nieuweVluchtNummerInput = currentFlight.VluchtNummer.ToString();
        if (string.IsNullOrWhiteSpace(nieuwePrijsNummerInput)) nieuwePrijsNummerInput = currentFlight.Prijs.ToString();

        if (CheckFlight(vliegtuigModelInput, vertrekAfkortingInput, aankomstAfkortingInput, vertrekDatum, vertrekTijd, vertraging, nieuweVluchtNummerInput, nieuwePrijsNummerInput, id, out VluchtModel flight))
        {
            if (vluchtLogic.Update(flight))
            {
                return "Vlucht aangepast.";
            }
            else
            {
                return $"Er bestaat geen vlucht met vluchtnummer {vluchtNummer}.";
            }
        }
        return "Vlucht niet aangepast; er ging iets fout.";
    }

    private static bool CheckFlight(string vliegtuigModelInput, string vertrekAfkortingInput, string aankomstAfkortingInput,
        string vertrekDatum, string vertrekTijd, string vertraging, string vluchtNummerInput, string prijsNummerInput, int id,
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
        bool hasPrijs = double.TryParse(prijsNummerInput, out double prijs);
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
            Console.WriteLine("De vertrekdatum moet geldig zijn en het formaat dd-MM-yyyy hebben.");
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

        if (!hasPrijs || !double.IsFinite(prijs) || prijs < 0)
        {
            Console.WriteLine("Voer een geldige prijs van nul of hoger in.");
            isValid = false;
        }

        flight = new VluchtModel(id, vliegtuigID, vertrekLocatie, aankomstLocatie, vertrekDatum, vertrekTijd, vertraging, vluchtNummer, prijs);
        return isValid;
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