using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Core.Models;

namespace Core.Validation
{
    public static class PosetilacValidator
    {
        public static bool IsValidText(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                && !value.Contains('|')
                && !value.Contains('\n')
                && !value.Contains('\r');
        }

        public static bool IsValidPhone(string phone)
        {
            return IsValidText(phone)
                && Regex.IsMatch(phone, @"^\+?[0-9]{6,15}$");
        }

        public static bool IsValidEmail(string email)
        {
            return IsValidText(email)
                && MailAddress.TryCreate(email, out var address)
                && address.Address == email
                && address.Host.Contains('.');
        }

        public static bool IsValidBirthDate(DateTime date)
        {
            return date != DateTime.MinValue
                && date.Date <= DateTime.Today;
        }

        public static bool IsValidMembershipCard(string card)
        {
            return IsValidText(card)
                && Regex.IsMatch(card, @"^SK-[1-9][0-9]*-[0-9]{4}$");
        }

        public static bool IsValidMembershipYear(int year)
        {
            return year > 0;
        }

        public static bool IsValidStatus(StatusPosetioca status)
        {
            return Enum.IsDefined(status);
        }

        public static bool IsValidAverageRating(double rating)
        {
            return rating >= 0 && rating <= 5;
        }

        public static bool IsValid(Posetilac? posetilac)
        {
            return posetilac != null
                && AdresaValidator.IsValid(posetilac.Adresa)
                && IsValidText(posetilac.Ime)
                && IsValidText(posetilac.Prezime)
                && IsValidBirthDate(posetilac.DatumRodjenja)
                && IsValidPhone(posetilac.KontaktTelefon)
                && IsValidEmail(posetilac.Email)
                && IsValidMembershipCard(posetilac.BrojClanskeKarte)
                && IsValidMembershipYear(posetilac.GodinaClanstva)
                && IsValidStatus(posetilac.Status)
                && IsValidAverageRating(posetilac.ProsecnaOcena);
        }
    }
}