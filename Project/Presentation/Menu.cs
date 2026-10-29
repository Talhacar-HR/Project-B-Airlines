static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        AccountModel? currentAccount = AccountsLogic.CurrentAccount;
        {
            Console.WriteLine("Welcome to Project B Airlines.");
            Console.WriteLine("");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Enter 1 to login");
            Console.WriteLine("Enter 2 to do something else in the future");
            Console.WriteLine("Enter 0 to exit program");
            string input = Console.ReadLine() ?? "";
            Console.WriteLine("");

            if (input == "1")
            {
                UserLogin.Start();
            }
            else if (input == "2")
            {
                Console.WriteLine("This feature is not yet implemented");
            }
            else if (input == "0")
            {
                Console.WriteLine("Shutting down...");
            }
            else
            {
                Console.WriteLine("Invalid input");
                Start();
            }
        }
    }
}