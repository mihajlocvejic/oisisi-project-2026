using System;
using Core.Models;

namespace Core.Validation
{
    public static class AutorKnjigaValidator
    {
        public static bool IsValid(AutorKnjiga? veza)
        {
            return veza != null
                && !string.IsNullOrWhiteSpace(veza.BrojLicneKarte)
                && !string.IsNullOrWhiteSpace(veza.ISBN);
        }

        public static void Validate(AutorKnjiga? veza)
        {
            if (!IsValid(veza))
                throw new ArgumentException("Autor i ISBN su obavezni.");
        }
    }
}
