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
            string naam = "";
            string achternaam = "";
            string email = "";
            string paspoortNummer = "";
            string geboorteDatum = "";
            string telefoonnummer = "";
            string paspoortVerloopDatum = "";

            string feedback = "";
            string warning = "";
            int currentChoice = 0;
            bool choicesConfirmed = false;
            while (choicesConfirmed == false)
            {
                Menu.PrintTitel();
                if (feedback != "")
                    Console.WriteLine(feedback);
                if (warning != "")
                    Console.WriteLine(warning);
                    warning = "";

                switch (currentChoice)
                {
                    case 0:
                        Console.Write("Voornaam: ");
                        naam = Console.ReadLine() ?? "";

                        if (passagierLogic.IsValidNaam(naam))
                        {
                            currentChoice += 1;
                            feedback += $"Voornaam: {naam}";
                        }
                        else
                            warning = "Voornaam bevat ongeldige tekens.";
                        break;
                    case 1:
                        Console.Write("Achternaam: ");
                        achternaam = Console.ReadLine() ?? "";

                        if (passagierLogic.IsValidAchternaam(achternaam))
                        {
                            currentChoice += 1;
                            feedback += $"\nAchternaam: {achternaam}";
                        }
                        else
                            warning = "Achternaam bevat ongeldige tekens.";
                        break;
                    case 2:
                        Console.Write("E-mailadres: ");
                        email = Console.ReadLine() ?? "";

                        if (passagierLogic.IsValidEmail(email))
                        {
                            currentChoice += 1;
                            feedback += $"\nE-mailadres: {email}";
                        }
                        else
                            warning = "Het e-mailadres is ongeldig.";
                        break;
                    case 3:
                        Console.Write("Paspoortnummer: ");
                        paspoortNummer = Console.ReadLine() ?? "";

                        if (passagierLogic.IsValidPaspoortNummer(paspoortNummer))
                        {
                            currentChoice += 1;
                            feedback += $"\nPaspoortnummer: {paspoortNummer}";
                        }
                        else
                            warning = "Het paspoortnummer moet 5-20 letters of cijfers bevatten.";
                        break;
                    case 4:
                        Console.Write("Geboortedatum (dd-MM-yyyy): ");
                        geboorteDatum = Console.ReadLine() ?? "";

                        if (passagierLogic.IsValidGeboorteDatum(geboorteDatum))
                        {
                            currentChoice += 1;
                            feedback += $"\nGeboortedatum: {geboorteDatum}";
                        }
                        else
                            warning = "De geboortedatum moet een geldige datum in het verleden zijn (dd-MM-yyyy).";
                        break;
                    case 5:
                        Console.Write("Telefoonnummer: ");
                        telefoonnummer = Console.ReadLine() ?? "";

                        if (passagierLogic.IsValidTelefoonnummer(telefoonnummer))
                        {
                            currentChoice += 1;
                            feedback += $"\nTelefoonnummer: {telefoonnummer}";
                        }
                        else
                            warning = "Het telefoonnummer moet 7-15 cijfers bevatten.";
                        break;
                    case 6:
                        Console.Write("Paspoortverloopdatum (dd-MM-yyyy): ");
                        paspoortVerloopDatum = Console.ReadLine() ?? "";

                        if (passagierLogic.IsValidPaspoortVerloopDatum(paspoortVerloopDatum))
                        {
                            currentChoice += 1;
                            feedback += $"\nPaspoortverloopdatum: {paspoortVerloopDatum}";
                        }
                        else
                            warning = "De paspoortverloopdatum moet een geldige datum in de toekomst zijn (dd-MM-yyyy).";
                        break;
                    case 7:
                        choicesConfirmed = true;
                        break;
                }
            }

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
            
            Menu.PrintTitel();
            Console.WriteLine(feedback);
            passagierLogic.Add(passagier);

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
                meerPassagiers = false;
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
}