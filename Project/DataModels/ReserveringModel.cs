public class ReserveringModel
{
    public int ID { get; set; }

    public int GebruikersID { get; set; }

    public int VluchtID { get; set; }

    public string Status { get; set; }

    public int BoekingsCode { get; set; }

    public string AanmaakDatum { get; set; }

    public string LaatsteWijziging { get; set; }

    public ReserveringModel()
    {
    }

    public ReserveringModel(int id, int gebruikersID, int vluchtID, string status, int boekingsCode, string aanmaakDatum, string laatsteWijziging)
    {
        ID = id;
        GebruikersID = gebruikersID;
        VluchtID = vluchtID;
        Status = status;
        BoekingsCode = boekingsCode;
        AanmaakDatum = aanmaakDatum;
        LaatsteWijziging = laatsteWijziging;
    }
}
