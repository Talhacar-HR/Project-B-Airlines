public class ReserveringLogic
{
    private ReserveringAccess _reserveringAccess = new();
    private VluchtAccess _vluchtAccess = new();
    private StoelAccess _stoelAccess = new();

    private Random _random = new Random();

    // Geeft true terug als de reservering gelukt is, anders false
    public bool MaakReservering(int vluchtId, List<ReserveringsStoelModel> stoelen)
    {
        // 1. De gebruiker moet ingelogd zijn
        AccountModel? gebruiker = AccountsLogic.CurrentAccount;
        if (gebruiker == null)
        {
            return false;
        }

        // 2. Er moet minstens één stoel gekozen zijn
        if (stoelen == null || stoelen.Count == 0)
        {
            return false;
        }

        // 3. De vlucht moet bestaan
        VluchtModel? vlucht = ZoekVlucht(vluchtId);
        if (vlucht == null)
        {
            return false;
        }

        // 4. Elke stoel moet bij de vlucht horen en vrij zijn
        List<int> gekozenStoelIds = new List<int>();

        foreach (ReserveringsStoelModel reserveringsStoel in stoelen)
        {
            StoelModel? stoel = ZoekStoel(reserveringsStoel.StoelID);

            // Stoel bestaat niet
            if (stoel == null)
            {
                return false;
            }

            // Stoel zit niet in het vliegtuig van deze vlucht
            if (stoel.VliegtuigID != vlucht.VliegtuigID)
            {
                return false;
            }

            // Stoel is al door iemand anders gereserveerd
            if (IsStoelBezet(vluchtId, stoel.ID))
            {
                return false;
            }

            // Dezelfde stoel is twee keer gekozen in deze reservering
            if (gekozenStoelIds.Contains(stoel.ID))
            {
                return false;
            }

            gekozenStoelIds.Add(stoel.ID);
        }

        // 5. Alles klopt, dus maak de reservering
        string nu = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        ReserveringModel reservering = new ReserveringModel();
        reservering.GebruikersID = gebruiker.Id;
        reservering.VluchtID = vluchtId;
        reservering.Status = "Bevestigd";
        reservering.BoekingsCode = MaakBoekingsCode();
        reservering.AanmaakDatum = nu;
        reservering.LaatsteWijziging = nu;
        reservering.Stoelen = stoelen;

        _reserveringAccess.Add(reservering);
        return true;
    }

    // Geeft alleen de reserveringen van de ingelogde gebruiker terug
    public List<ReserveringModel> GetMijnReserveringen()
    {
        AccountModel? gebruiker = AccountsLogic.CurrentAccount;

        // Niet ingelogd, dus geen reserveringen
        if (gebruiker == null)
        {
            return new List<ReserveringModel>();
        }

        return _reserveringAccess.GetByGebruikerId(gebruiker.Id);
    }

    public bool IsStoelBezet(int vluchtId, int stoelId)
    {
        List<int> bezetteStoelIds = _reserveringAccess.GetBezetteStoelIds(vluchtId);
        return bezetteStoelIds.Contains(stoelId);
    }

    public int GetAantalBezetteStoelen(int vluchtID)
    {
        List<int> bezetteStoelIds = _reserveringAccess.GetBezetteStoelIds(vluchtID);
        return bezetteStoelIds.Count;
    }

    // Maakt een willekeurige code van 6 cijfers die nog niet bestaat
    private int MaakBoekingsCode()
    {
        int code = _random.Next(100000, 1000000);

        while (_reserveringAccess.BestaatBoekingsCode(code))
        {
            code = _random.Next(100000, 1000000);
        }

        return code;
    }

    private VluchtModel? ZoekVlucht(int vluchtId)
    {
        foreach (VluchtModel vlucht in _vluchtAccess.GetAll())
        {
            if (vlucht.ID == vluchtId)
            {
                return vlucht;
            }
        }

        return null;
    }

    private StoelModel? ZoekStoel(int stoelId)
    {
        foreach (StoelModel stoel in _stoelAccess.GetAll())
        {
            if (stoel.ID == stoelId)
            {
                return stoel;
            }
        }

        return null;
    }
}