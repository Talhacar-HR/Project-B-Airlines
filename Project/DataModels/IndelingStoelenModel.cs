public class IndelingStoelenModel
{
    public int VanRij { get; set; }

    public int TotRij { get; set; }
    public string Letters { get; set; }
    public int StoelKlasse { get; set; }

    public IndelingStoelenModel(int vanRij, int totRij, string letters, int stoelKlasse)
    {
        VanRij = vanRij;
        TotRij = totRij;
        Letters = letters;
        StoelKlasse = stoelKlasse;
    }
}

