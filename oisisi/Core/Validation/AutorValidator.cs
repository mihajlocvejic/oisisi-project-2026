using System;
using Core.Models;
using System.Net.Mail;

namespace Core.Validation
{
    public static class AutorValidator
    {
        public static void Validate(Autor autor)
        {
            if (autor == null)
                throw new ArgumentNullException(nameof(autor));

            CsvFieldValidator.ValidateRequired(autor.Ime, "Ime autora");
            CsvFieldValidator.ValidateRequired(autor.Prezime, "Prezime autora");
            CsvFieldValidator.ValidateRequired(autor.Telefon, "Telefon autora");
            CsvFieldValidator.ValidateRequired(autor.Email, "Email autora");
            CsvFieldValidator.ValidateRequired(autor.BrojLicneKarte, "Broj licne karte");

            if (autor.DatumRodjenja == default ||
                autor.DatumRodjenja.Date > DateTime.Today)
                throw new ArgumentException("Datum rodjenja nije ispravan.");

            AdresaValidator.Validate(autor.Adresa);

            if (string.IsNullOrWhiteSpace(autor.Telefon) ||
                !System.Text.RegularExpressions.Regex.IsMatch(
                    autor.Telefon, @"^\+?[\d\s()/\-]{6,20}$"))
                throw new ArgumentException("Broj telefona nije ispravan.");

            if (string.IsNullOrWhiteSpace(autor.Email) ||
                !MailAddress.TryCreate(autor.Email, out MailAddress? email) ||
                email.Address != autor.Email ||
                !email.Host.Contains('.'))
                throw new ArgumentException("Email adresa nije ispravna.");

            if (autor.GodineIskustva < 0)
                throw new ArgumentException(
                    "Godine iskustva ne mogu biti negativne.");

            if (autor.GodineIskustva >
                DateTime.Today.Year - autor.DatumRodjenja.Year)
                throw new ArgumentException(
                    "Godine iskustva ne mogu biti vece od starosti autora.");
        }
    }
}
