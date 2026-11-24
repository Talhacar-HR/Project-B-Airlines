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
            {
                Console.WriteLine(feedback);
                Console.WriteLine("");
            }

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
        string mijnKeuzes = "";
        VliegtuigModel? gekozenVliegtuig = KeuzeMenu.KiesVliegtuig();
        if (gekozenVliegtuig is null)
            return "Geen vliegtuig gekozen; vlucht niet toegevoegd.";
        mijnKeuzes += $"Gekozen vliegtuig: {gekozenVliegtuig.Model}\n";

        string vliegtuigModelInput = gekozenVliegtuig.Model;
        VliegveldModel? vertrekVliegveld = KeuzeMenu.KiesVliegveld("vertrekvliegveld");
        if (vertrekVliegveld is null)
            return "Geen vertrekvliegveld gekozen; vlucht niet toegevoegd.";
        mijnKeuzes += $"Gekozen vertrekvliegveld: {vertrekVliegveld.Naam}\n";

        VliegveldModel? aankomstVliegveld = KeuzeMenu.KiesVliegveld(
            "aankomstvliegveld",
            uitgeslotenVliegveldId: vertrekVliegveld.ID);
        if (aankomstVliegveld is null)
            return "Geen aankomstvliegveld gekozen; vlucht niet toegevoegd.";
        mijnKeuzes += $"Gekozen aankomstvliegveld: {aankomstVliegveld.Naam}\n";

        string vertrekAfkortingInput = vertrekVliegveld.Afkorting;
        string aankomstAfkortingInput = aankomstVliegveld.Afkorting;
        string vertrekDatum = Invoer.VraagTotGeldig(
            "Vertrekdatum (dd-MM-yyyy): ",
            vluchtLogic.IsValidVertrekDatum,
            "De vertrekdatum moet geldig zijn en het formaat dd-MM-yyyy hebben.");
        mijnKeuzes += $"Gekozen vertrekdatum: {vertrekDatum}\n";
        string vertrekTijd = Invoer.VraagTotGeldig(
            "Vertrektijd (HH:mm): ",
            vluchtLogic.IsValidVertrekTijd,
            "De vertrektijd moet geldig zijn en het formaat HH:mm hebben.");
        mijnKeuzes += $"Gekozen vertrektijd: {vertrekTijd}\n";
        string vertraging = Invoer.VraagTotGeldig(
            "Vertraging (HH:mm): ",
            vluchtLogic.IsValidVertraging,
            "De vertraging moet geldig zijn en het formaat HH:mm hebben.");
        mijnKeuzes += $"Gekozen vertraging: {vertraging}\n";
        string vluchtNummerInput = Invoer.VraagTotGeldig(
            "Vluchtnummer: ",
            input => int.TryParse(input, out int nummer) && vluchtLogic.IsValidVluchtNummer(nummer, 0),
            "Voer een uniek vluchtnummer groter dan nul in.");
        mijnKeuzes += $"Gekozen vluchtnummer: {vluchtNummerInput}\n";
        string prijsNummerInput = Invoer.VraagTotGeldig(
            "Prijs: ",
            input => double.TryParse(input, out double prijs) && double.IsFinite(prijs) && prijs >= 0,
            "Voer een geldige prijs van nul of hoger in.");
        mijnKeuzes += $"Gekozen prijs: {prijsNummerInput}\n";

        if (CheckFlight(vliegtuigModelInput, vertrekAfkortingInput, aankomstAfkortingInput, vertrekDatum, vertrekTijd, vertraging, vluchtNummerInput, prijsNummerInput, 0, out VluchtModel flight))
        {
            vluchtLogic.Add(flight);
            return $"Vlucht toegevoegd.";
        }
        return "Vlucht niet toegevoegd; er ging iets fout.";
    }

    public static string Aanpassen()
    {
        VluchtModel? currentFlight = KeuzeMenu.KiesVlucht(aanpassen: true);
        if (currentFlight == null)
            return "Geen vlucht gekozen; vlucht niet aangepast.";

        string mijnKeuzes = "";
        int vluchtNummer = currentFlight.VluchtNummer;
        int id = currentFlight.ID;

        ToonVlucht(currentFlight, mijnKeuzes);

        VliegtuigModel? currentVliegtuig = vliegtuigLogic.GetById(currentFlight.VliegtuigID);
        VliegveldModel? currentVertrekVliegveld = vliegveldLogic.GetById(currentFlight.VertrekLocatie);
        VliegveldModel? currentAankomstVliegveld = vliegveldLogic.GetById(currentFlight.AankomstLocatie);

        VliegtuigModel? gekozenVliegtuig = KeuzeMenu.KiesVliegtuig(currentVliegtuig?.ID, aanpassen: true);
        if (gekozenVliegtuig is null)
            return "Vlucht niet aangepast; er is geen vliegtuig gekozen.";

        string vliegtuigModelInput = gekozenVliegtuig.Model;
        mijnKeuzes += $"Gekozen vliegtuig: {gekozenVliegtuig.Model}\n";
        ToonVlucht(currentFlight, mijnKeuzes);

        VliegveldModel? vertrekVliegveld = KeuzeMenu.KiesVliegveld(
            "vertrekvliegveld",
            currentVertrekVliegveld?.ID,
            aanpassen: true);
        if (vertrekVliegveld is null)
            return "Vlucht niet aangepast; er is geen vertrekvliegveld gekozen.";
        mijnKeuzes += $"Gekozen vertrekvliegveld: {vertrekVliegveld.Naam}\n";
        ToonVlucht(currentFlight, mijnKeuzes);

        VliegveldModel? aankomstVliegveld = KeuzeMenu.KiesVliegveld(
            "aankomstvliegveld",
            currentAankomstVliegveld?.ID,
            vertrekVliegveld.ID,
            aanpassen: true);
        if (aankomstVliegveld is null)
            return "Vlucht niet aangepast; er is geen aankomstvliegveld gekozen.";
        mijnKeuzes += $"Gekozen aankomstvliegveld: {aankomstVliegveld.Naam}\n";
        ToonVlucht(currentFlight, mijnKeuzes);

        string vertrekAfkortingInput = vertrekVliegveld.Afkorting;
        string aankomstAfkortingInput = aankomstVliegveld.Afkorting;
        string vertrekDatum = Invoer.VraagTotGeldig(
            $"Nieuwe vertrekdatum [{currentFlight.VertrekDatum}] (dd-MM-yyyy): ",
            input => string.IsNullOrWhiteSpace(input) || vluchtLogic.IsValidVertrekDatum(input),
            "De vertrekdatum moet geldig zijn en het formaat dd-MM-yyyy hebben.");
        if (string.IsNullOrWhiteSpace(vertrekDatum)) vertrekDatum = currentFlight.VertrekDatum;
        mijnKeuzes += $"Gekozen vertrekdatum: {vertrekDatum}\n";
        ToonVlucht(currentFlight, mijnKeuzes);

        string vertrekTijd = Invoer.VraagTotGeldig(
            $"Nieuwe vertrektijd [{currentFlight.VertrekTijd}] (HH:mm): ",
            input => string.IsNullOrWhiteSpace(input) || vluchtLogic.IsValidVertrekTijd(input),
            "De vertrektijd moet geldig zijn en het formaat HH:mm hebben.");
        if (string.IsNullOrWhiteSpace(vertrekTijd)) vertrekTijd = currentFlight.VertrekTijd;
        mijnKeuzes += $"Gekozen vertrektijd: {vertrekTijd}\n";
        ToonVlucht(currentFlight, mijnKeuzes);

        string vertraging = Invoer.VraagTotGeldig(
            $"Nieuwe vertraging [{currentFlight.Vertraging}] (HH:mm): ",
            input => string.IsNullOrWhiteSpace(input) || vluchtLogic.IsValidVertraging(input),
            "De vertraging moet geldig zijn en het formaat HH:mm hebben.");
        if (string.IsNullOrWhiteSpace(vertraging)) vertraging = currentFlight.Vertraging;
        mijnKeuzes += $"Gekozen vertraging: {vertraging}\n";
        ToonVlucht(currentFlight, mijnKeuzes);

        string nieuweVluchtNummerInput = Invoer.VraagTotGeldig(
            $"Nieuw vluchtnummer [{currentFlight.VluchtNummer}]: ",
            input => string.IsNullOrWhiteSpace(input) ||
                (int.TryParse(input, out int nummer) && vluchtLogic.IsValidVluchtNummer(nummer, id)),
            "Voer een uniek vluchtnummer groter dan nul in.");
        if (string.IsNullOrWhiteSpace(nieuweVluchtNummerInput))
            nieuweVluchtNummerInput = currentFlight.VluchtNummer.ToString();
        mijnKeuzes += $"Gekozen vluchtnummer: {nieuweVluchtNummerInput}\n";
        ToonVlucht(currentFlight, mijnKeuzes);

        string nieuwePrijsNummerInput = Invoer.VraagTotGeldig(
            $"Nieuwe prijs [{currentFlight.Prijs}]: ",
            input => string.IsNullOrWhiteSpace(input) ||
                (double.TryParse(input, out double prijs) && double.IsFinite(prijs) && prijs >= 0),
            "Voer een geldige prijs van nul of hoger in.");
        if (string.IsNullOrWhiteSpace(nieuwePrijsNummerInput))
            nieuwePrijsNummerInput = currentFlight.Prijs.ToString();
        mijnKeuzes += $"Gekozen prijs: {nieuwePrijsNummerInput}\n";
        ToonVlucht(currentFlight, mijnKeuzes);

        Console.WriteLine("Wilt u deze aanpassingen bevestigen? (J/N)");
        if (Console.ReadLine()?.Trim().ToLowerInvariant() != "j")
            return "Vlucht niet aangepast; bevestiging geannuleerd.";

        if (CheckFlight(vliegtuigModelInput, vertrekAfkortingInput, aankomstAfkortingInput, vertrekDatum, vertrekTijd, vertraging, nieuweVluchtNummerInput, nieuwePrijsNummerInput, id, out VluchtModel flight))
        {
            if (vluchtLogic.Update(flight))
            {
                return $"Vlucht aangepast.";
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

    private static void ToonVlucht(VluchtModel vlucht, string mijnKeuzes)
    {
        VliegtuigModel? vliegtuig = vliegtuigLogic.GetById(vlucht.VliegtuigID);
        VliegveldModel? vertrekVliegveld = vliegveldLogic.GetById(vlucht.VertrekLocatie);
        VliegveldModel? aankomstVliegveld = vliegveldLogic.GetById(vlucht.AankomstLocatie);

        Menu.PrintTitel();
        Console.WriteLine("Gevonden vlucht:");
        Console.WriteLine($"  Vluchtnummer:          {vlucht.VluchtNummer}");
        Console.WriteLine($"  Vliegtuigmodel:        {vliegtuig?.Model ?? "Onbekend"}");
        Console.WriteLine($"  Vertrekvliegveld:      {vertrekVliegveld?.Naam ?? "Onbekend"}");
        Console.WriteLine($"  Aankomstvliegveld:     {aankomstVliegveld?.Naam ?? "Onbekend"}");
        Console.WriteLine($"  Vertrekdatum:          {vlucht.VertrekDatum}");
        Console.WriteLine($"  Vertrektijd:           {vlucht.VertrekTijd}");
        Console.WriteLine($"  Vertraging:            {vlucht.Vertraging}");
        Console.WriteLine($"  Prijs:                 {vlucht.Prijs}");
        Console.WriteLine("");
        if (mijnKeuzes != "")
        {
            Console.WriteLine("Gekozen wijzigingen:");
            Console.WriteLine(mijnKeuzes);
        }
    }
}