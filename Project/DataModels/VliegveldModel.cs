public class VliegveldModel
{
    public Int64 ID { get; set; }

    public string Naam { get; set; }

    public string Stad { get; set; }

    public string Land { get; set; }

    public string Afkorting { get; set; }

    public VliegveldModel()
    {
    }

    public VliegveldModel(Int64 id, string naam, string stad, string land, string afkorting)
    {
        ID = id;
        Naam = naam;
        Stad = stad;
        Land = land;
        Afkorting = afkorting;
    }
}