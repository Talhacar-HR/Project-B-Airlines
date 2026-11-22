public class StoelkaartLogic
{
    private StoelAccess _stoelAccess = new();
    private ReserveringAccess _reserveringAccess = new();
    private VluchtLogic _vluchtLogic = new();
    private VliegtuigLogic _vliegtuigLogic = new();
    private ReserveringLogic _reserveringLogic = new();

    private static IndelingStoelenModel[] _indeling737 =
    {
        new IndelingStoelenModel(1, 33, "ABC DEF", 1)
    };

    private static IndelingStoelenModel[] _indeling330 =
    {
        new IndelingStoelenModel(1, 3, "AC DG HK", 3),
        new IndelingStoelenModel(4, 50, "AC DFG HK", 1)
    };

    private static IndelingStoelenModel[] _indeling787 =
    {
        new IndelingStoelenModel(1, 6, "AB DE KL", 2),
        new IndelingStoelenModel(16, 38, "ABC DEF JKL", 1)
    };

    public IndelingStoelenModel[] GetIndeling(string model)
    {
        if (model.Contains("737"))
        {
            return _indeling737;
        }

        if (model.Contains("330"))
        {
            return _indeling330;
        }

        if (model.Contains("787"))
        {
            return _indeling787;
        }

        return new IndelingStoelenModel[0];
    }

    public List<StoelModel> MaakStoelLijst(int vliegtuigId, string model)
    {
        List<StoelModel> stoelen = new List<StoelModel>();

        foreach (IndelingStoelenModel sectie in GetIndeling(model))
        {
            for (int rij = sectie.VanRij; rij <= sectie.TotRij; rij += 1)
            {
                foreach (char letter in sectie.Letters)
                {
                    if (letter == ' ')
                    {
                        continue;
                    }

                    string stoelNummer = rij.ToString() + letter;
                    stoelen.Add(new StoelModel(0, vliegtuigId, stoelNummer, rij.ToString(), sectie.StoelKlasse));
                }
            }
        }

        return stoelen;
    }

    public int MaakStoelenAan(VliegtuigModel vliegtuig)
    {
        List<string> bestaandeNummers = new List<string>();
        foreach (StoelModel stoel in _stoelAccess.GetAll())
        {
            if (stoel.VliegtuigID == vliegtuig.ID)
            {
                bestaandeNummers.Add(stoel.StoelNummer);
            }
        }

        int aantalToegevoegd = 0;
        foreach (StoelModel nieuweStoel in MaakStoelLijst(vliegtuig.ID, vliegtuig.Model))
        {
            if (!bestaandeNummers.Contains(nieuweStoel.StoelNummer))
            {
                _stoelAccess.Add(nieuweStoel);
                aantalToegevoegd += 1;
            }
        }

        return aantalToegevoegd;
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

    public List<PlattegrondRijModel> GetPlattegrond(int vluchtId)
    {
        List<PlattegrondRijModel> plattegrond = new List<PlattegrondRijModel>();

        VliegtuigModel? vliegtuig = GetVliegtuigVoorVlucht(vluchtId);
        if (vliegtuig == null)
        {
            return plattegrond;
        }

        List<StoelModel> stoelen = GetStoelenVoorVlucht(vluchtId);

        foreach (IndelingStoelenModel sectie in GetIndeling(vliegtuig.Model))
        {
            for (int rij = sectie.VanRij; rij <= sectie.TotRij; rij += 1)
            {
                PlattegrondRijModel plattegrondRij = new PlattegrondRijModel(rij, sectie.Letters);

                foreach (char letter in sectie.Letters)
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