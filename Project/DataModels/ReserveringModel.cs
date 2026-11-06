public class ReserveringModel
{
    public int ID { get; set; }

    public int GebruikersID { get; set; }

    public int VluchtID { get; set; }

    public string Status { get; set; }

    public int BoekingsCode { get; set; }

    public string AanmaakDatum { get; set; }

    public string LaatsteWijziging { get; set; }

    public List<ReserveringsStoelModel> Stoelen { get; set; } = new List<ReserveringsStoelModel>();

    public ReserveringModel()
    {
    }
}