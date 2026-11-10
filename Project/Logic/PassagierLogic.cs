using System.Globalization;

public class PassagierLogic
{
    private PassagierAccess _access = new();

    public void Add(PassagierModel passagier)
    {
        passagier.ID = _access.Add(passagier);
    }

    public bool IsValidNaam(string naam)
    {
        return IsValidName(naam);
    }

    public bool IsValidAchternaam(string achternaam)
    {
        return IsValidName(achternaam);
    }

    public bool IsValidEmail(string email)
    {
        int atIndex = email.IndexOf('@');
        return atIndex > 0 &&
            email.IndexOf('@', atIndex + 1) == -1 &&
            email.IndexOf('.', atIndex) > atIndex + 1 &&
            email.IndexOf('.', atIndex) < email.Length - 1 &&
            !email.Contains(' ');
    }

    public bool IsValidPaspoortNummer(string paspoortNummer)
    {
        return paspoortNummer.Length >= 5 &&
            paspoortNummer.Length <= 20 &&
            paspoortNummer.All(char.IsLetterOrDigit);
    }

    public bool IsValidGeboorteDatum(string geboorteDatum)
    {
        return DateTime.TryParseExact(geboorteDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datum) &&
            datum.Date < DateTime.Today;
    }

    public bool IsValidTelefoonnummer(string telefoonnummer)
    {
        int phoneDigits = telefoonnummer.Count(char.IsDigit);
        return phoneDigits >= 7 &&
            phoneDigits <= 15 &&
            telefoonnummer.All(character => char.IsDigit(character) || character == '+' ||
                character == ' ' || character == '(' || character == ')' || character == '-');
    }

    public bool IsValidPaspoortVerloopDatum(string paspoortVerloopDatum)
    {
        return DateTime.TryParseExact(paspoortVerloopDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datum) &&
            datum.Date > DateTime.Today;
    }

    private static bool IsValidName(string value)
    {
        return !string.IsNullOrWhiteSpace(value) &&
            char.IsLetter(value[0]) &&
            value.All(character => char.IsLetter(character) || character == ' ' || character == '-' || character == '\'');
    }
}
