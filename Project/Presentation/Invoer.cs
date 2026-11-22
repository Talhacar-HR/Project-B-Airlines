static class Invoer
{
    // Vraagt een waarde, controleert hem meteen en vraagt opnieuw tot hij goed is.
    // De controle komt uit de logic layer, de foutmelding staat hier.
    public static string VraagTotGeldig(string vraag, Func<string, bool> controle, string foutmelding)
    {
        while (true)
        {
            Console.Write(vraag);
            string invoer = (Console.ReadLine() ?? "").Trim();

            if (controle(invoer))
                return invoer;

            Console.WriteLine(foutmelding);
        }
    }
}
