public class VluchtModel
{
    public int ID { get; set; }

    public int VliegtuigID { get; set; }

    public int VertrekLocatie { get; set; }

    public int AankomstLocatie { get; set; }

    public string VertreksTijd { get; set; }

    public string Vertraging { get; set; }

    public int VluchtNummer { get; set; }

    public VluchtModel()
    {
    }

    public VluchtModel(int id, int vliegtuigID, int vertrekLocatie, int aankomstLocatie,
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
