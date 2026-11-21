public class PlattegrondLogic
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

    // Geeft het vliegtuig dat bij een vlucht hoort, of null
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

    // Zoekt een stoel in een lijst op stoelnummer, bijvoorbeeld "12C". Hoofdletters maken niet uit.
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

    // Geeft de status van een stoel: "bezet", "gekozen" of "vrij"
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
}