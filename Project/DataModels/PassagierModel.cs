public class PassagierModel
{
    public int ID { get; set; }

    public int? GebruikerID { get; set; }

    public string Naam { get; set; }

    public string Achternaam { get; set; }

    public string Email { get; set; }

    public string PaspoortNummer { get; set; }

    public string GeboorteDatum { get; set; }

    public string Telefoonnummer { get; set; }

    public string PaspoortVerloopDatum { get; set; }

    public PassagierModel()
    {
    }

    public PassagierModel(
        int id,
        int gebruikerID,
        string naam,
        string achternaam,
        string email,
        string paspoortNummer,
        string geboorteDatum,
        string telefoonnummer,
        string paspoortVerloopDatum)
    {
        ID = id;
        GebruikerID = gebruikerID;
        Naam = naam;
        Achternaam = achternaam;
        Email = email;
        PaspoortNummer = paspoortNummer;
        GeboorteDatum = geboorteDatum;
        Telefoonnummer = telefoonnummer;
        PaspoortVerloopDatum = paspoortVerloopDatum;
    }
}