public class UserLogic
{
    // De gebruiker die nu is ingelogd. Is null als er niemand is ingelogd.
    // static, zodat je de ingelogde gebruiker overal in het programma kunt opvragen
    public static UserModel? CurrentUser { get; private set; }
 
    private UserAccess _access = new();
 
    public bool Register(string email, string password)
    {
        // E-mailadres mag niet leeg zijn
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }
 
        // E-mailadres moet een @ en een punt bevatten
        if (!email.Contains("@") || !email.Contains("."))
        {
            return false;
        }
 
        // Wachtwoord mag niet leeg zijn
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }
 
        // E-mailadres mag nog niet bestaan
        UserModel? existingUser = _access.GetUserByEmail(email);
        if (existingUser != null)
        {
            return false;
        }
 
        // Alles klopt, dus sla de nieuwe gebruiker op
        UserModel newUser = new UserModel(0, email, password);
        _access.AddUser(newUser);
        return true;
    }
 
    public bool Login(string email, string password)
    {
        UserModel? user = _access.GetUserByEmail(email);
 
        // Geen gebruiker gevonden met dit e-mailadres
        if (user == null)
        {
            return false;
        }
 
        // E-mailadres en wachtwoord moeten allebei kloppen
        if (user.Email == email && user.Password == password)
        {
            CurrentUser = user;
            return true;
        }
 
        return false;
    }
}