public class VluchtLogic
{
    private VluchtAccess _access = new();
    private VliegtuigLogic _vliegtuigLogic = new();
    private VliegveldLogic _vliegveldLogic = new();

    public VluchtLogic()
    {
    }

    public List<VluchtModel> GetAll()
    {
        return _access.GetAll();
    }

    public VluchtModel? GetById(int id)
    {
        return _access.GetById(id);
    }

    public void Add(VluchtModel flight)
    {
        _access.Add(flight);
    }

    public bool Update(VluchtModel flight)
    {
        return _access.Update(flight);
    }

    public bool IsValidId(int id)
    {
        return _access.Exists(id);
    }

    public bool IsValidVliegtuigID(int vliegtuigID)
    {
        return _vliegtuigLogic.Exists(vliegtuigID);
    }

    public bool IsValidVertrekLocatie(int vertrekLocatie)
    {
        return _vliegveldLogic.Exists(vertrekLocatie);
    }

    public bool IsValidAankomstLocatie(int aankomstLocatie)
    {
        return _vliegveldLogic.Exists(aankomstLocatie);
    }

    public bool AreDifferentLocations(int vertrekLocatie, int aankomstLocatie)
    {
        return vertrekLocatie != aankomstLocatie;
    }

    public bool IsValidVertrekDatum(string vertrekDatum)
    {
        return DateOnly.TryParseExact(vertrekDatum, "yyyy-MM-dd", out _);
    }

    public bool IsValidVertrekTijd(string vertrekTijd)
    {
        return TimeOnly.TryParseExact(vertrekTijd, "HH:mm", out _);
    }

    public bool IsValidVertraging(string vertraging)
    {
        return TimeOnly.TryParseExact(vertraging, "HH:mm", out _);
    }

    public bool IsValidVluchtNummer(int vluchtNummer, int id)
    {
        return vluchtNummer > 0 && !_access.VluchtNummerExists(vluchtNummer, id);
    }
}
