using System;
using Core.Models;

namespace Core.Validation
{
    public static class KupovinaValidator
    {
        public static bool IsValidISBN(string isbn)
        {
            return ISBNValidator.IsValid(isbn);
        }

        public static bool IsValidPurchaseDate(DateTime date)
        {
            return date != DateTime.MinValue
                && date.Date <= DateTime.Today;
        }

        public static bool IsValidRating(int rating)
        {
            return rating >= 1 && rating <= 5;
        }

        public static bool IsValidComment(string comment)
        {
            return comment != null
                && !comment.Contains('|')
                && !comment.Contains('\n')
                && !comment.Contains('\r');
        }

        public static bool IsValid(Kupovina? kupovina)
        {
            return kupovina != null
                && PosetilacValidator.IsValidMembershipCard(kupovina.BrojClanskeKarte)
                && IsValidISBN(kupovina.ISBN)
                && IsValidPurchaseDate(kupovina.DatumKupovine)
                && IsValidRating(kupovina.Ocena)
                && IsValidComment(kupovina.Komentar);
        }
    }
}