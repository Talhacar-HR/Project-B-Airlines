static class DataMenu
{
    public static void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            Console.WriteLine("");
            string menuInput = MenuOptions();
            Console.WriteLine("");

            if (menuInput == "1")
            {
                VluchtMenu.Start();
            }
            else if (menuInput == "2")
            {
                VliegtuigMenu.Start();
            }
            else if (menuInput == "3")
            {
                VliegveldMenu.Start();
            }
            else if (menuInput == "4")
            {
                StoelMenu.Start();
            }
            else if (menuInput == "0")
            {
                menuRunning = false;
            }
            else
            {
                Console.WriteLine("Ongeldige keuze.");
            }
        }
    }

    public static string MenuOptions()
    {
        Console.WriteLine("Admin beheerdersmenu");
        Console.WriteLine("1 - Vluchten beheren");
        Console.WriteLine("2 - Vliegtuigen beheren");
        Console.WriteLine("3 - Vliegvelden beheren");
        Console.WriteLine("4 - Stoelen beheren");
        Console.WriteLine("0 - Terug");
        Console.Write("Wat wilt u doen? ");
        return Console.ReadLine() ?? "";
    }
}
