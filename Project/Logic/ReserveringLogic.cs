public class ReserveringLogic
{
    private ReserveringAccess _reserveringAccess = new();
    private VluchtAccess _vluchtAccess = new();
    private StoelAccess _stoelAccess = new();

    private Random _random = new Random();
    public int LaatsteBoekingsCode { get; private set; }
    public bool MaakReservering(int vluchtId, List<ReserveringsStoelModel> stoelen)
    {
        AccountModel? gebruiker = AccountsLogic.CurrentAccount;
        if (gebruiker == null)
        {
            return false;
        }

        if (stoelen == null || stoelen.Count == 0)
        {
            return false;
        }

        VluchtModel? vlucht = ZoekVlucht(vluchtId);
        if (vlucht == null)
        {
            return false;
        }

        List<int> gekozenStoelIds = new List<int>();

        foreach (ReserveringsStoelModel reserveringsStoel in stoelen)
        {
            StoelModel? stoel = ZoekStoel(reserveringsStoel.StoelID);

            if (stoel == null)
            {
                return false;
            }

            if (stoel.VliegtuigID != vlucht.VliegtuigID)
            {
                return false;
            }

            if (IsStoelBezet(vluchtId, stoel.ID))
            {
                return false;
            }

            if (gekozenStoelIds.Contains(stoel.ID))
            {
                return false;
            }

            gekozenStoelIds.Add(stoel.ID);
        }

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
        LaatsteBoekingsCode = reservering.BoekingsCode;
        return true;
    }

    public List<ReserveringModel> GetMijnReserveringen()
    {
        AccountModel? gebruiker = AccountsLogic.CurrentAccount;

        if (gebruiker == null)
        {
            return new List<ReserveringModel>();
        }

        return _reserveringAccess.GetByGebruikerId(gebruiker.Id);
    }

    public ReserveringModel? GetById(int id)
    {
        return _reserveringAccess.GetById(id);
    }

    public List<ReserveringsStoelModel> GetStoelenByReserveringId(int reserveringId)
    {
        return _reserveringAccess.GetStoelenByReserveringId(reserveringId);
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
    public List<StoelModel> GetStoelenVoorVlucht(int vluchtId)
    {
        List<StoelModel> stoelenVanVlucht = new List<StoelModel>();

        VluchtModel? vlucht = ZoekVlucht(vluchtId);
        if (vlucht == null)
        {
            return stoelenVanVlucht;
        }

        foreach (StoelModel stoel in _stoelAccess.GetAll())
        {
            if (stoel.VliegtuigID == vlucht.VliegtuigID)
            {
                stoelenVanVlucht.Add(stoel);
            }
        }

        return stoelenVanVlucht;
    }
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