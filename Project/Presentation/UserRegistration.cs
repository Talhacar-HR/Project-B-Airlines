static class UserRegistration
{
    static private UserLogic userLogic = new UserLogic();
 
 
    public static void Start()
    {
        Console.WriteLine("Registreren");
        Console.WriteLine("Voer je e-mailadres in:");
        string email = Console.ReadLine();
        Console.WriteLine("Voer je wachtwoord in:");
        string password = Console.ReadLine();
 
        bool success = userLogic.Register(email, password);
 
        if (success)
        {
            Console.WriteLine("Je account is aangemaakt. Je kunt nu inloggen.");
        }
        else
        {
            Console.WriteLine("Registreren is mislukt. Het e-mailadres is ongeldig of bestaat al, of het wachtwoord is leeg.");
        }
    }
}