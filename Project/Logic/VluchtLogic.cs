public class VluchtLogic
{
    private VluchtAccess _access = new();
    private VliegtuigAccess _vliegtuigAccess = new();
    private VliegveldAccess _vliegveldAccess = new();

    public VluchtLogic()
    {
    }

    public List<VluchtModel> GetAll()
    {
        return _access.GetAll();
    }

    public void Add(VluchtModel flight)
    {
        ArgumentNullException.ThrowIfNull(flight);
        Validate(flight);
        _access.Add(flight);
    }

    public void Update(VluchtModel flight)
    {
        ArgumentNullException.ThrowIfNull(flight);
        if (flight.ID <= 0)
        {
            throw new ArgumentException("Het vlucht-ID moet groter zijn dan nul.", nameof(flight));
        }

        Validate(flight);
        if (!_access.Update(flight))
        {
            throw new KeyNotFoundException($"Er bestaat geen vlucht met ID {flight.ID}.");
        }
    }

    private void Validate(VluchtModel flight)
    {
        if (flight.VliegtuigID <= 0 || !_vliegtuigAccess.Exists(flight.VliegtuigID))
        {
            throw new ArgumentException($"Er bestaat geen vliegtuig met ID {flight.VliegtuigID}.", nameof(flight));
        }

        if (flight.VertrekLocatie <= 0 || !_vliegveldAccess.Exists(flight.VertrekLocatie))
        {
            throw new ArgumentException($"Er bestaat geen vertrekluchthaven met ID {flight.VertrekLocatie}.", nameof(flight));
        }

        if (flight.AankomstLocatie <= 0 || !_vliegveldAccess.Exists(flight.AankomstLocatie))
        {
            throw new ArgumentException($"Er bestaat geen aankomstluchthaven met ID {flight.AankomstLocatie}.", nameof(flight));
        }

        if (flight.VertrekLocatie == flight.AankomstLocatie)
        {
            throw new ArgumentException("De vertrek- en aankomstluchthaven moeten verschillend zijn.", nameof(flight));
        }

        if (string.IsNullOrWhiteSpace(flight.VertreksTijd) ||
            string.IsNullOrWhiteSpace(flight.Vertraging))
        {
            throw new ArgumentException("Vertrektijd en vertraging zijn verplicht.", nameof(flight));
        }

        if (flight.VluchtNummer <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(flight), "Het vluchtnummer moet groter zijn dan nul.");
        }
    }
}
