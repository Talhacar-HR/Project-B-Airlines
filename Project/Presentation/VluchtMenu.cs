static class VluchtMenu
{
    static private VluchtLogic vluchtLogic = new VluchtLogic();

    public static void Start()
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            Console.WriteLine("");
            Console.WriteLine("Vluchten beheren");
            Console.WriteLine("1 - Overzicht");
            Console.WriteLine("2 - Toevoegen");
            Console.WriteLine("3 - Aanpassen");
            Console.WriteLine("0 - Terug");
            string input = Console.ReadLine() ?? "";
            Console.WriteLine("");

                if (input == "1")
                {
                    List<VluchtModel> flights = vluchtLogic.GetAll();
                    if (flights.Count == 0)
                    {
                        Console.WriteLine("Geen vluchten gevonden.");
                    }

                    foreach (VluchtModel flight in flights)
                    {
                        Console.WriteLine($"ID: {flight.ID}, Vliegtuig-ID: {flight.VliegtuigID}, Vertrekluchthaven-ID: {flight.VertrekLocatie}, Aankomstluchthaven-ID: {flight.AankomstLocatie}, Vertrektijd: {flight.VertreksTijd}, Vertraging: {flight.Vertraging}, Vluchtnummer: {flight.VluchtNummer}");
                    }
                }
                else if (input == "2")
                {
                    Console.Write("Vliegtuig-ID: ");
                    string vliegtuigIDInput = Console.ReadLine() ?? "";
                    Console.Write("Vertrekluchthaven-ID: ");
                    string vertrekLocatieInput = Console.ReadLine() ?? "";
                    Console.Write("Aankomstluchthaven-ID: ");
                    string aankomstLocatieInput = Console.ReadLine() ?? "";
                    Console.Write("Vertrektijd: ");
                    string vertreksTijd = Console.ReadLine() ?? "";
                    Console.Write("Vertraging: ");
                    string vertraging = Console.ReadLine() ?? "";
                    Console.Write("Vluchtnummer: ");
                    string vluchtNummerInput = Console.ReadLine() ?? "";

                    if (int.TryParse(vliegtuigIDInput, out int vliegtuigID) &&
                        int.TryParse(vertrekLocatieInput, out int vertrekLocatie) &&
                        int.TryParse(aankomstLocatieInput, out int aankomstLocatie) &&
                        int.TryParse(vluchtNummerInput, out int vluchtNummer))
                    {
                        VluchtModel flight = new(0, vliegtuigID, vertrekLocatie, aankomstLocatie, vertreksTijd, vertraging, vluchtNummer);
                        vluchtLogic.Add(flight);
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer.");
                    }
                }
                else if (input == "3")
                {
                    Console.Write("ID van de vlucht: ");
                    string idInput = Console.ReadLine() ?? "";
                    Console.Write("Nieuw vliegtuig-ID: ");
                    string vliegtuigIDInput = Console.ReadLine() ?? "";
                    Console.Write("Nieuw vertrekluchthaven-ID: ");
                    string vertrekLocatieInput = Console.ReadLine() ?? "";
                    Console.Write("Nieuw aankomstluchthaven-ID: ");
                    string aankomstLocatieInput = Console.ReadLine() ?? "";
                    Console.Write("Nieuwe vertrektijd: ");
                    string vertreksTijd = Console.ReadLine() ?? "";
                    Console.Write("Nieuwe vertraging: ");
                    string vertraging = Console.ReadLine() ?? "";
                    Console.Write("Nieuw vluchtnummer: ");
                    string vluchtNummerInput = Console.ReadLine() ?? "";

                    if (int.TryParse(idInput, out int id) &&
                        int.TryParse(vliegtuigIDInput, out int vliegtuigID) &&
                        int.TryParse(vertrekLocatieInput, out int vertrekLocatie) &&
                        int.TryParse(aankomstLocatieInput, out int aankomstLocatie) &&
                        int.TryParse(vluchtNummerInput, out int vluchtNummer))
                    {
                        VluchtModel flight = new(id, vliegtuigID, vertrekLocatie, aankomstLocatie, vertreksTijd, vertraging, vluchtNummer);
                        vluchtLogic.Update(flight);
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
