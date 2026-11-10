static class Menu
{
    static public void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            AccountModel? currentAccount = AccountsLogic.CurrentAccount;
            string input = MenuOptions(currentAccount);
            Console.WriteLine("");

            if (input == "1")
            {
                if (currentAccount == null)
                    // Inloggen
                    UserLogin.Start();
                else
                    // Uitloggen
                    Console.WriteLine("Deze functie is nog niet beschikbaar.");
            }
            else if (input == "2")
            {
                if (currentAccount == null)
                    // Registeren
                    UserRegistration.Start();
                else
                    // Vluchten Bekijken
                    FlightOverview.Start();
            }
            else if (input == "3" && currentAccount != null)
            {
                MijnReserveringen.Start();
            }
            else if (input == "8" && currentAccount != null)
            {
                Console.WriteLine($"Gebruikersnaam: {currentAccount.FullName}");
                Console.WriteLine($"Gebruikers Email: {currentAccount.EmailAddress}");
            }
            else if (input == "9" && currentAccount != null && currentAccount.IsAdmin != 0)
            {
                DataMenu.Start();
            }
            else if (input == "0")
            {
                Console.WriteLine("Programma wordt afgesloten...");
                menuRunning = false;
            }
            else
            {
                Console.WriteLine("Ongeldige invoer.");
            }
        }
    }

    public static string MenuOptions(AccountModel? currentAccount)
    {
        Console.WriteLine("");
        Console.WriteLine("Wat wilt u doen?");
        if (currentAccount != null)
        {
            Console.WriteLine("1 - Uitloggen");
            Console.WriteLine("2 - Vluchten checken");
            Console.WriteLine("3 - Mijn reserveringen");
            Console.WriteLine("8 - Check gebruiker");
            if (currentAccount.IsAdmin != 0)
            {
                Console.WriteLine("9 - Gegevens beheren");
            }
        }
        else
        {
            Console.WriteLine("1 - Inloggen");
            Console.WriteLine("2 - Registreren");
        }
        Console.WriteLine("0 - Programma afsluiten");
        return Console.ReadLine() ?? "";
    }
}