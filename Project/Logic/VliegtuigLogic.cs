public class VliegtuigLogic
{
    private VliegtuigAccess _access = new();

    public VliegtuigLogic()
    {
    }

    public List<VliegtuigModel> GetAll()
    {
        return _access.GetAll();
    }

    public bool Exists(int id)
    {
        return _access.Exists(id);
    }

    public VliegtuigModel? GetById(int id)
    {
        return _access.GetById(id);
    }

    public void Add(VliegtuigModel plane)
    {
        _access.Add(plane);
    }

    public bool Update(VliegtuigModel plane)
    {
        return _access.Update(plane);
    }

    public bool IsValidBaggageRuimte(int baggageRuimte)
    {
        return baggageRuimte >= 0;
    }

    public bool IsValidModel(string model)
    {
        return !string.IsNullOrWhiteSpace(model);
    }

    public bool IsValidMerk(string merk)
    {
        return !string.IsNullOrWhiteSpace(merk);
    }
    public List<VliegtuigModel> Zoek(string zoekterm)
    {
        List<VliegtuigModel> gevonden = new List<VliegtuigModel>();

        if (string.IsNullOrWhiteSpace(zoekterm))
        {
            return gevonden;
        }

        string term = zoekterm.Trim().ToLower();

        foreach (VliegtuigModel vliegtuig in _access.GetAll())
        {
            if (vliegtuig.Model.ToLower().Contains(term))
            {
                gevonden.Add(vliegtuig);
            }
        }

        return gevonden;
    }
}
