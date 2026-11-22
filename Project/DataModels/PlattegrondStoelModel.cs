public class PlattegrondStoelModel
{
    public StoelModel Stoel { get; set; }

    public string Status { get; set; }

    public PlattegrondStoelModel(StoelModel stoel, string status)
    {
        Stoel = stoel;
        Status = status;
    }
}
