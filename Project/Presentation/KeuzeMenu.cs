static class KeuzeMenu
{
    static private VliegtuigLogic vliegtuigLogic = new VliegtuigLogic();
    static private VliegveldLogic vliegveldLogic = new VliegveldLogic();
    static private VluchtLogic vluchtLogic = new VluchtLogic();

    public static VliegtuigModel? KiesVliegtuig(int? geselecteerdVliegtuigId = null)
    {
        List<VliegtuigModel> vliegtuigen = vliegtuigLogic.GetAll();
        int selectedIndex = geselecteerdVliegtuigId.HasValue
            ? vliegtuigen.FindIndex(vliegtuig => vliegtuig.ID == geselecteerdVliegtuigId.Value)
            : 0;

        return ToonKeuzemenu(
            "Kies een vliegtuigmodel:",
            vliegtuigen,
            vliegtuig => $"{vliegtuig.Model} ({vliegtuig.Merk})",
            selectedIndex);
    }

    public static VliegveldModel? KiesVliegveld(
        string titel,
        int? geselecteerdVliegveldId = null,
        int? uitgeslotenVliegveldId = null)
    {
        List<VliegveldModel> vliegvelden = vliegveldLogic.GetAll();
        if (uitgeslotenVliegveldId.HasValue)
            vliegvelden.RemoveAll(vliegveld => vliegveld.ID == uitgeslotenVliegveldId.Value);

        int selectedIndex = geselecteerdVliegveldId.HasValue
            ? vliegvelden.FindIndex(vliegveld => vliegveld.ID == geselecteerdVliegveldId.Value)
            : 0;

        return ToonKeuzemenu(
            titel,
            vliegvelden,
            vliegveld => $"{vliegveld.Naam} - {vliegveld.Stad}, {vliegveld.Land}",
            selectedIndex);
    }

    public static VluchtModel? KiesVlucht()
    {
        List<VluchtModel> flights = vluchtLogic.GetAll();
        List<VliegtuigModel> vliegtuigen = vliegtuigLogic.GetAll();
        List<VliegveldModel> vliegvelden = vliegveldLogic.GetAll();
        Dictionary<int, string> flightLabels = new();

        foreach (VluchtModel flight in flights)
        {
            VliegtuigModel? vliegtuig = vliegtuigen.Find(item => item.ID == flight.VliegtuigID);
            VliegveldModel? vertrekVliegveld = vliegvelden.Find(item => item.ID == flight.VertrekLocatie);
            VliegveldModel? aankomstVliegveld = vliegvelden.Find(item => item.ID == flight.AankomstLocatie);
            flightLabels.Add(
                flight.ID,
                $"{flight.VluchtNummer} | {vliegtuig?.Model ?? "Onbekend"} | " +
                $"{vertrekVliegveld?.Naam ?? "Onbekend"} -> {aankomstVliegveld?.Naam ?? "Onbekend"} | " +
                $"{flight.VertrekDatum} {flight.VertrekTijd}");
        }

        return ToonKeuzemenu(
            "Kies een vlucht:",
            flights,
            flight => flightLabels[flight.ID],
            0);
    }

    private static T? ToonKeuzemenu<T>(
        string titel,
        List<T> opties,
        Func<T, string> label,
        int selectedIndex) where T : class
    {
        if (opties.Count == 0)
            return null;

        if (selectedIndex < 0 || selectedIndex >= opties.Count)
            selectedIndex = 0;

        while (true)
        {
            Menu.PrintTitel();
            Console.WriteLine($"{titel} (pijltjes omhoog/omlaag, Enter bevestigt, Escape annuleert):");
            for (int i = 0; i < opties.Count; i++)
            {
                string marker = i == selectedIndex ? "> " : "  ";
                Console.WriteLine($"{marker}{label(opties[i])}");
            }

            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(opties.Count - 1, selectedIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    return opties[selectedIndex];
                case ConsoleKey.Escape:
                    return null;
            }
        }
    }
}
