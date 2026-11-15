using System.Net.Mail;
using System.Text.RegularExpressions;
using Core.Models;

namespace Core.Validation
{
    public static class AutorValidator
    {
        public static bool IsValidText(string? value) => CsvFieldValidator.IsValidRequired(value);
        public static bool IsValidIdentityCard(string? value) => IsValidText(value);

        public static bool IsValidPhone(string? phone) => IsValidText(phone)
            && Regex.IsMatch(phone!, @"^\+?[0-9]{6,15}$");

        public static bool IsValidEmail(string? email) => IsValidText(email)
            && MailAddress.TryCreate(email, out var address) && address.Address == email;

        public static bool IsValidBirthDate(DateTime date) =>
            date != DateTime.MinValue && date.Date <= DateTime.Today;

        public static int GetAge(DateTime birthDate)
        {
            if (!IsValidBirthDate(birthDate))
                throw new ArgumentException("Datum rodjenja nije ispravan.");
            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date.AddYears(age) > DateTime.Today) age--;
            return age;
        }

        public static bool IsValidExperience(int years, DateTime birthDate) =>
            IsValidBirthDate(birthDate) && years >= 0 && years <= GetAge(birthDate);

        public static bool IsValid(Autor? autor) => autor != null
            && IsValidText(autor.Ime) && IsValidText(autor.Prezime)
            && IsValidBirthDate(autor.DatumRodjenja)
            && AdresaValidator.IsValid(autor.Adresa)
            && IsValidPhone(autor.Telefon) && IsValidEmail(autor.Email)
            && IsValidIdentityCard(autor.BrojLicneKarte)
            && IsValidExperience(autor.GodineIskustva, autor.DatumRodjenja);

        public static void Validate(Autor autor)
        {
            ArgumentNullException.ThrowIfNull(autor);
            CsvFieldValidator.ValidateRequired(autor.Ime, "Ime autora");
            CsvFieldValidator.ValidateRequired(autor.Prezime, "Prezime autora");
            CsvFieldValidator.ValidateRequired(autor.Telefon, "Telefon autora");
            CsvFieldValidator.ValidateRequired(autor.Email, "Email autora");
            CsvFieldValidator.ValidateRequired(autor.BrojLicneKarte, "Broj licne karte");
            if (!IsValidBirthDate(autor.DatumRodjenja))
                throw new ArgumentException("Datum rodjenja nije ispravan.");
            AdresaValidator.Validate(autor.Adresa);
            if (!IsValidPhone(autor.Telefon))
                throw new ArgumentException("Telefon mora sadrzati 6-15 cifara i opcioni + na pocetku.");
            if (!IsValidEmail(autor.Email))
                throw new ArgumentException("Email adresa nije ispravna.");
            if (!IsValidExperience(autor.GodineIskustva, autor.DatumRodjenja))
                throw new ArgumentException("Godine iskustva moraju biti izmedju 0 i navrsene starosti autora.");
        }
    }
}
