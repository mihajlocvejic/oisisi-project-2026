using System;
using Core.Models;

namespace Core.Validation
{
    public static class KnjigaValidator
    {
        public static void Validate(Knjiga knjiga)
        {
            if (knjiga == null)
                throw new ArgumentNullException(nameof(knjiga));

            if (!ISBNValidator.IsValid(knjiga.ISBN))
                throw new ArgumentException(
                    "ISBN mora biti ispravan ISBN-10 ili ISBN-13.");

            if (string.IsNullOrWhiteSpace(knjiga.Naziv))
                throw new ArgumentException(
                    "Naziv knjige je obavezan.");

            if (string.IsNullOrWhiteSpace(knjiga.Zanr))
                throw new ArgumentException(
                    "Zanr knjige je obavezan.");

            if (knjiga.GodinaIzdanja < 1450 ||
                knjiga.GodinaIzdanja > DateTime.Now.Year)
                throw new ArgumentException(
                    "Godina izdanja nije ispravna.");

            if (knjiga.Cena < 0)
                throw new ArgumentException(
                    "Cena knjige ne moze biti negativna.");

            if (knjiga.BrojStrana <= 0)
                throw new ArgumentException(
                    "Broj strana mora biti veci od nule.");

            if (string.IsNullOrWhiteSpace(knjiga.SifraIzdavaca))
                throw new ArgumentException(
                    "Sifra izdavaca je obavezna.");
        }
    }
}
