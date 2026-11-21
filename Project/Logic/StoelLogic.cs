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
            indeling = GetAirbusIndeling();
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

    private List<StoelIndelingModel> GetAirbusIndeling()
    {
        List<StoelIndelingModel> airBusIndeling = [];

        // lijst waarin de rijen worden toegveogd, begin rij, stop bij rij, 
       // stoelletters bij rij, stoelklasse(club class)
       VoegRijenToe(airBusIndeling, 1, 2, "ACDGHK", 2); 
       VoegRijenToe(airBusIndeling, 4, 4, "ACDGHK", 1); // extra been ruimte
       VoegRijenToe(airBusIndeling, 5, 9, "ACDGHK", 1); // zitplaatsen voorin
       VoegRijenToe(airBusIndeling, 10, 10, "DFG", 1); // standaard
       VoegRijenToe(airBusIndeling, 14, 14, "ABCDFGHJK", 1); // extra been ruimte
       VoegRijenToe(airBusIndeling, 15, 33, "ABCDFGHJK", 1); // standaard
       VoegRijenToe(airBusIndeling, 36, 36, "ABCDFGHJK", 1); // extra been ruimte
       VoegRijenToe(airBusIndeling, 37, 43, "ACDFGHJK", 1); // standaard
       VoegRijenToe(airBusIndeling, 44, 44, "ACDFGHK", 1); // standaard
       VoegRijenToe(airBusIndeling, 45, 50, "ACDFGHK", 1); // twee stoelen naast elkaar (AC en HK), DFG standaard




       return airBusIndeling;
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
