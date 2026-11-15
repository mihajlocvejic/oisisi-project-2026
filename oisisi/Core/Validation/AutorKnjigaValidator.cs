using Core.Models;

namespace Core.Validation
{
    public static class AutorKnjigaValidator
    {
        public static bool IsValidIdentityCard(string? value) => CsvFieldValidator.IsValidRequired(value);

        public static bool IsValidISBN(string? isbn) => CsvFieldValidator.IsValidRequired(isbn)
            && ISBNValidator.IsValid(isbn!);

        public static bool IsValid(AutorKnjiga? veza) => veza != null
            && IsValidIdentityCard(veza.BrojLicneKarte) && IsValidISBN(veza.ISBN);

        public static void Validate(AutorKnjiga? veza)
        {
            ArgumentNullException.ThrowIfNull(veza);
            CsvFieldValidator.ValidateRequired(veza.BrojLicneKarte, "Broj licne karte autora");
            CsvFieldValidator.ValidateRequired(veza.ISBN, "ISBN");
            if (!IsValidISBN(veza.ISBN))
                throw new ArgumentException("ISBN veze mora biti ispravan ISBN-10 ili ISBN-13.");
        }
    }
}
