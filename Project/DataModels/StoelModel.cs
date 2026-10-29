public class StoelModel
{
    public int ID { get; set; }

    public int VliegtuigID { get; set; }

    public string StoelNummer { get; set; }

    public string RijNummer { get; set; }

    public int StoelKlasse { get; set; }

    public StoelModel()
    {
    }

    public StoelModel(int id, int vliegtuigID, string stoelNummer, string rijNummer, int stoelKlasse)
    {
        ID = id;
        VliegtuigID = vliegtuigID;
        StoelNummer = stoelNummer;
        RijNummer = rijNummer;
        StoelKlasse = stoelKlasse;
    }
}
