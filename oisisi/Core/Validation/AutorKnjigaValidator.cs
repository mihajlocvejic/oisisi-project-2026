using System;
using Core.Models;

namespace Core.Validation
{
    public static class AutorKnjigaValidator
    {
        public static bool IsValid(AutorKnjiga? veza)
        {
            return veza != null
                && CsvFieldValidator.IsValidRequired(veza.BrojLicneKarte)
                && CsvFieldValidator.IsValidRequired(veza.ISBN);
        }

        public static void Validate(AutorKnjiga? veza)
        {
            if (!IsValid(veza))
                throw new ArgumentException("Autor i ISBN su obavezni i ne smeju sadrzati | ili novi red.");
        }
    }
}
