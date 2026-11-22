static class VluchtBoeken
{
    static private PassagierLogic passagierLogic = new PassagierLogic();
    static private ReserveringLogic reserveringLogic = new ReserveringLogic();

    public static void Start(int vluchtId, long vluchtNummer)
    {
        AccountModel? currentAccount = AccountsLogic.CurrentAccount;

        Console.WriteLine($"U heeft vlucht nummer {vluchtNummer} geselecteerd.");

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

            // Stoel kiezen voor deze passagier
            int stoelId = KiesStoel(vluchtId, gekozenStoelen);
            if (stoelId == 0)
            {
                Console.WriteLine("Er zijn geen vrije stoelen meer op deze vlucht.");
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
                {
                    meerPassagiers = false;
                }
            }
        }

        // Geen enkele stoel gekozen, dus ook geen reservering
        if (gekozenStoelen.Count == 0)
        {
            Console.WriteLine("De reservering is niet gemaakt.");
            return;
        }

        // Eerst de prijs laten zien, dan pas bevestigen
        PrijsOverzicht.Start(vluchtId, gekozenStoelen);
        if (!VraagBevestiging())
        {
            Console.WriteLine("De reservering is geannuleerd.");
            return;
        }

        // Reservering bevestigen
        bool gelukt = reserveringLogic.MaakReservering(vluchtId, gekozenStoelen);

        if (gelukt)
        {
            Console.WriteLine("");
            Console.WriteLine("Uw reservering is bevestigd!");
            Console.WriteLine("Uw boekingscode is: " + reserveringLogic.LaatsteBoekingsCode);
        }
        else
        {
            Console.WriteLine("De reservering is mislukt. Probeer het opnieuw.");
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

    // Laat de stoelen van de vlucht zien en laat de gebruiker er één kiezen.
    // Geeft het ID van de gekozen stoel terug, of 0 als er geen vrije stoel meer is.
    private static int KiesStoel(int vluchtId, List<ReserveringsStoelModel> gekozenStoelen)
    {
        List<StoelModel> stoelen = reserveringLogic.GetStoelenVoorVlucht(vluchtId);

        // Kijk eerst of er nog minstens één vrije stoel is
        bool vrijeStoelGevonden = false;
        foreach (StoelModel stoel in stoelen)
        {
            if (!IsBezet(vluchtId, stoel.ID, gekozenStoelen))
            {
                vrijeStoelGevonden = true;
            }
        }

        if (!vrijeStoelGevonden)
        {
            return 0;
        }

        while (true)
        {
            Console.WriteLine("");
            Console.WriteLine("=== Stoelplattegrond ===");
            Console.WriteLine("X = bezet, zonder X = vrij");
            string vorigeRij = "";
            foreach (StoelModel stoel in stoelen)
            {
                // Begint een nieuwe regel wanneer het rijnummer verandert
                if (stoel.RijNummer != vorigeRij)
                {
                    Console.WriteLine();
                    Console.Write($"Rij {stoel.RijNummer}: ");
                    vorigeRij = stoel.RijNummer;
                }

                // Laat zien of de stoel bezet is
                if (IsBezet(vluchtId, stoel.ID, gekozenStoelen) == true)
                {
                    Console.Write($"[{stoel.StoelNummer}:X] ");
                }
                else
                {
                    Console.Write($"[{stoel.StoelNummer}] ");
                }
            }
            Console.WriteLine();
            Console.Write("Kies een stoelnummer (bijvoorbeeld 15A): ");
            string invoer = Console.ReadLine() ?? "";

            // Zoek de stoel die de gebruiker heeft gekozen
            StoelModel? gekozenStoel = null;
            foreach (StoelModel stoel in stoelen)
            {
                if (stoel.StoelNummer.ToLower() == invoer.Trim().ToLower())
                {
                    gekozenStoel = stoel;
                }
            }

            if (gekozenStoel == null)
            {
                Console.WriteLine("Deze stoel bestaat niet. Kies een stoel uit de lijst.");
            }
            else if (IsBezet(vluchtId, gekozenStoel.ID, gekozenStoelen))
            {
                Console.WriteLine("Deze stoel is al bezet. Kies een andere stoel.");
            }
            else
            {
                return gekozenStoel.ID;
            }
        }
    }

    // Een stoel is bezet als iemand anders hem al heeft gereserveerd,
    // of als je hem in deze reservering al voor een andere passagier hebt gekozen
    private static bool IsBezet(int vluchtId, int stoelId, List<ReserveringsStoelModel> gekozenStoelen)
    {
        if (reserveringLogic.IsStoelBezet(vluchtId, stoelId))
        {
            return true;
        }

        foreach (ReserveringsStoelModel gekozen in gekozenStoelen)
        {
            if (gekozen.StoelID == stoelId)
            {
                return true;
            }
        }

        return false;
    }


    // Vraagt alle passagiersgegevens. Elk veld wordt meteen gecontroleerd
    // en opnieuw gevraagd tot het goed is.
    private static PassagierModel VraagPassagier(int? gebruikerId)
    {
        Console.WriteLine("Vul uw passagiersgegevens in:");

        return new PassagierModel
        {
            GebruikerID = gebruikerId,
            Naam = Invoer.VraagTotGeldig("Voornaam: ", passagierLogic.IsValidNaam, "Voornaam is verplicht en mag alleen letters bevatten."),
            Achternaam = Invoer.VraagTotGeldig("Achternaam: ", passagierLogic.IsValidAchternaam, "Achternaam is verplicht en mag alleen letters bevatten."),
            Email = Invoer.VraagTotGeldig("E-mailadres: ", passagierLogic.IsValidEmail, "Ongeldig e-mailadres, bijvoorbeeld naam@voorbeeld.nl."),
            PaspoortNummer = Invoer.VraagTotGeldig("Paspoortnummer: ", passagierLogic.IsValidPaspoortNummer, "Het paspoortnummer moet 5-20 letters of cijfers bevatten."),
            GeboorteDatum = Invoer.VraagTotGeldig("Geboortedatum (jjjj-mm-dd): ", passagierLogic.IsValidGeboorteDatum, "Ongeldige geboortedatum, gebruik jjjj-mm-dd en een datum in het verleden."),
            Telefoonnummer = Invoer.VraagTotGeldig("Telefoonnummer: ", passagierLogic.IsValidTelefoonnummer, "Het telefoonnummer moet 7-15 cijfers bevatten."),
            PaspoortVerloopDatum = Invoer.VraagTotGeldig("Paspoortverloopdatum (jjjj-mm-dd): ", passagierLogic.IsValidPaspoortVerloopDatum, "Ongeldige verloopdatum, gebruik jjjj-mm-dd en een datum in de toekomst.")
        };
    }
}