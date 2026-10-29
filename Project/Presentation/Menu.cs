static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            AccountModel? currentAccount = AccountsLogic.CurrentAccount;
            {
                Console.WriteLine("Wat wilt u doen?");
                Console.WriteLine("1 - Inloggen");
                // Console.WriteLine("2 - dummy holder");
                if (currentAccount != null)
                {
                    if (currentAccount.IsAdmin != 0)
                    {
                        Console.WriteLine("8 - Gegevens beheren");
                    }
                    Console.WriteLine("9 - ");
                }
                Console.WriteLine("0 - Programma afsluiten");
                string input = Console.ReadLine() ?? "";
                Console.WriteLine("");

                if (input == "1")
                {
                    UserLogin.Start();
                }
                else if (input == "2")
                {
                    Console.WriteLine("Deze functie is nog niet beschikbaar.");
                }
                else if (input == "8" && currentAccount != null)
                {
                    string userName = currentAccount.FullName;
                    string userEmail = currentAccount.EmailAddress;
                    Console.WriteLine($"Gebruikersnaam: {userName}");
                    Console.WriteLine($"Gebruikers Email: {userEmail}");
                    Start();
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
    }
}