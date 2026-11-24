static class Menu
{
    static public void Start()
    {
        int selectedIndex = 0;
        bool menuRunning = true;
        string feedback = "";
        while (menuRunning)
        {
            AccountModel? currentAccount = AccountsLogic.CurrentAccount;
            List<string> menuChoices = CreateMenuChoices(currentAccount);

            bool menuChoosing = true;
            while (menuChoosing == true)
            {
                PrintTitel();

                if (feedback != "")
                    Console.WriteLine(feedback);
                    Console.WriteLine("");
                Console.WriteLine("Wat wilt u doen? Klik enter om te bevestigen.");

                for (int i = 0; i < menuChoices.Count; i++)
                {
                    string marker = i == selectedIndex ? "> " : "  ";
                    Console.WriteLine($"{marker}{menuChoices[i]}");
                }

                ConsoleKey key = Console.ReadKey(intercept: true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = Math.Max(0, selectedIndex - 1);
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = Math.Min(menuChoices.Count - 1, selectedIndex + 1);
                        break;

                    case ConsoleKey.Enter:
                        menuChoosing = false;
                        break;
                }
            }
            char input = menuChoices[selectedIndex][0];

            if (input == '0')
            {
                Console.WriteLine("Programma wordt afgesloten...");
                menuRunning = false;
            }
            else
            {
                feedback = MenuChoice(input, currentAccount);
            }
        }
    }

    static public List<string> CreateMenuChoices(AccountModel? currentAccount)
    {
        List<string> menuOptions = new();
        if (currentAccount == null)
        {
            menuOptions.Add("1 - Inloggen");
            menuOptions.Add("2 - Registreren");
        }
        else
        {
            menuOptions.Add("1 - Uitloggen");
            if (currentAccount.IsAdmin == 0)
            {
                menuOptions.Add("2 - Vluchten checken");
                menuOptions.Add("3 - Mijn reserveringen");
            }
            menuOptions.Add("8 - Check gebruiker");
            if (currentAccount.IsAdmin == 1)
            {
                menuOptions.Add("9 - Gegevens beheren");
            }
        }
        menuOptions.Add("0 - Programma afsluiten");
        return menuOptions;
    }

    public static string MenuChoice(char input, AccountModel? currentAccount)
    {
        if (input == '1')
        {
            if (currentAccount == null)
                // Inloggen
                return UserLogin.Start();
            else
                // Uitloggen
                return "Deze functie is nog niet beschikbaar.";
        }
        else if (input == '2')
        {
            if (currentAccount == null)
                // Registeren
                return UserRegistration.Start();
            else if (currentAccount.IsAdmin == 0)
                // Vluchten Bekijken
                return FlightOverview.Start();
            else
                return "Ongeldige invoer.";
        }
        else if (input == '3' && currentAccount != null && currentAccount.IsAdmin == 0)
        {
            return MijnReserveringen.Start();
        }
        else if (input == '8' && currentAccount != null)
        {
            string userInfo =
                $"Gebruikersnaam: {currentAccount.FullName}\n" +
                $"Gebruikers Email: {currentAccount.EmailAddress}";

            if (currentAccount.IsAdmin == 1)
                userInfo += "\nGebruikers Admin: True";

            return userInfo;
        }
        else if (input == '9' && currentAccount != null && currentAccount.IsAdmin == 1)
        {
            DataMenu.Start();
        }
        else
        {
            return "Ongeldige invoer.";
        }
        return "";
    }

    public static void PrintTitel()
    {
        try
        {
            Console.Clear();
        }
        catch (IOException) {}
        Console.WriteLine(@$"
        ▀█▀▀▄        █    █                 ▀█                    ▄▀▄   ▀        ▀█   ▀                  
         █▄▄▀ ▄▀▀▀▄ ▀█▀▀ ▀█▀▀ ▄▀▀▀▄ ▀█▄▀▄  ▄▄█  ▀▀▀▄  █▀▄▀▄      █   █ ▀█  ▀█▄▀▄  █  ▀█  █▄▀▀▄ ▄▀▀▀▄ ▄▀▀▀
         █ ▀▄ █   █  █ ▄  █ ▄ █▀▀▀▀  █  ▀ █  █  ▄▀▀█  █ █ █      █▀▀▀█  █   █  ▀  █   █  █   █ █▀▀▀▀  ▀▀▄
         ▀  ▀  ▀▀▀    ▀    ▀   ▀▀▀  ▀▀▀    ▀▀ ▀  ▀▀ ▀ ▀ ▀ ▀      ▀   ▀ ▀▀▀ ▀▀▀   ▀▀▀ ▀▀▀ ▀   ▀  ▀▀▀  ▀▀▀
        ");
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