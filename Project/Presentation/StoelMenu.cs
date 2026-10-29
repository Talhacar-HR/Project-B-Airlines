static class StoelMenu
{
    static private StoelLogic stoelLogic = new StoelLogic();

    public static void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            Console.WriteLine("");
            Console.WriteLine("Stoelen beheren");
            Console.WriteLine("1 - Overzicht");
            Console.WriteLine("2 - Toevoegen");
            Console.WriteLine("3 - Aanpassen");
            Console.WriteLine("0 - Terug");
            string input = Console.ReadLine() ?? "";
            Console.WriteLine("");

                if (input == "1")
                {
                    List<StoelModel> seats = stoelLogic.GetAll();
                    if (seats.Count == 0)
                    {
                        Console.WriteLine("Geen stoelen gevonden.");
                    }

                    foreach (StoelModel seat in seats)
                    {
                        Console.WriteLine($"ID: {seat.ID}, Vliegtuig-ID: {seat.VliegtuigID}, Stoelnummer: {seat.StoelNummer}, Rijnummer: {seat.RijNummer}, Stoelklasse: {seat.StoelKlasse}");
                    }
                }
                else if (input == "2")
                {
                    Console.Write("Vliegtuig-ID: ");
                    string vliegtuigIDInput = Console.ReadLine() ?? "";
                    Console.Write("Stoelnummer: ");
                    string stoelNummer = Console.ReadLine() ?? "";
                    Console.Write("Rijnummer: ");
                    string rijNummer = Console.ReadLine() ?? "";
                    Console.Write("Stoelklasse: ");
                    string stoelKlasseInput = Console.ReadLine() ?? "";

                    if (long.TryParse(vliegtuigIDInput, out long vliegtuigID) &&
                        int.TryParse(stoelKlasseInput, out int stoelKlasse))
                    {
                        StoelModel seat = new(0, vliegtuigID, stoelNummer, rijNummer, stoelKlasse);
                        stoelLogic.Add(seat);
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer.");
                    }
                }
                else if (input == "3")
                {
                    Console.Write("ID van de stoel: ");
                    string idInput = Console.ReadLine() ?? "";
                    Console.Write("Nieuw vliegtuig-ID: ");
                    string vliegtuigIDInput = Console.ReadLine() ?? "";
                    Console.Write("Nieuw stoelnummer: ");
                    string stoelNummer = Console.ReadLine() ?? "";
                    Console.Write("Nieuw rijnummer: ");
                    string rijNummer = Console.ReadLine() ?? "";
                    Console.Write("Nieuwe stoelklasse: ");
                    string stoelKlasseInput = Console.ReadLine() ?? "";

                    if (long.TryParse(idInput, out long id) &&
                        long.TryParse(vliegtuigIDInput, out long vliegtuigID) &&
                        int.TryParse(stoelKlasseInput, out int stoelKlasse))
                    {
                        StoelModel seat = new(id, vliegtuigID, stoelNummer, rijNummer, stoelKlasse);
                        stoelLogic.Update(seat);
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
