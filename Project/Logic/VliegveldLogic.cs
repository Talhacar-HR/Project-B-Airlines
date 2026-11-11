public class VliegveldLogic
{
    private VliegveldAccess _access = new();

    public VliegveldLogic()
    {
    }

    public List<VliegveldModel> GetAll()
    {
        return _access.GetAll();
    }

    public bool Exists(int id)
    {
        return _access.Exists(id);
    }

    public VliegveldModel? GetById(int id)
    {
        return _access.GetById(id);
    }

    public void Add(VliegveldModel airport)
    {
        _access.Add(airport);
    }

    public bool Update(VliegveldModel airport)
    {
        return _access.Update(airport);
    }

    public bool IsValidNaam(string naam)
    {
        return !string.IsNullOrWhiteSpace(naam);
    }

    public bool IsValidStad(string stad)
    {
        return !string.IsNullOrWhiteSpace(stad);
    }

    public bool IsValidLand(string land)
    {
        return !string.IsNullOrWhiteSpace(land);
    }

    public bool IsValidAfkorting(string afkorting)
    {
        return !string.IsNullOrWhiteSpace(afkorting);
    }
    public List<VliegveldModel> Zoek(string zoekterm)
    {
        List<VliegveldModel> gevonden = new List<VliegveldModel>();

        if (string.IsNullOrWhiteSpace(zoekterm))
        {
            return gevonden;
        }

        string term = zoekterm.Trim().ToLower();

        foreach (VliegveldModel vliegveld in _access.GetAll())
        {
            if (vliegveld.Afkorting.ToLower() == term)
            {
                gevonden.Add(vliegveld);
            }
        }

        return gevonden;
    }
}
