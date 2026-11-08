public class PrijsLogic
{
    // Vaste prijs voor extra bagage per passagier
    public const double ExtraBagagePrijs = 50;

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
