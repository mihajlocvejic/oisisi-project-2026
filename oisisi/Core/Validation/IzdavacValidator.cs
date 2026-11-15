using Core.Models;

namespace Core.Validation
{
    public static class IzdavacValidator
    {
        public static bool IsValidText(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                && !value.Contains('|')
                && !value.Contains('\n')
                && !value.Contains('\r');
        }

        public static bool IsValid(Izdavac izdavac)
        {
            return IsValidText(izdavac.Sifra)
                && IsValidText(izdavac.Naziv);
        }
    }
}