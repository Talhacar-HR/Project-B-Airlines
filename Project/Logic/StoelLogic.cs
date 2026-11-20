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

    public List<StoelModel> Zoek(string zoekterm)
    {
        List<StoelModel> gevonden = new List<StoelModel>();

        if (string.IsNullOrWhiteSpace(zoekterm))
        {
            return gevonden;
        }

        string term = zoekterm.Trim().ToLower();

        foreach (StoelModel stoel in GetAll())
        {
            if (stoel.StoelNummer.ToLower() == term)
            {
                gevonden.Add(stoel);
            }
        }

        return gevonden;
    }
      public List<StoelIndelingModel> GetIndeling(string model)
    {
        List<StoelIndelingModel> indeling = [];

        if (model == "Airbus A330-200")
        {
            throw new NotImplementedException();
        }
        else if (model == "Boeing 737")
        {
            throw new NotImplementedException();
        }
        else if (model == "Boeing 787")
        {
            throw new NotImplementedException();
        }

        return indeling;

    }

    private void VoegRijenToe(List<StoelIndelingModel> indeling,int vanafRij, 
    int totRij, string letters, int klasse)
    {
        for (int rij = vanafRij; rij <= totRij; rij++)
        {
            StoelIndelingModel stoelIndeling = new(rij, letters, klasse);
            indeling.Add(stoelIndeling);
        }
    }
}
