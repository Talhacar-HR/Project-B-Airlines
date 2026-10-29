public class VluchtModel
{
    public Int64 ID { get; set; }

    public Int64 VliegtuigID { get; set; }

    public Int64 VertrekLocatie { get; set; }

    public Int64 AankomstLocatie { get; set; }

    public string VertreksTijd { get; set; }

    public string Vertraging { get; set; }

    public int VluchtNummer { get; set; }

    public VluchtModel()
    {
    }

    public VluchtModel(Int64 id, Int64 vliegtuigID, Int64 vertrekLocatie, Int64 aankomstLocatie,
        string vertreksTijd, string vertraging, int vluchtNummer)
    {
        ID = id;
        VliegtuigID = vliegtuigID;
        VertrekLocatie = vertrekLocatie;
        AankomstLocatie = aankomstLocatie;
        VertreksTijd = vertreksTijd;
        Vertraging = vertraging;
        VluchtNummer = vluchtNummer;
    }
}
