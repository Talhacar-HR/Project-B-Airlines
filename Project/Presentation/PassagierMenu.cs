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
}
