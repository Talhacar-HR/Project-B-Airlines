static class StoelMenu
{
    static private StoelLogic stoelLogic = new StoelLogic();

    public static string Overzicht()
    {
        List<StoelModel> seats = stoelLogic.GetAll();
        if (seats.Count == 0)
            return "Geen stoelen gevonden.";

        string stoelen = "";
        foreach (StoelModel seat in seats)
            stoelen += $"ID: {seat.ID}, Vliegtuig-ID: {seat.VliegtuigID}, Stoelnummer: {seat.StoelNummer}, Rijnummer: {seat.RijNummer}, Stoelklasse: {seat.StoelKlasse}\n";
            return stoelen;
    }
}
