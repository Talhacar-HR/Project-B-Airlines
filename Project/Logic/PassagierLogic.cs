using System.Globalization;

public class PassagierLogic
{
    private PassagierAccess _access = new();

    // Slaat de passagier alleen op als alle gegevens geldig zijn.
    // Geeft true terug als de passagier is opgeslagen.
    public bool Add(PassagierModel passagier)
    {
        if (!IsValidPassagier(passagier))
            return false;

        passagier.ID = _access.Add(passagier);
        return passagier.ID > 0;
    }

    // Zijn alle verplichte gegevens van de passagier geldig?
    public bool IsValidPassagier(PassagierModel passagier)
    {
        if (passagier == null || passagier.Email == null || passagier.PaspoortNummer == null || passagier.Telefoonnummer == null)
            return false;

        return IsValidNaam(passagier.Naam) &&
            IsValidAchternaam(passagier.Achternaam) &&
            IsValidEmail(passagier.Email) &&
            IsValidPaspoortNummer(passagier.PaspoortNummer) &&
            IsValidGeboorteDatum(passagier.GeboorteDatum) &&
            IsValidTelefoonnummer(passagier.Telefoonnummer) &&
            IsValidPaspoortVerloopDatum(passagier.PaspoortVerloopDatum);
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
