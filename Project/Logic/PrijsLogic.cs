public class PrijsLogic
{
    // Vaste prijs voor extra bagage per passagier
    public const double ExtraBagagePrijs = 50;

    private ReserveringLogic _reserveringLogic = new();
    private VluchtLogic _vluchtLogic = new();
    private StoelLogic _stoelLogic = new();
    private StoelKlasseLogic _stoelKlasseLogic = new();

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
        ReserveringModel? reservering = _reserveringLogic.GetById(reserveringId);
        if (reservering == null)
        {
            return 0;
        }

        VluchtModel? vlucht = _vluchtLogic.GetById(reservering.VluchtID);
        if (vlucht == null)
        {
            return 0;
        }

        List<ReserveringsStoelModel> passagiers = _reserveringLogic.GetStoelenByReserveringId(reserveringId);

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
        StoelModel? stoel = _stoelLogic.GetById(stoelId);
        if (stoel == null)
        {
            return 0;
        }

        StoelKlasseModel? stoelKlasse = _stoelKlasseLogic.GetById(stoel.StoelKlasse);
        if (stoelKlasse == null)
        {
            return 0;
        }

        return stoelKlasse.Toeslag;
    }
}
