public class StoelIndelingModel
{
    public int RijNummer {get; set;}
    public string StoelLetters{get; set;}
    public int StoelKlasse {get; set;}

    public StoelIndelingModel(int rijNummer, string stoelLetters, int stoelklasse)
    {
        RijNummer = rijNummer;
        StoelLetters = stoelLetters;
        StoelKlasse = stoelklasse;
    }
}