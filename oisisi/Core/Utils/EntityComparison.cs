using System.Text.RegularExpressions;
using Core.Models;

namespace Core.Utils
{
    public static class EntityComparison
    {
        public static string IdentifierKey(string value) => value.Trim().ToUpperInvariant();

        public static bool SameIdentifier(string left, string right) =>
            string.Equals(left.Trim(), right.Trim(), StringComparison.OrdinalIgnoreCase);

        public static bool SameISBN(string left, string right) =>
            SameIdentifier(left.Replace("-", ""), right.Replace("-", ""));

        private static string AddressPart(string value) => Regex.Replace(value.Trim(), @"\s+", " ");

        public static bool SameAddress(Adresa left, Adresa right) =>
            SameIdentifier(AddressPart(left.Ulica), AddressPart(right.Ulica))
            && SameIdentifier(AddressPart(left.Broj), AddressPart(right.Broj))
            && SameIdentifier(AddressPart(left.Grad), AddressPart(right.Grad))
            && SameIdentifier(AddressPart(left.Drzava), AddressPart(right.Drzava));
    }
}
