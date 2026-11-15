namespace Core.Validation
{
    public static class CsvFieldValidator
    {
        public static bool IsValidRequired(string? value)
        {
            return !string.IsNullOrWhiteSpace(value)
                && !value.Contains('|')
                && !value.Contains('\r')
                && !value.Contains('\n');
        }

        public static void ValidateRequired(string? value, string fieldName)
        {
            if (!IsValidRequired(value))
                throw new ArgumentException($"Polje '{fieldName}' je obavezno i ne sme sadrzati | ili novi red.");
        }

        public static bool IsValidListItem(string? value)
        {
            return IsValidRequired(value) && !value!.Contains(';');
        }
    }
}
