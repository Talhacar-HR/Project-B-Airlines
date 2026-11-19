public class UserLogic
{
    // De gebruiker die nu is ingelogd. Is null als er niemand is ingelogd.
    // static, zodat je de ingelogde gebruiker overal in het programma kunt opvragen
    public static UserModel? CurrentUser { get; private set; }
 
    private UserAccess _access = new();
    private PassagierLogic _passagierLogic = new();

    // Zelfde e-mailcontrole als bij de passagiersgegevens
    public bool IsValidEmail(string email)
    {
        return email != null && _passagierLogic.IsValidEmail(email);
    }

    // Wachtwoord: minimaal 6 tekens en geen spaties
    public bool IsValidWachtwoord(string password)
    {
        return password != null && password.Length >= 6 && !password.Contains(' ');
    }

    // Is dit e-mailadres nog vrij?
    public bool IsEmailBeschikbaar(string email)
    {
        return _access.GetUserByEmail(email) == null;
    }

    public bool Register(string email, string password)
    {
        // Ongeldige gegevens worden niet opgeslagen
        if (!IsValidEmail(email) || !IsValidWachtwoord(password) || !IsEmailBeschikbaar(email))
        {
            return false;
        }

        UserModel newUser = new UserModel(0, email, password);
        return _access.Add(newUser);
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