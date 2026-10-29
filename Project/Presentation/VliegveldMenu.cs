static class VliegveldMenu
{
    static private VliegveldLogic vliegveldLogic = new VliegveldLogic();

    public static void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            Console.WriteLine("");
            Console.WriteLine("Vliegvelden beheren");
            Console.WriteLine("1 - Overzicht");
            Console.WriteLine("2 - Toevoegen");
            Console.WriteLine("3 - Aanpassen");
            Console.WriteLine("0 - Terug");
            string input = Console.ReadLine() ?? "";
            Console.WriteLine("");
            
                if (input == "1")
                {
                    List<VliegveldModel> airports = vliegveldLogic.GetAll();
                    if (airports.Count == 0)
                    {
                        Console.WriteLine("Geen vliegvelden gevonden.");
                    }

                    foreach (VliegveldModel airport in airports)
                    {
                        Console.WriteLine($"ID: {airport.ID}, Naam: {airport.Naam}, Stad: {airport.Stad}, Land: {airport.Land}, Afkorting: {airport.Afkorting}");
                    }
                }
                else if (input == "2")
                {
                    Console.Write("Naam: ");
                    string naam = Console.ReadLine() ?? "";
                    Console.Write("Stad: ");
                    string stad = Console.ReadLine() ?? "";
                    Console.Write("Land: ");
                    string land = Console.ReadLine() ?? "";
                    Console.Write("Afkorting: ");
                    string afkorting = Console.ReadLine() ?? "";

                    VliegveldModel airport = new(
                        0,
                        naam,
                        stad,
                        land,
                        afkorting);

                    vliegveldLogic.Add(airport);
                }
                else if (input == "3")
                {
                    Console.Write("ID van het vliegveld: ");
                    string idInput = Console.ReadLine() ?? "";
                    Console.Write("Nieuwe naam: ");
                    string naam = Console.ReadLine() ?? "";
                    Console.Write("Nieuwe stad: ");
                    string stad = Console.ReadLine() ?? "";
                    Console.Write("Nieuw land: ");
                    string land = Console.ReadLine() ?? "";
                    Console.Write("Nieuwe afkorting: ");
                    string afkorting = Console.ReadLine() ?? "";

                    if (long.TryParse(idInput, out long id))
                    {
                        VliegveldModel airport = new(id, naam, stad, land, afkorting);
                        vliegveldLogic.Update(airport);
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer.");
                    }
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
