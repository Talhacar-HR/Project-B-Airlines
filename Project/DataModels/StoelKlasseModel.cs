public class StoelKlasseModel
{
    public int ID { get; set; }

    public string Naam { get; set; }

    public double Toeslag { get; set; }

    public StoelKlasseModel()
    {
    }

    public StoelKlasseModel(int id, string naam, double toeslag)
    {
        ID = id;
        Naam = naam;
        Toeslag = toeslag;
    }
}
