// 1 regel in het prijsoverzicht, per passagier
public class PrijsRegelModel
{
    public int VluchtNummer { get; set; }

    public double VluchtPrijs { get; set; }

    public string StoelNummer { get; set; }

    public double Toeslag { get; set; }

    public bool ExtraBagage { get; set; }

    public double Prijs { get; set; }

    public PrijsRegelModel()
    {
    }

    public PrijsRegelModel(int vluchtNummer, double vluchtPrijs, string stoelNummer, double toeslag, bool extraBagage, double prijs)
    {
        VluchtNummer = vluchtNummer;
        VluchtPrijs = vluchtPrijs;
        StoelNummer = stoelNummer;
        Toeslag = toeslag;
        ExtraBagage = extraBagage;
        Prijs = prijs;
    }
}
