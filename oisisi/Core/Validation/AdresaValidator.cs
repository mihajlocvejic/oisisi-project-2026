using System;
using Core.Models;

namespace Core.Validation
{
    public static class AdresaValidator
    {
        public static bool IsValidText(string value)
        {
            return CsvFieldValidator.IsValidRequired(value);
        }

        public static bool IsValid(Adresa? adresa)
        {
            return adresa != null
                && IsValidText(adresa.Ulica)
                && IsValidText(adresa.Broj)
                && IsValidText(adresa.Grad)
                && IsValidText(adresa.Drzava);
        }

        public static void Validate(Adresa? adresa)
        {
            if (!IsValid(adresa))
                throw new ArgumentException("Sva polja adrese su obavezna i ne smeju sadrzati | ili novi red.");
        }
    }
}
