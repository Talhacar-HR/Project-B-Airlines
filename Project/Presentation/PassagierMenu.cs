static class VluchtBoeken
{
    static private PassagierLogic passagierLogic = new PassagierLogic();
    static private ReserveringLogic reserveringLogic = new ReserveringLogic();

    public static void Start(int vluchtId)
    {
        AccountModel? currentAccount = AccountsLogic.CurrentAccount;

        Console.WriteLine($"U heeft vlucht nummer {vluchtId} geselecteerd.");

        // Hierin komen de gekozen stoelen van alle passagiers
        List<ReserveringsStoelModel> gekozenStoelen = new List<ReserveringsStoelModel>();

        bool meerPassagiers = true;
        while (meerPassagiers)
        {
            Console.WriteLine("Vul uw passagiersgegevens in:");

            Console.Write("Voornaam: ");
            string naam = Console.ReadLine() ?? "";
            Console.Write("Achternaam: ");
            string achternaam = Console.ReadLine() ?? "";
            Console.Write("E-mailadres: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Paspoortnummer: ");
            string paspoortNummer = Console.ReadLine() ?? "";
            Console.Write("Geboortedatum: ");
            string geboorteDatum = Console.ReadLine() ?? "";
            Console.Write("Telefoonnummer: ");
            string telefoonnummer = Console.ReadLine() ?? "";
            Console.Write("Paspoortverloopdatum: ");
            string paspoortVerloopDatum = Console.ReadLine() ?? "";

            PassagierModel passagier = new()
            {
                GebruikerID = currentAccount?.Id,
                Naam = naam,
                Achternaam = achternaam,
                Email = email,
                PaspoortNummer = paspoortNummer,
                GeboorteDatum = geboorteDatum,
                Telefoonnummer = telefoonnummer,
                PaspoortVerloopDatum = paspoortVerloopDatum
            };

            if (!CheckPassagier(passagier))
                continue;

            passagierLogic.Add(passagier);
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
                if (keuze.Trim().Equals("N", StringComparison.OrdinalIgnoreCase))
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
        Console.Write($"Wilt u extra bagage (+ € {PrijsLogic.ExtraBagagePrijs})? (J/N): ");
        string keuze = Console.ReadLine() ?? "";

        return keuze.Trim().Equals("J", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
    }

    // Vraagt of de gebruiker de reservering wil bevestigen
    private static bool VraagBevestiging()
    {
        Console.Write("Wilt u deze reservering bevestigen? (J/N): ");
        string keuze = Console.ReadLine() ?? "";

        return keuze.Trim().Equals("J", StringComparison.OrdinalIgnoreCase);
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
            Console.WriteLine("Stoelen:");
            Console.WriteLine("ID | Rij | Stoel | Status");
            Console.WriteLine("------------------------");

            foreach (StoelModel stoel in stoelen)
            {
                string status = "vrij";
                if (IsBezet(vluchtId, stoel.ID, gekozenStoelen))
                {
                    status = "BEZET";
                }

                Console.WriteLine(stoel.ID + " | " + stoel.RijNummer + " | " + stoel.StoelNummer + " | " + status);
            }

            Console.Write("Kies het ID van een stoel: ");
            string invoer = Console.ReadLine() ?? "";

            // Zoek de stoel die de gebruiker heeft gekozen
            StoelModel? gekozenStoel = null;
            foreach (StoelModel stoel in stoelen)
            {
                if (stoel.ID.ToString() == invoer.Trim())
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


    private static bool CheckPassagier(PassagierModel passagier)
    {
        bool isValid = true;

        if (!passagierLogic.IsValidNaam(passagier.Naam))
        {
            Console.WriteLine("Voornaam bevat ongeldige tekens.");
            isValid = false;
        }

        if (!passagierLogic.IsValidAchternaam(passagier.Achternaam))
        {
            Console.WriteLine("Achternaam bevat ongeldige tekens.");
            isValid = false;
        }

        if (!passagierLogic.IsValidEmail(passagier.Email))
        {
            Console.WriteLine("Het e-mailadres is ongeldig.");
            isValid = false;
        }

        if (!passagierLogic.IsValidPaspoortNummer(passagier.PaspoortNummer))
        {
            Console.WriteLine("Het paspoortnummer moet 5-20 letters of cijfers bevatten.");
            isValid = false;
        }

        if (!passagierLogic.IsValidGeboorteDatum(passagier.GeboorteDatum))
        {
            Console.WriteLine("De geboortedatum moet een geldige datum in het verleden zijn (yyyy-MM-dd).");
            isValid = false;
        }

        if (!passagierLogic.IsValidTelefoonnummer(passagier.Telefoonnummer))
        {
            Console.WriteLine("Het telefoonnummer moet 7-15 cijfers bevatten.");
            isValid = false;
        }

        if (!passagierLogic.IsValidPaspoortVerloopDatum(passagier.PaspoortVerloopDatum))
        {
            Console.WriteLine("De paspoortverloopdatum moet een geldige datum in de toekomst zijn (yyyy-MM-dd).");
            isValid = false;
        }

        return isValid;
    }
}