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