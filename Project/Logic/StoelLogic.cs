public class StoelLogic
{
    private StoelAccess _access = new();
    private VliegtuigLogic _vliegtuigLogic = new();

    public StoelLogic()
    {
    }

    public List<StoelModel> GetAll()
    {
        return _access.GetAll();
    }

    public StoelModel? GetById(int id)
    {
        return _access.GetById(id);
    }

    public void Add(StoelModel seat)
    {
        _access.Add(seat);
    }

    public int GetTotalSeats(int vliegtuigID)
    {
        return _access.GetTotalSeats(vliegtuigID);
    }

    public bool Update(StoelModel seat)
    {
        return _access.Update(seat);
    }

    public bool IsValidVliegtuigID(int vliegtuigID)
    {
        return _vliegtuigLogic.Exists(vliegtuigID);
    }

    public bool IsValidStoelNummer(string stoelNummer)
    {
        return !string.IsNullOrWhiteSpace(stoelNummer);
    }

    public bool IsValidRijNummer(string rijNummer)
    {
        return !string.IsNullOrWhiteSpace(rijNummer);
    }

    public bool IsValidStoelKlasse(int stoelKlasse)
    {
        return stoelKlasse > 0;
    }
}
