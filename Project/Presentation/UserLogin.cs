static class UserLogin
{
    static private UserLogic userLogic = new UserLogic();
 
 
    public static void Start()
    {
        Console.WriteLine("Inloggen");
        Console.WriteLine("Voer je e-mailadres in:");
        string email = Console.ReadLine();
        Console.WriteLine("Voer je wachtwoord in:");
        string password = Console.ReadLine();
 
        bool success = userLogic.Login(email, password);
 
        if (success)
        {
            Console.WriteLine("Welkom, " + UserLogic.CurrentUser.Email + "!");
        }
        else
        {
            Console.WriteLine("E-mail of wachtwoord is onjuist");
            Console.WriteLine("Wil je het opnieuw proberen? (j/n)");
            string answer = Console.ReadLine();
 
            if (answer == "j")
            {
                Start();
            }
        }
    }
}