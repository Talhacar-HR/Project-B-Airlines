static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static string Start()
    {
        Menu.PrintTitel();

        Console.WriteLine("Welkom op de inlogpagina.");
        Console.WriteLine("Voer uw e-mailadres in.");
        string email = Console.ReadLine() ?? "";
        Console.WriteLine("Voer uw wachtwoord in.");
        string password = Console.ReadLine() ?? "";
        
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        
        if (acc != null)
        {
            return $"Welkom terug, {acc.FullName}.\n" + $"Uw e-mailadres is {acc.EmailAddress}.";
        }
        else
        {
            return "Geen account gevonden met dit e-mailadres en wachtwoord.";
        }
    }
}