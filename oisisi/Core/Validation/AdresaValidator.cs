using System;
using Core.Models;

namespace Core.Validation
{
    public static class AdresaValidator
    {
        public static bool IsValid(Adresa? adresa)
        {
            return adresa != null
                && !string.IsNullOrWhiteSpace(adresa.Ulica)
                && !string.IsNullOrWhiteSpace(adresa.Broj)
                && !string.IsNullOrWhiteSpace(adresa.Grad)
                && !string.IsNullOrWhiteSpace(adresa.Drzava);
        }

        public static void Validate(Adresa? adresa)
        {
            if (!IsValid(adresa))
                throw new ArgumentException("Sva polja adrese su obavezna.");
        }
    }
}
