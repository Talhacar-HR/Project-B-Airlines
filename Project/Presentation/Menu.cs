static class Menu
{
    static public void Start()
    {
        PrintTitel();
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
                else if (currentAccount.IsAdmin == 0)
                    // Vluchten Bekijken
                    FlightOverview.Start();
                else
                    Console.WriteLine("Ongeldige invoer.");
            }
            else if (input == "3" && currentAccount != null && currentAccount.IsAdmin == 0)
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

    public static void PrintTitel()
    {
        Console.WriteLine(@$"
       ▀█▀▀▄        █    █                 ▀█                    ▄▀▄   ▀        ▀█   ▀                  
        █▄▄▀ ▄▀▀▀▄ ▀█▀▀ ▀█▀▀ ▄▀▀▀▄ ▀█▄▀▄  ▄▄█  ▀▀▀▄  █▀▄▀▄      █   █ ▀█  ▀█▄▀▄  █  ▀█  █▄▀▀▄ ▄▀▀▀▄ ▄▀▀▀
        █ ▀▄ █   █  █ ▄  █ ▄ █▀▀▀▀  █  ▀ █  █  ▄▀▀█  █ █ █      █▀▀▀█  █   █  ▀  █   █  █   █ █▀▀▀▀  ▀▀▄
        ▀  ▀  ▀▀▀    ▀    ▀   ▀▀▀  ▀▀▀    ▀▀ ▀  ▀▀ ▀ ▀ ▀ ▀      ▀   ▀ ▀▀▀ ▀▀▀   ▀▀▀ ▀▀▀ ▀   ▀  ▀▀▀  ▀▀▀");
    }

    public static string MenuOptions(AccountModel? currentAccount)
    {
        Console.WriteLine("");
        Console.WriteLine("Wat wilt u doen?");
        if (currentAccount != null)
        {
            Console.WriteLine("1 - Uitloggen");
            if (currentAccount.IsAdmin == 0)
            {
                Console.WriteLine("2 - Vluchten checken");
                Console.WriteLine("3 - Mijn reserveringen");
            }
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