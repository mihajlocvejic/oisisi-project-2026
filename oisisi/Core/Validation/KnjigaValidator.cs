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

            CsvFieldValidator.ValidateRequired(knjiga.Naziv, "Naziv knjige");
            CsvFieldValidator.ValidateRequired(knjiga.Zanr, "Zanr knjige");
            CsvFieldValidator.ValidateRequired(knjiga.SifraIzdavaca, "Sifra izdavaca");

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

            if (knjiga.KupciClanskeKarte == null
                || knjiga.KupciClanskeKarte.Any(value => !CsvFieldValidator.IsValidListItem(value)))
                throw new ArgumentException("Lista kupaca mora sadrzati neprazne brojeve kartica bez |, ; i novih redova.");

            if (knjiga.ListaZeljaClanskeKarte == null
                || knjiga.ListaZeljaClanskeKarte.Any(value => !CsvFieldValidator.IsValidListItem(value)))
                throw new ArgumentException("Lista zelja mora sadrzati neprazne brojeve kartica bez |, ; i novih redova.");
        }
    }
}
