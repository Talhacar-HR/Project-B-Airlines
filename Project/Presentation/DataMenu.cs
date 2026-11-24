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
        string[] menuOptions = { "1 - Vluchten beheren", "2 - Vliegtuigen beheren", "3 - Vliegvelden beheren", "0 - Terug" };
        int selectedIndex = 0;

        while (true)
        {
            Menu.PrintTitel();

            Console.WriteLine("Admin beheerdersmenu");
            for (int i = 0; i < menuOptions.Length; i++)
            {
                string marker = i == selectedIndex ? "> " : "  ";
                Console.WriteLine($"{marker}{menuOptions[i]}");
            }

            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(menuOptions.Length - 1, selectedIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    return menuOptions[selectedIndex][0].ToString();
                case ConsoleKey.Escape:
                    return "0";
            }
        }
    }
}
