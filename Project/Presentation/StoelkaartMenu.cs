static class StoelkaartMenu
{
    static private StoelkaartLogic stoelkaartLogic = new StoelkaartLogic();

    static private int aantalRijenOpScherm = 15;

    public static int Start(int vluchtId, List<ReserveringsStoelModel> gekozenStoelen)
    {
        VliegtuigModel? vliegtuig = stoelkaartLogic.GetVliegtuigVoorVlucht(vluchtId);
        if (vliegtuig == null)
        {
            Console.WriteLine("Het vliegtuig van deze vlucht is niet gevonden.");
            return 0;
        }

        List<PlattegrondRijModel> plattegrond = stoelkaartLogic.GetPlattegrond(vluchtId);
        if (plattegrond.Count == 0)
        {
            Console.WriteLine("Er is geen plattegrond voor dit vliegtuig.");
            return 0;
        }

        List<int> gekozenStoelIds = new List<int>();
        foreach (ReserveringsStoelModel gekozen in gekozenStoelen)
        {
            gekozenStoelIds.Add(gekozen.StoelID);
        }

        bool vrijeStoelGevonden = false;
        foreach (PlattegrondRijModel rij in plattegrond)
        {
            foreach (PlattegrondStoelModel plattegrondStoel in rij.Stoelen)
            {
                if (!IsBezet(plattegrondStoel, gekozenStoelIds))
                {
                    vrijeStoelGevonden = true;
                }
            }
        }

        if (!vrijeStoelGevonden)
        {
            Console.WriteLine("Er zijn geen vrije stoelen meer op deze vlucht.");
            return 0;
        }

        int rijIndex = 0;
        int stoelIndex = 0;
        string melding = "";

        while (true)
        {
            ToonPlattegrond(vliegtuig, plattegrond, gekozenStoelIds, rijIndex, stoelIndex, melding);
            melding = "";

            ConsoleKey toets = Console.ReadKey(true).Key;

            if (toets == ConsoleKey.UpArrow && rijIndex > 0)
            {
                rijIndex -= 1;
            }
            else if (toets == ConsoleKey.DownArrow && rijIndex < plattegrond.Count - 1)
            {
                rijIndex += 1;
            }
            else if (toets == ConsoleKey.LeftArrow && stoelIndex > 0)
            {
                stoelIndex -= 1;
            }
            else if (toets == ConsoleKey.RightArrow && stoelIndex < plattegrond[rijIndex].Stoelen.Count - 1)
            {
                stoelIndex += 1;
            }
            else if (toets == ConsoleKey.Escape)
            {
                Console.WriteLine("Er is geen stoel gekozen.");
                return 0;
            }
            else if (toets == ConsoleKey.Enter)
            {
                StoelModel stoel = plattegrond[rijIndex].Stoelen[stoelIndex].Stoel;

                if (gekozenStoelIds.Contains(stoel.ID) || !stoelkaartLogic.KanStoelKiezen(vluchtId, stoel.ID))
                {
                    melding = $"Stoel {stoel.StoelNummer} is al bezet. Kies een andere stoel.";
                }
                else
                {
                    Console.Write($"Wilt u stoel {stoel.StoelNummer} kiezen? (J/N): ");
                    string keuze = Console.ReadLine() ?? "";

                    if (keuze.Trim().ToLower() == "j")
                    {
                        return stoel.ID;
                    }
                }
            }

            if (stoelIndex > plattegrond[rijIndex].Stoelen.Count - 1)
            {
                stoelIndex = plattegrond[rijIndex].Stoelen.Count - 1;
            }
        }
    }

    private static void ToonPlattegrond(VliegtuigModel vliegtuig, List<PlattegrondRijModel> plattegrond,
        List<int> gekozenStoelIds, int rijIndex, int stoelIndex, string melding)
    {
        //Console.Clear();
        Console.WriteLine($"Plattegrond {vliegtuig.Merk} {vliegtuig.Model}  (voorkant vliegtuig = boven)");
        ToonLegenda();
        Console.WriteLine("Pijltjestoetsen = bewegen   Enter = stoel kiezen   Esc = stoppen");
        Console.WriteLine("");

        int eersteRij = rijIndex - aantalRijenOpScherm / 2;
        if (eersteRij > plattegrond.Count - aantalRijenOpScherm)
        {
            eersteRij = plattegrond.Count - aantalRijenOpScherm;
        }
        if (eersteRij < 0)
        {
            eersteRij = 0;
        }

        int laatsteRij = eersteRij + aantalRijenOpScherm - 1;
        if (laatsteRij > plattegrond.Count - 1)
        {
            laatsteRij = plattegrond.Count - 1;
        }

        for (int i = eersteRij; i <= laatsteRij; i += 1)
        {
            PlattegrondRijModel rij = plattegrond[i];
            Console.Write(rij.RijNummer.ToString("00") + "  ");

            foreach (char letter in rij.Letters)
            {
                if (letter == ' ')
                {
                    Console.Write("   ");
                    continue;
                }

                int positie = ZoekPositie(rij, letter);
                if (positie == -1)
                {
                    Console.Write("   ");
                    continue;
                }

                PlattegrondStoelModel plattegrondStoel = rij.Stoelen[positie];

                if (i == rijIndex && positie == stoelIndex)
                {
                    ToonStoel(letter, ConsoleColor.Yellow);
                }
                else if (IsBezet(plattegrondStoel, gekozenStoelIds))
                {
                    ToonStoel(letter, ConsoleColor.DarkRed);
                }
                else
                {
                    ToonStoel(letter, ConsoleColor.DarkGreen);
                }
            }

            Console.WriteLine("");
        }

        Console.WriteLine("");
        Console.WriteLine($"Rij {plattegrond[eersteRij].RijNummer} t/m {plattegrond[laatsteRij].RijNummer} van de {plattegrond.Count} rijen");

        PlattegrondStoelModel geselecteerd = plattegrond[rijIndex].Stoelen[stoelIndex];
        string status = "beschikbaar";
        if (IsBezet(geselecteerd, gekozenStoelIds))
        {
            status = "bezet";
        }
        Console.WriteLine($"Geselecteerd: stoel {geselecteerd.Stoel.StoelNummer} ({status})");

        if (melding != "")
        {
            Console.WriteLine(melding);
        }
    }

    private static int ZoekPositie(PlattegrondRijModel rij, char letter)
    {
        string stoelNummer = rij.RijNummer.ToString() + letter;

        for (int i = 0; i < rij.Stoelen.Count; i += 1)
        {
            if (rij.Stoelen[i].Stoel.StoelNummer == stoelNummer)
            {
                return i;
            }
        }

        return -1;
    }

    private static bool IsBezet(PlattegrondStoelModel plattegrondStoel, List<int> gekozenStoelIds)
    {
        if (plattegrondStoel.Status == "Bezet")
        {
            return true;
        }

        return gekozenStoelIds.Contains(plattegrondStoel.Stoel.ID);
    }

    private static void ToonStoel(char letter, ConsoleColor kleur)
    {
        Console.BackgroundColor = kleur;

        if (kleur == ConsoleColor.Yellow)
        {
            Console.ForegroundColor = ConsoleColor.Black;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        Console.Write("[" + letter + "]");
        Console.ResetColor();
    }

    private static void ToonLegenda()
    {
        ToonStoel(' ', ConsoleColor.DarkGreen);
        Console.Write(" beschikbaar   ");
        ToonStoel(' ', ConsoleColor.DarkRed);
        Console.Write(" bezet   ");
        ToonStoel(' ', ConsoleColor.Yellow);
        Console.WriteLine(" gekozen");
    }
}
