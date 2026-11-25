public class StoelkaartLogic
{
    private ReserveringAccess _reserveringAccess = new();
    private VluchtLogic _vluchtLogic = new();
    private VliegtuigLogic _vliegtuigLogic = new();
    private ReserveringLogic _reserveringLogic = new();

    public string GetKolommen(string model)
    {
        if (model.Contains("737"))
        {
            return "ABC DEF";
        }

        if (model.Contains("330"))
        {
            return "ABC DFG HJK";
        }

        if (model.Contains("787"))
        {
            return "ABC DEF JKL";
        }

        return "";
    }

    public VliegtuigModel? GetVliegtuigVoorVlucht(int vluchtId)
    {
        VluchtModel? vlucht = _vluchtLogic.GetById(vluchtId);
        if (vlucht == null)
        {
            return null;
        }

        return _vliegtuigLogic.GetById(vlucht.VliegtuigID);
    }

    public List<StoelModel> GetStoelenVoorVlucht(int vluchtId)
    {
        return _reserveringLogic.GetStoelenVoorVlucht(vluchtId);
    }

    public List<int> GetBezetteStoelIds(int vluchtId)
    {
        return _reserveringAccess.GetBezetteStoelIds(vluchtId);
    }

    public StoelModel? ZoekStoel(List<StoelModel> stoelen, string stoelNummer)
    {
        string gezocht = stoelNummer.Trim().ToUpper();

        foreach (StoelModel stoel in stoelen)
        {
            if (stoel.StoelNummer.ToUpper() == gezocht)
            {
                return stoel;
            }
        }

        return null;
    }

    public string GetStatus(int stoelId, List<int> bezetteStoelIds, List<int> gekozenStoelIds)
    {
        if (bezetteStoelIds.Contains(stoelId))
        {
            return "bezet";
        }

        if (gekozenStoelIds.Contains(stoelId))
        {
            return "gekozen";
        }

        return "vrij";
    }

    public List<int> GetRijNummers(List<StoelModel> stoelen)
    {
        List<int> rijNummers = new List<int>();

        foreach (StoelModel stoel in stoelen)
        {
            if (int.TryParse(stoel.RijNummer, out int rijNummer) && !rijNummers.Contains(rijNummer))
            {
                rijNummers.Add(rijNummer);
            }
        }

        rijNummers.Sort();
        return rijNummers;
    }

    public List<PlattegrondRijModel> GetPlattegrond(int vluchtId)
    {
        List<PlattegrondRijModel> plattegrond = new List<PlattegrondRijModel>();

        VliegtuigModel? vliegtuig = GetVliegtuigVoorVlucht(vluchtId);
        if (vliegtuig == null)
        {
            return plattegrond;
        }

        string kolommen = GetKolommen(vliegtuig.Model);
        if (kolommen == "")
        {
            return plattegrond;
        }

        List<StoelModel> stoelen = GetStoelenVoorVlucht(vluchtId);

        foreach (int rij in GetRijNummers(stoelen))
        {
            PlattegrondRijModel plattegrondRij = new PlattegrondRijModel(rij, kolommen);

            foreach (char letter in kolommen)
            {
                if (letter == ' ')
                {
                    continue;
                }

                StoelModel? stoel = ZoekStoel(stoelen, rij.ToString() + letter);
                if (stoel == null)
                {
                    continue;
                }

                string status = "Beschikbaar";
                if (_reserveringLogic.IsStoelBezet(vluchtId, stoel.ID))
                {
                    status = "Bezet";
                }

                plattegrondRij.Stoelen.Add(new PlattegrondStoelModel(stoel, status));
            }

            if (plattegrondRij.Stoelen.Count > 0)
            {
                plattegrond.Add(plattegrondRij);
            }
        }

        return plattegrond;
    }

    public bool KanStoelKiezen(int vluchtId, int stoelId)
    {
        bool stoelZitInVliegtuig = false;
        foreach (StoelModel stoel in GetStoelenVoorVlucht(vluchtId))
        {
            if (stoel.ID == stoelId)
            {
                stoelZitInVliegtuig = true;
            }
        }

        if (!stoelZitInVliegtuig)
        {
            return false;
        }

        if (_reserveringLogic.IsStoelBezet(vluchtId, stoelId))
        {
            return false;
        }

        return true;
    }
}
