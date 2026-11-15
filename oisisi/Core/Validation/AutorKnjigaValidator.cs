using Core.Models;

namespace Core.Validation
{
    public static class AutorKnjigaValidator
    {
        public static bool IsValid(AutorKnjiga? veza) => veza != null
            && CsvFieldValidator.IsValidRequired(veza.BrojLicneKarte)
            && CsvFieldValidator.IsValidRequired(veza.ISBN)
            && ISBNValidator.IsValid(veza.ISBN);

        public static void Validate(AutorKnjiga? veza)
        {
            ArgumentNullException.ThrowIfNull(veza);
            CsvFieldValidator.ValidateRequired(veza.BrojLicneKarte, "Broj licne karte autora");
            CsvFieldValidator.ValidateRequired(veza.ISBN, "ISBN");
            if (!ISBNValidator.IsValid(veza.ISBN))
                throw new ArgumentException("ISBN veze mora biti ispravan ISBN-10 ili ISBN-13.");
        }
    }
}
