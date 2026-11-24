static class VluchtBoeken
{
    static private PassagierLogic passagierLogic = new PassagierLogic();
    static private ReserveringLogic reserveringLogic = new ReserveringLogic();
    static private string gekozenVlucht = "";

    public static string Start(int vluchtId, long vluchtNummer, string keuzeVlucht)
    {
        AccountModel? currentAccount = AccountsLogic.CurrentAccount;
        gekozenVlucht = keuzeVlucht;

        ToonPassagier("");

        // Hierin komen de gekozen stoelen van alle passagiers
        List<ReserveringsStoelModel> gekozenStoelen = new List<ReserveringsStoelModel>();

        bool meerPassagiers = true;

        while (meerPassagiers)
        {
            PassagierModel passagier = VraagPassagier(currentAccount?.Id);

            if (!passagierLogic.Add(passagier))
            {
                Console.WriteLine("De passagier kon niet worden opgeslagen. Probeer het opnieuw.");
                continue;
            }
            Console.WriteLine("Passagier toegevoegd.");

            int stoelId = StoelkaartMenu.Start(vluchtId, gekozenStoelen);
            if (stoelId == 0)
            {
                meerPassagiers = false;
            }
            else
            {
                ReserveringsStoelModel reserveringsStoel = new ReserveringsStoelModel();
                reserveringsStoel.PassagierID = passagier.ID;
                reserveringsStoel.StoelID = stoelId;
                reserveringsStoel.ExtraBagage = VraagExtraBagage();
                gekozenStoelen.Add(reserveringsStoel);

                Console.Write("Wilt u nog een passagier toevoegen? (J/N): ");
                string keuze = Console.ReadLine() ?? "";
                if (keuze.Trim().ToLower() == "n")
                    meerPassagiers = false;
            }
        }

        // Geen enkele stoel gekozen, dus ook geen reservering
        if (gekozenStoelen.Count == 0)
        {
            return "De reservering is niet gemaakt.";
        }

        // Eerst de prijs laten zien, dan pas bevestigen
        PrijsOverzicht.Start(vluchtId, gekozenStoelen);
        if (!VraagBevestiging())
        {
            return "De reservering is geannuleerd.";
        }

        // Reservering bevestigen
        if (reserveringLogic.MaakReservering(vluchtId, gekozenStoelen))
        {
            return $"\nUw reservering is bevestigd!\nUw boekingscode is: {reserveringLogic.LaatsteBoekingsCode}";
        }
        else
        {
            return "De reservering is mislukt. Probeer het opnieuw.";
        }
    }

    // Vraagt of de passagier extra bagage wil. Geeft 1 (ja) of 0 (nee) terug.
    private static int VraagExtraBagage()
    {
        string bagagePrijs = PrijsLogic.ExtraBagagePrijs.ToString("N2", new System.Globalization.CultureInfo("nl-NL"));
        Console.Write($"Wilt u extra bagage (+ € {bagagePrijs})? (J/N): ");
        string keuze = Console.ReadLine() ?? "";

        return keuze.Trim().ToLower() == "j" ? 1 : 0;
    }

    // Vraagt of de gebruiker de reservering wil bevestigen
    private static bool VraagBevestiging()
    {
        Console.Write("Wilt u deze reservering bevestigen? (J/N): ");
        string keuze = Console.ReadLine() ?? "";

        return keuze.Trim().ToLower() == "j";
    }

    // Vraagt alle passagiersgegevens. Elk veld wordt meteen gecontroleerd
    // en opnieuw gevraagd tot het goed is.
    private static PassagierModel VraagPassagier(int? gebruikerId)
    {
        string mijnKeuzes = "";
        ToonPassagier(mijnKeuzes);
        string naam = Invoer.VraagTotGeldig(
            "Voornaam: ",
            passagierLogic.IsValidNaam,
            "Voornaam is verplicht en mag alleen letters bevatten.");
        mijnKeuzes += $"Voornaam: {naam}\n";

        ToonPassagier(mijnKeuzes);
        string achternaam = Invoer.VraagTotGeldig(
            "Achternaam: ",
            passagierLogic.IsValidAchternaam,
            "Achternaam is verplicht en mag alleen letters bevatten.");
        mijnKeuzes += $"Achternaam: {achternaam}\n";

        ToonPassagier(mijnKeuzes);
        string email = Invoer.VraagTotGeldig(
            "E-mailadres: ",
            passagierLogic.IsValidEmail,
            "Ongeldig e-mailadres, bijvoorbeeld naam@voorbeeld.nl.");
        mijnKeuzes += $"E-mailadres: {email}\n";

        ToonPassagier(mijnKeuzes);
        string paspoortNummer = Invoer.VraagTotGeldig(
            "Paspoortnummer: ",
            passagierLogic.IsValidPaspoortNummer,
            "Het paspoortnummer moet 5-20 letters of cijfers bevatten.");
        mijnKeuzes += $"Paspoortnummer: {paspoortNummer}\n";

        ToonPassagier(mijnKeuzes);
        string geboorteDatum = Invoer.VraagTotGeldig(
            "Geboortedatum (dd-MM-yyyy): ",
            passagierLogic.IsValidGeboorteDatum,
            "Ongeldige geboortedatum, gebruik dd-MM-yyyy en een datum in het verleden.");
        mijnKeuzes += $"Geboortedatum: {geboorteDatum}\n";

        ToonPassagier(mijnKeuzes);
        string telefoonnummer = Invoer.VraagTotGeldig(
            "Telefoonnummer: ",
            passagierLogic.IsValidTelefoonnummer,
            "Het telefoonnummer moet 7-15 cijfers bevatten.");
        mijnKeuzes += $"Telefoonnummer: {telefoonnummer}\n";

        ToonPassagier(mijnKeuzes);
        string paspoortVerloopDatum = Invoer.VraagTotGeldig(
            "Paspoortverloopdatum (dd-MM-yyyy): ",
            passagierLogic.IsValidPaspoortVerloopDatum,
            "Ongeldige verloopdatum, gebruik dd-MM-yyyy en een datum in de toekomst.");
        mijnKeuzes += $"Paspoortverloopdatum: {paspoortVerloopDatum}\n";

        ToonPassagier(mijnKeuzes);
        return new PassagierModel
        {
            GebruikerID = gebruikerId,
            Naam = naam,
            Achternaam = achternaam,
            Email = email,
            PaspoortNummer = paspoortNummer,
            GeboorteDatum = geboorteDatum,
            Telefoonnummer = telefoonnummer,
            PaspoortVerloopDatum = paspoortVerloopDatum
        };
    }

    private static void ToonPassagier(string mijnKeuzes)
    {
        Menu.PrintTitel();
        Console.WriteLine("Gekozen vlucht:");
        Console.WriteLine(gekozenVlucht);
        if (mijnKeuzes != "")
        {
            Console.WriteLine("Nieuwe passagier:");
            Console.WriteLine(mijnKeuzes);
        }
    }
}