static class VluchtBoeken
{
    static private PassagierLogic passagierLogic = new PassagierLogic();

    public static void Start(int vluchtId)
    {
        AccountModel? currentAccount = AccountsLogic.CurrentAccount;

        Console.WriteLine($"U heeft vlucht nummer {vluchtId} geselecteerd.");

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

            Console.Write("Wilt u nog een passagier toevoegen? (J/N): ");
            string keuze = Console.ReadLine() ?? "";
            if (keuze.Trim().Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                meerPassagiers = false;
            }
        }
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
