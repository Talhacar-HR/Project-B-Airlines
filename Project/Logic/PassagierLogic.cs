using System.Globalization;

public class PassagierLogic
{
    private PassagierAccess _access = new();

    public void Add(PassagierModel passagier)
    {
        ArgumentNullException.ThrowIfNull(passagier);

        if (string.IsNullOrWhiteSpace(passagier.Naam) ||
            string.IsNullOrWhiteSpace(passagier.Achternaam) ||
            string.IsNullOrWhiteSpace(passagier.Email) ||
            string.IsNullOrWhiteSpace(passagier.PaspoortNummer) ||
            string.IsNullOrWhiteSpace(passagier.GeboorteDatum) ||
            string.IsNullOrWhiteSpace(passagier.Telefoonnummer) ||
            string.IsNullOrWhiteSpace(passagier.PaspoortVerloopDatum))
        {
            throw new ArgumentException("Alle passagiersgegevens zijn verplicht.", nameof(passagier));
        }

        if (!IsValidName(passagier.Naam) || !IsValidName(passagier.Achternaam))
        {
            throw new ArgumentException("De naam bevat ongeldige tekens.", nameof(passagier));
        }

        int atIndex = passagier.Email.IndexOf('@');
        if (atIndex <= 0 ||
            passagier.Email.IndexOf('@', atIndex + 1) != -1 ||
            passagier.Email.IndexOf('.', atIndex) <= atIndex + 1 ||
            passagier.Email.IndexOf('.', atIndex) == passagier.Email.Length - 1 ||
            passagier.Email.Contains(' '))
        {
            throw new ArgumentException("Het e-mailadres is ongeldig.", nameof(passagier));
        }

        if (passagier.PaspoortNummer.Length < 5 ||
            passagier.PaspoortNummer.Length > 20 ||
            !passagier.PaspoortNummer.All(char.IsLetterOrDigit))
        {
            throw new ArgumentException("Het paspoortnummer moet 5-20 letters of cijfers bevatten.", nameof(passagier));
        }

        if (!DateTime.TryParseExact(passagier.GeboorteDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime geboorteDatum) ||
            geboorteDatum.Date >= DateTime.Today)
        {
            throw new ArgumentException("De geboortedatum moet een geldige datum in het verleden zijn (yyyy-MM-dd).", nameof(passagier));
        }

        if (!DateTime.TryParseExact(passagier.PaspoortVerloopDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime verloopDatum) ||
            verloopDatum.Date <= DateTime.Today)
        {
            throw new ArgumentException("De paspoortverloopdatum moet een geldige datum in de toekomst zijn (yyyy-MM-dd).", nameof(passagier));
        }

        int phoneDigits = passagier.Telefoonnummer.Count(char.IsDigit);
        if (phoneDigits < 7 ||
            phoneDigits > 15 ||
            !passagier.Telefoonnummer.All(character => char.IsDigit(character) || character == '+' ||
                character == ' ' || character == '(' || character == ')' || character == '-'))
        {
            throw new ArgumentException("Het telefoonnummer moet 7-15 cijfers bevatten.", nameof(passagier));
        }

        passagier.ID = _access.Add(passagier);
    }

    private static bool IsValidName(string value)
    {
        return char.IsLetter(value[0]) &&
            value.All(character => char.IsLetter(character) || character == ' ' || character == '-' || character == '\'');
    }
}
