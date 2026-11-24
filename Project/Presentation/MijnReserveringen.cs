static class MijnReserveringen
{
    static private ReserveringLogic reserveringLogic = new ReserveringLogic();


    public static string Start()
    {
        List<ReserveringModel> reserveringen = reserveringLogic.GetMijnReserveringen();

        if (reserveringen.Count == 0)
        {
            return "U heeft nog geen reserveringen.";
        }

        List<string> feedback = new()
        {
            "Mijn reserveringen",
            "",
            "Boekingscode | Vlucht | Status | Aangemaakt op",
            "--------------------------------------------"
        };

        foreach (ReserveringModel reservering in reserveringen)
        {
            feedback.Add(
                reservering.BoekingsCode + " | " + reservering.VluchtID + " | " +
                reservering.Status + " | " + reservering.AanmaakDatum);
        }

        return string.Join(Environment.NewLine, feedback);
    }
}