static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.WriteLine("Welkom op de inlogpagina.");
        Console.WriteLine("Voer uw e-mailadres in.");
        string email = Console.ReadLine() ?? "";
        Console.WriteLine("Voer uw wachtwoord in.");
        string password = Console.ReadLine() ?? "";
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        if (acc != null)
        {
            Console.WriteLine($"Welkom terug, {acc.FullName}.");
            Console.WriteLine($"Uw e-mailadres is {acc.EmailAddress}.");
        }
        else
        {
            Console.WriteLine("Geen account gevonden met dit e-mailadres en wachtwoord.");
        }
    }
}