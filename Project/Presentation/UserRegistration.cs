static class UserRegistration
{
    static private UserLogic userLogic = new UserLogic();
 
    public static string Start()
    {
        Menu.PrintTitel();

        Console.WriteLine("Registreren");
        string email = VraagEmail();
        string password = Invoer.VraagTotGeldig("Wachtwoord: ", userLogic.IsValidWachtwoord,
            "Het wachtwoord moet minimaal 6 tekens zijn, zonder spaties.");

        if (userLogic.Register(email, password))
            return "Uw account is aangemaakt. U kunt nu inloggen.";
        else
            return "Registreren is mislukt. Probeer het later opnieuw.";
    }

    // Vraagt een geldig e-mailadres dat nog niet in gebruik is
    private static string VraagEmail()
    {
        while (true)
        {
            string email = Invoer.VraagTotGeldig("E-mailadres: ", userLogic.IsValidEmail,
                "Ongeldig e-mailadres, bijvoorbeeld naam@voorbeeld.nl.");

            if (userLogic.IsEmailBeschikbaar(email))
                return email;

            Console.WriteLine("Dit e-mailadres is al in gebruik.");
        }
    }
}
