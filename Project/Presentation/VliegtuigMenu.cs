static class VliegtuigMenu
{
    static private VliegtuigLogic vliegtuigLogic = new VliegtuigLogic();

    public static void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            Console.WriteLine("");
            Console.WriteLine("Vliegtuigen beheren");
            Console.WriteLine("1 - Overzicht");
            Console.WriteLine("2 - Toevoegen");
            Console.WriteLine("3 - Aanpassen");
            Console.WriteLine("0 - Terug");
            string input = Console.ReadLine() ?? "";
            Console.WriteLine("");

            if (input == "1")
            {
                List<VliegtuigModel> planes = vliegtuigLogic.GetAll();
                if (planes.Count == 0)
                {
                    Console.WriteLine("Geen vliegtuigen gevonden.");
                }

                foreach (VliegtuigModel plane in planes)
                {
                    Console.WriteLine($"ID: {plane.ID}, Bagageruimte: {plane.BaggageRuimte}, Model: {plane.Model}, Merk: {plane.Merk}");
                }
            }
            else if (input == "2")
            {
                Console.Write("Bagageruimte: ");
                string baggageRuimteInput = Console.ReadLine() ?? "";
                Console.Write("Model: ");
                string model = Console.ReadLine() ?? "";
                Console.Write("Merk: ");
                string merk = Console.ReadLine() ?? "";

                if (int.TryParse(baggageRuimteInput, out int baggageRuimte))
                {
                    VliegtuigModel plane = new(0, baggageRuimte, model, merk);
                    vliegtuigLogic.Add(plane);
                }
                else
                {
                    Console.WriteLine("Ongeldige invoer.");
                }
            }
            else if (input == "3")
            {
                Console.Write("ID van het vliegtuig: ");
                string idInput = Console.ReadLine() ?? "";
                Console.Write("Nieuwe bagageruimte: ");
                string baggageRuimteInput = Console.ReadLine() ?? "";
                Console.Write("Nieuw model: ");
                string model = Console.ReadLine() ?? "";
                Console.Write("Nieuw merk: ");
                string merk = Console.ReadLine() ?? "";

                if (long.TryParse(idInput, out long id) &&
                    int.TryParse(baggageRuimteInput, out int baggageRuimte))
                {
                    VliegtuigModel plane = new(id, baggageRuimte, model, merk);
                    vliegtuigLogic.Update(plane);
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
