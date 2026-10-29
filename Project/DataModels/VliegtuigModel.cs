public class VliegtuigModel
{
    public Int64 ID { get; set; }

    public int BaggageRuimte { get; set; }

    public string Model { get; set; }

    public string Merk { get; set; }

    public VliegtuigModel()
    {
    }

    public VliegtuigModel(Int64 id, int baggageRuimte, string model, string merk)
    {
        ID = id;
        BaggageRuimte = baggageRuimte;
        Model = model;
        Merk = merk;
    }
}