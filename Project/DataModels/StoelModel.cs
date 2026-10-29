public class StoelModel
{
    public Int64 ID { get; set; }

    public Int64 VliegtuigID { get; set; }

    public string StoelNummer { get; set; }

    public string RijNummer { get; set; }

    public int StoelKlasse { get; set; }

    public StoelModel()
    {
    }

    public StoelModel(Int64 id, Int64 vliegtuigID, string stoelNummer, string rijNummer, int stoelKlasse)
    {
        ID = id;
        VliegtuigID = vliegtuigID;
        StoelNummer = stoelNummer;
        RijNummer = rijNummer;
        StoelKlasse = stoelKlasse;
    }
}
