static class MijnReserveringen
{
    static private ReserveringLogic reserveringLogic = new ReserveringLogic();


    public static void Start()
    {
        List<ReserveringModel> reserveringen = reserveringLogic.GetMijnReserveringen();

        Console.WriteLine("Mijn reserveringen");
        Console.WriteLine("");

        if (reserveringen.Count == 0)
        {
            Console.WriteLine("U heeft nog geen reserveringen.");
            return;
        }

        Console.WriteLine("Boekingscode | Vlucht | Status | Aangemaakt op");
        Console.WriteLine("--------------------------------------------");

        foreach (ReserveringModel reservering in reserveringen)
        {
            Console.WriteLine(reservering.BoekingsCode + " | " + reservering.VluchtID + " | " + reservering.Status + " | " + reservering.AanmaakDatum);
        }
    }
}