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
                        Console.WriteLine($"ID: {flight.ID}, Vliegtuig-ID: {flight.VliegtuigID}, Vertrekluchthaven-ID: {flight.VertrekLocatie}, Aankomstluchthaven-ID: {flight.AankomstLocatie}, Vertrek datum: {flight.VertrekDatum}, Vertrek tijd: {flight.VertrekTijd}, Vertraging (HH:mm): {flight.Vertraging}, Vluchtnummer: {flight.VluchtNummer}");
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
                    Console.Write("Vertrek datum (yyyy-MM-dd): ");
                    string vertrekDatum = Console.ReadLine() ?? "";
                    Console.Write("Vertrek tijd (HH:mm): ");
                    string vertrekTijd = Console.ReadLine() ?? "";
                    Console.Write("Vertraging (HH:mm): ");
                    string vertraging = Console.ReadLine() ?? "";
                    Console.Write("Vluchtnummer: ");
                    string vluchtNummerInput = Console.ReadLine() ?? "";

                    if (int.TryParse(vliegtuigIDInput, out int vliegtuigID) &&
                        int.TryParse(vertrekLocatieInput, out int vertrekLocatie) &&
                        int.TryParse(aankomstLocatieInput, out int aankomstLocatie) &&
                        int.TryParse(vluchtNummerInput, out int vluchtNummer))
                    {
                        VluchtModel flight = new(0, vliegtuigID, vertrekLocatie, aankomstLocatie, vertrekDatum, vertrekTijd, vertraging, vluchtNummer);
                        try
                        {
                            vluchtLogic.Add(flight);
                            Console.WriteLine("Vlucht toegevoegd.");
                        }
                        catch (ArgumentException exception)
                        {
                            Console.WriteLine(exception.Message);
                        }
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

                    if (!int.TryParse(idInput, out int id))
                    {
                        Console.WriteLine("Ongeldige invoer.");
                        continue;
                    }

                    VluchtModel? currentFlight = vluchtLogic.GetAll().Find(flight => flight.ID == id);
                    if (currentFlight is null)
                    {
                        Console.WriteLine($"Er bestaat geen vlucht met ID {id}.");
                        continue;
                    }

                    Console.Write($"Nieuw vliegtuig-ID (huidig: {currentFlight.VliegtuigID}): ");
                    string vliegtuigIDInput = Console.ReadLine() ?? "";
                    Console.Write($"Nieuw vertrekluchthaven-ID (huidig: {currentFlight.VertrekLocatie}): ");
                    string vertrekLocatieInput = Console.ReadLine() ?? "";
                    Console.Write($"Nieuw aankomstluchthaven-ID (huidig: {currentFlight.AankomstLocatie}): ");
                    string aankomstLocatieInput = Console.ReadLine() ?? "";
                    Console.Write($"Nieuwe vertrek datum (huidig: {currentFlight.VertrekDatum}, yyyy-MM-dd): ");
                    string vertrekDatum = Console.ReadLine() ?? "";
                    Console.Write($"Nieuwe vertrek tijd (huidig: {currentFlight.VertrekTijd}, HH:mm): ");
                    string vertrekTijd = Console.ReadLine() ?? "";
                    Console.Write($"Nieuwe vertraging (huidig: {currentFlight.Vertraging}, HH:mm): ");
                    string vertraging = Console.ReadLine() ?? "";
                    Console.Write($"Nieuw vluchtnummer (huidig: {currentFlight.VluchtNummer}): ");
                    string vluchtNummerInput = Console.ReadLine() ?? "";

                    if (int.TryParse(vliegtuigIDInput, out int vliegtuigID) &&
                        int.TryParse(vertrekLocatieInput, out int vertrekLocatie) &&
                        int.TryParse(aankomstLocatieInput, out int aankomstLocatie) &&
                        int.TryParse(vluchtNummerInput, out int vluchtNummer))
                    {
                        VluchtModel flight = new(id, vliegtuigID, vertrekLocatie, aankomstLocatie, vertrekDatum, vertrekTijd, vertraging, vluchtNummer);
                        try
                        {
                            vluchtLogic.Update(flight);
                            Console.WriteLine("Vlucht aangepast.");
                        }
                        catch (ArgumentException exception)
                        {
                            Console.WriteLine(exception.Message);
                        }
                        catch (KeyNotFoundException exception)
                        {
                            Console.WriteLine(exception.Message);
                        }
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
