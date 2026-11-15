using Core.Models;

namespace Core.Validation
{
    public static class KnjigaValidator
    {
        public static bool IsValidText(string? value) => CsvFieldValidator.IsValidRequired(value);
        public static bool IsValidISBN(string? isbn) => isbn != null && ISBNValidator.IsValid(isbn);
        public static bool IsValidPublisherCode(string? code) => IsValidText(code);
        public static bool IsValidYear(int year) => year >= 1450 && year <= DateTime.Today.Year;
        public static bool IsValidPrice(decimal price) => price >= 0;
        public static bool IsValidPageCount(int pages) => pages > 0;
        public static bool IsValidCardList(List<string>? cards) => cards != null
            && cards.All(CsvFieldValidator.IsValidListItem);

        public static bool IsValid(Knjiga? knjiga) => knjiga != null
            && IsValidISBN(knjiga.ISBN) && IsValidText(knjiga.Naziv) && IsValidText(knjiga.Zanr)
            && IsValidPublisherCode(knjiga.SifraIzdavaca) && IsValidYear(knjiga.GodinaIzdanja)
            && IsValidPrice(knjiga.Cena) && IsValidPageCount(knjiga.BrojStrana);

        public static void Validate(Knjiga knjiga)
        {
            ArgumentNullException.ThrowIfNull(knjiga);
            if (!IsValidISBN(knjiga.ISBN))
                throw new ArgumentException("ISBN mora biti ispravan ISBN-10 ili ISBN-13.");
            CsvFieldValidator.ValidateRequired(knjiga.Naziv, "Naziv knjige");
            CsvFieldValidator.ValidateRequired(knjiga.Zanr, "Zanr knjige");
            CsvFieldValidator.ValidateRequired(knjiga.SifraIzdavaca, "Sifra izdavaca");
            if (!IsValidYear(knjiga.GodinaIzdanja))
                throw new ArgumentException("Godina izdanja nije ispravna.");
            if (!IsValidPrice(knjiga.Cena))
                throw new ArgumentException("Cena knjige ne moze biti negativna.");
            if (!IsValidPageCount(knjiga.BrojStrana))
                throw new ArgumentException("Broj strana mora biti veci od nule.");
            if (!IsValidCardList(knjiga.KupciClanskeKarte) || !IsValidCardList(knjiga.ListaZeljaClanskeKarte))
                throw new ArgumentException("Liste moraju sadrzati neprazne brojeve kartica bez |, ; i novih redova.");
        }
    }
}
