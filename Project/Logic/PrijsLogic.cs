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

    // Prijsregels voor het prijsoverzicht: 1 regel per passagier.
    // Werkt ook voor stoelen die nog niet zijn opgeslagen (voor het bevestigen).
    public List<PrijsRegelModel> GetPrijsRegels(int vluchtId, List<ReserveringsStoelModel> stoelen)
    {
        List<PrijsRegelModel> regels = new List<PrijsRegelModel>();

        VluchtModel? vlucht = _vluchtLogic.GetById(vluchtId);
        if (vlucht == null || stoelen == null)
        {
            return regels;
        }

        foreach (ReserveringsStoelModel stoel in stoelen)
        {
            regels.Add(MaakPrijsRegel(vlucht, stoel));
        }

        return regels;
    }

    // Totaalprijs van een vlucht met gekozen stoelen: alle prijsregels bij elkaar
    public double BerekenTotaalprijs(int vluchtId, List<ReserveringsStoelModel> stoelen)
    {
        double totaal = 0;
        foreach (PrijsRegelModel regel in GetPrijsRegels(vluchtId, stoelen))
        {
            totaal += regel.Prijs;
        }

        // Afronden op centen, zodat er geen getallen als 859.9800000001 uitkomen
        return Math.Round(totaal, 2);
    }

    // Totaalprijs van een opgeslagen reservering.
    // Een onbekende reservering of een reservering zonder passagiers geeft 0 terug (geen crash).
    public double BerekenTotaalprijs(int reserveringId)
    {
        ReserveringModel? reservering = _reserveringLogic.GetById(reserveringId);
        if (reservering == null)
        {
            return 0;
        }

        List<ReserveringsStoelModel> stoelen = _reserveringLogic.GetStoelenByReserveringId(reserveringId);
        return BerekenTotaalprijs(reservering.VluchtID, stoelen);
    }

    // Maakt 1 prijsregel voor 1 passagier
    private PrijsRegelModel MaakPrijsRegel(VluchtModel vlucht, ReserveringsStoelModel reserveringsStoel)
    {
        StoelModel? stoel = _stoelLogic.GetById(reserveringsStoel.StoelID);
        string stoelNummer = stoel == null ? "" : stoel.StoelNummer;

        double toeslag = GetStoelToeslag(reserveringsStoel.StoelID);
        bool extraBagage = reserveringsStoel.ExtraBagage == 1;
        double prijs = BerekenPrijsPerPassagier(vlucht.Prijs, toeslag, extraBagage);

        return new PrijsRegelModel(vlucht.VluchtNummer, vlucht.Prijs, stoelNummer, toeslag, extraBagage, prijs);
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
