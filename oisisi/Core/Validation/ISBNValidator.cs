using System.Text.RegularExpressions;

namespace Core.Validation
{
    public static class ISBNValidator
    {
        public static bool IsValid(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            isbn = isbn.Trim().Replace("-", "");

            return IsValidISBN10(isbn) || IsValidISBN13(isbn);
        }

        private static bool IsValidISBN10(string isbn)
        {
            if (!Regex.IsMatch(isbn, @"\A[0-9]{9}[0-9X]\z"))
                return false;

            int sum = 0;

            for (int i = 0; i < 10; i++)
            {
                int digit = isbn[i] == 'X' ? 10 : isbn[i] - '0';
                sum += digit * (10 - i);
            }

            return sum % 11 == 0;
        }

        private static bool IsValidISBN13(string isbn)
        {
            if (!Regex.IsMatch(isbn, @"\A97[89][0-9]{10}\z"))
                return false;

            int sum = 0;

            for (int i = 0; i < 13; i++)
            {
                int digit = isbn[i] - '0';
                sum += digit * (i % 2 == 0 ? 1 : 3);
            }

            return sum % 10 == 0;
        }
    }
}