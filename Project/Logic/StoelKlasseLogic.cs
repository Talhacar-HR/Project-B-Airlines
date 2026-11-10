public class StoelKlasseLogic
{
    private StoelKlasseAccess _access = new();

    public List<StoelKlasseModel> GetAll()
    {
        return _access.GetAll();
    }

    public StoelKlasseModel? GetById(int id)
    {
        return _access.GetById(id);
    }
}
