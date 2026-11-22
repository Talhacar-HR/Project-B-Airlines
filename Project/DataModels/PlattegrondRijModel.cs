public class PlattegrondRijModel
{
    public int RijNummer { get; set; }

    public string Letters { get; set; }

    public List<PlattegrondStoelModel> Stoelen { get; set; }

    public PlattegrondRijModel(int rijNummer, string letters)
    {
        RijNummer = rijNummer;
        Letters = letters;
        Stoelen = new List<PlattegrondStoelModel>();
    }
}
