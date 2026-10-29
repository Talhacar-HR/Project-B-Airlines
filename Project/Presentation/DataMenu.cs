static class DataMenu
{
    public static void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            Console.WriteLine("");
            Console.WriteLine("Admin beheerdersmenu");
            Console.WriteLine("1 - Vluchten beheren");
            Console.WriteLine("2 - Vliegtuigen beheren");
            Console.WriteLine("3 - Vliegvelden beheren");
            Console.WriteLine("4 - Stoelen beheren");
            Console.WriteLine("0 - Terug");
            string input = Console.ReadLine() ?? "";
            Console.WriteLine("");

            if (input == "1")
            {
                VluchtMenu.Start();
            }
            else if (input == "2")
            {
                VliegtuigMenu.Start();
            }
            else if (input == "3")
            {
                VliegveldMenu.Start();
            }
            else if (input == "4")
            {
                StoelMenu.Start();
            }
            else if (input == "0")
            {
                menuRunning = false;
            }
            else
            {
                Console.WriteLine("Ongeldige keuze.");
            }
        }
    }
}
