using System.Globalization;

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
        if (flight.VliegtuigID <= 0 || !_vliegtuigLogic.Exists(flight.VliegtuigID))
        {
            throw new ArgumentException($"Er bestaat geen vliegtuig met ID {flight.VliegtuigID}.", nameof(flight));
        }

        if (flight.VertrekLocatie <= 0 || !_vliegveldLogic.Exists(flight.VertrekLocatie))
        {
            throw new ArgumentException($"Er bestaat geen vertrekluchthaven met ID {flight.VertrekLocatie}.", nameof(flight));
        }

        if (flight.AankomstLocatie <= 0 || !_vliegveldLogic.Exists(flight.AankomstLocatie))
        {
            throw new ArgumentException($"Er bestaat geen aankomstluchthaven met ID {flight.AankomstLocatie}.", nameof(flight));
        }

        if (flight.VertrekLocatie == flight.AankomstLocatie)
        {
            throw new ArgumentException("De vertrek- en aankomstluchthaven moeten verschillend zijn.", nameof(flight));
        }

        if (!DateOnly.TryParseExact(flight.VertrekDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            throw new ArgumentException("De vertrekdatum moet geldig zijn en het formaat yyyy-MM-dd hebben.", nameof(flight));
        }

        if (!TimeOnly.TryParseExact(flight.VertrekTijd, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            throw new ArgumentException("De vertrektijd moet geldig zijn en het formaat HH:mm hebben.", nameof(flight));
        }

        if (!TimeOnly.TryParseExact(flight.Vertraging, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            throw new ArgumentException("De vertraging moet geldig zijn en het formaat HH:mm hebben.", nameof(flight));
        }

        if (flight.VluchtNummer <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(flight), "Het vluchtnummer moet groter zijn dan nul.");
        }
    }
}
