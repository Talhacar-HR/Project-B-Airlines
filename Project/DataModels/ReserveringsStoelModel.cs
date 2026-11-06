public class ReserveringsStoelModel
{
    public int ID { get; set; }

    public int PassagierID { get; set; }

    public int ReserveringID { get; set; }

    public int StoelID { get; set; }

    // 0 = geen extra bagage, 1 = wel extra bagage
    public int ExtraBagage { get; set; }

    public ReserveringsStoelModel()
    {
    }

    public ReserveringsStoelModel(int id, int passagierID, int reserveringID, int stoelID, int extraBagage = 0)
    {
        ID = id;
        PassagierID = passagierID;
        ReserveringID = reserveringID;
        StoelID = stoelID;
        ExtraBagage = extraBagage;
    }
}
