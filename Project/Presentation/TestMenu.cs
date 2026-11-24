using System.Collections;

static class TestMenu
{
    static public void Start()
    {
        AccountModel? currentAccount = AccountsLogic.CurrentAccount;
        List<string> menuChoices = CreateMenuChoices(currentAccount);
        int selectedIndex = 0;

        bool menuRunning = true;
        while (menuRunning)
        {
            // Console.Clear();
            Console.WriteLine("Which menu would you like to select?");

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

                case ConsoleKey.Enter or ConsoleKey.Escape:
                    menuRunning = false;
                    break;
            }
        }

        Console.WriteLine($"You selected {menuChoices[selectedIndex]}!");
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
}