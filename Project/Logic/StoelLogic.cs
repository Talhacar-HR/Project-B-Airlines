public class StoelLogic
{
    private StoelAccess _access = new();
    private VliegtuigAccess _vliegtuigAccess = new();

    public StoelLogic()
    {
    }

    public List<StoelModel> GetAll()
    {
        return _access.GetAll();
    }

    public void Add(StoelModel seat)
    {
        ArgumentNullException.ThrowIfNull(seat);
        Validate(seat);
        _access.Add(seat);
    }

    public void Update(StoelModel seat)
    {
        ArgumentNullException.ThrowIfNull(seat);
        if (seat.ID <= 0)
        {
            throw new ArgumentException("Het stoel-ID moet groter zijn dan nul.", nameof(seat));
        }

        Validate(seat);
        if (!_access.Update(seat))
        {
            throw new KeyNotFoundException($"Er bestaat geen stoel met ID {seat.ID}.");
        }
    }

    private void Validate(StoelModel seat)
    {
        if (seat.VliegtuigID <= 0 || !_vliegtuigAccess.Exists(seat.VliegtuigID))
        {
            throw new ArgumentException($"Er bestaat geen vliegtuig met ID {seat.VliegtuigID}.", nameof(seat));
        }

        if (string.IsNullOrWhiteSpace(seat.StoelNummer) || string.IsNullOrWhiteSpace(seat.RijNummer))
        {
            throw new ArgumentException("Stoelnummer en rijnummer zijn verplicht.", nameof(seat));
        }

        if (seat.StoelKlasse <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(seat), "De stoelklasse moet groter zijn dan nul.");
        }
    }
}
