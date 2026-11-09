public class VliegtuigModel
{
    public int ID { get; set; }

    public int BaggageRuimte { get; set; }

    public string Model { get; set; }

    public string Merk { get; set; }

    public VliegtuigModel()
    {
    }

    public VliegtuigModel(int id, int baggageRuimte, string model, string merk)
    {
        ID = id;
        BaggageRuimte = baggageRuimte;
        Model = model;
        Merk = merk;
    }
}