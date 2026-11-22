static class UserRegistration
{
    static private UserLogic userLogic = new UserLogic();
 
    public static string Start()
    {
        Menu.PrintTitel();

        Console.WriteLine("Registreren");
        Console.WriteLine("Voer je e-mailadres in:");
        string email = Console.ReadLine() ?? "";
        Console.WriteLine("Voer je wachtwoord in:");
        string password = Console.ReadLine() ?? "";
 
        if (userLogic.Register(email, password))
        {
            return "Je account is aangemaakt. Je kunt nu inloggen.";
        }
        else
        {
            return "Registreren is mislukt. Het e-mailadres is ongeldig of bestaat al, of het wachtwoord is leeg.";
        }
    }
}