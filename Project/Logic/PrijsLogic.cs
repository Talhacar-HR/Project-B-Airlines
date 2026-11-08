public class PrijsLogic
{
    // Vaste prijs voor extra bagage per passagier
    public const double ExtraBagagePrijs = 50;

    private ReserveringAccess _reserveringAccess = new();
    private VluchtAccess _vluchtAccess = new();
    private StoelAccess _stoelAccess = new();
    private StoelKlasseAccess _stoelKlasseAccess = new();

    public PrijsLogic()
    {
    }

    // Prijs voor 1 passagier: vluchtprijs + stoeltoeslag + eventueel extra bagage
    public double BerekenPrijsPerPassagier(double vluchtPrijs, double stoelToeslag, bool extraBagage)
    {
        double prijs = vluchtPrijs + stoelToeslag;

        // Bagagekosten tellen alleen mee als extra bagage gekozen is
        if (extraBagage)
        {
            prijs += ExtraBagagePrijs;
        }

        return prijs;
    }

    // Prijs voor 1 passagier uit een reservering (zoekt zelf de toeslag van de stoel op)
    public double BerekenPrijsPerPassagier(ReserveringsStoelModel reserveringsStoel, double vluchtPrijs)
    {
        double toeslag = GetStoelToeslag(reserveringsStoel.StoelID);
        bool extraBagage = reserveringsStoel.ExtraBagage == 1;

        return BerekenPrijsPerPassagier(vluchtPrijs, toeslag, extraBagage);
    }

    // Totaalprijs van een reservering: de prijzen van alle passagiers bij elkaar opgeteld.
    // Een onbekende reservering of een reservering zonder passagiers geeft 0 terug (geen crash).
    public double BerekenTotaalprijs(int reserveringId)
    {
        ReserveringModel? reservering = _reserveringAccess.GetById(reserveringId);
        if (reservering == null)
        {
            return 0;
        }

        VluchtModel? vlucht = _vluchtAccess.GetById(reservering.VluchtID);
        if (vlucht == null)
        {
            return 0;
        }

        List<ReserveringsStoelModel> passagiers = _reserveringAccess.GetStoelenByReserveringId(reserveringId);

        double totaal = 0;
        foreach (ReserveringsStoelModel passagier in passagiers)
        {
            totaal += BerekenPrijsPerPassagier(passagier, vlucht.Prijs);
        }

        // Afronden op centen, zodat er geen getallen als 859.9800000001 uitkomen
        return Math.Round(totaal, 2);
    }

    // Zoekt de toeslag van de stoelklasse van een stoel op.
    // Bestaat de stoel of stoelklasse niet, dan is de toeslag 0.
    private double GetStoelToeslag(int stoelId)
    {
        StoelModel? stoel = _stoelAccess.GetById(stoelId);
        if (stoel == null)
        {
            return 0;
        }

        StoelKlasseModel? stoelKlasse = _stoelKlasseAccess.GetById(stoel.StoelKlasse);
        if (stoelKlasse == null)
        {
            return 0;
        }

        return stoelKlasse.Toeslag;
    }
}
