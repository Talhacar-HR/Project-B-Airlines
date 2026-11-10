using System.Globalization;

static class PrijsOverzicht
{
    static private PrijsLogic prijsLogic = new PrijsLogic();

    // Laat per passagier de prijsopbouw zien en onderaan de totaalprijs
    public static void Start(int vluchtId, List<ReserveringsStoelModel> stoelen)
    {
        List<PrijsRegelModel> regels = prijsLogic.GetPrijsRegels(vluchtId, stoelen);

        Console.WriteLine("");
        Console.WriteLine("Prijsoverzicht");
        Console.WriteLine("--------------------------------------------");

        int nummer = 1;
        foreach (PrijsRegelModel regel in regels)
        {
            ToonRegel(nummer, regel);
            nummer++;
        }

        Console.WriteLine("--------------------------------------------");
        Console.WriteLine("Totaalprijs: " + Bedrag(prijsLogic.BerekenTotaalprijs(vluchtId, stoelen)));
        Console.WriteLine("");
    }

    private static void ToonRegel(int nummer, PrijsRegelModel regel)
    {
        string bagage = regel.ExtraBagage ? "ja" : "nee";

        Console.WriteLine($"Passagier {nummer}");
        Console.WriteLine($"  Vlucht {regel.VluchtNummer}: {Bedrag(regel.VluchtPrijs)}");
        Console.WriteLine($"  Stoel {regel.StoelNummer} (toeslag): {Bedrag(regel.Toeslag)}");
        Console.WriteLine($"  Extra bagage: {bagage}");
        Console.WriteLine($"  Prijs: {Bedrag(regel.Prijs)}");
    }

    // Geeft een bedrag terug als "€ 0,00"
    private static string Bedrag(double bedrag)
    {
        return "€ " + bedrag.ToString("N2", new CultureInfo("nl-NL"));
    }
}
