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

    public void Add(VliegtuigModel plane)
    {
        ArgumentNullException.ThrowIfNull(plane);
        Validate(plane);
        _access.Add(plane);
    }

    public void Update(VliegtuigModel plane)
    {
        ArgumentNullException.ThrowIfNull(plane);
        if (plane.ID <= 0)
        {
            throw new ArgumentException("Het vliegtuig-ID moet groter zijn dan nul.", nameof(plane));
        }

        Validate(plane);
        if (!_access.Update(plane))
        {
            throw new KeyNotFoundException($"Er bestaat geen vliegtuig met ID {plane.ID}.");
        }
    }

    private static void Validate(VliegtuigModel plane)
    {
        if (string.IsNullOrWhiteSpace(plane.Model) || string.IsNullOrWhiteSpace(plane.Merk))
        {
            throw new ArgumentException("Model en merk zijn verplicht.", nameof(plane));
        }
    }
}