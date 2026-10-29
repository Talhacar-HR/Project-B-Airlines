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

    public void Add(VliegveldModel airport)
    {
        ArgumentNullException.ThrowIfNull(airport);
        Validate(airport);
        _access.Add(airport);
    }

    public void Update(VliegveldModel airport)
    {
        ArgumentNullException.ThrowIfNull(airport);
        if (airport.ID <= 0)
        {
            throw new ArgumentException("Het vliegveld-ID moet groter zijn dan nul.", nameof(airport));
        }

        Validate(airport);
        if (!_access.Update(airport))
        {
            throw new KeyNotFoundException($"Er bestaat geen vliegveld met ID {airport.ID}.");
        }
    }

    private static void Validate(VliegveldModel airport)
    {
        if (string.IsNullOrWhiteSpace(airport.Naam) ||
            string.IsNullOrWhiteSpace(airport.Stad) ||
            string.IsNullOrWhiteSpace(airport.Land) ||
            string.IsNullOrWhiteSpace(airport.Afkorting))
        {
            throw new ArgumentException("Naam, stad, land en afkorting zijn verplicht.", nameof(airport));
        }
    }
}