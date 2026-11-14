
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Models;
using Core.Serialization;
using System.Text.RegularExpressions;

namespace Core.DAO
{
    public class KnjigaDAO
    {
        private readonly List<Knjiga> _knjige;
        private readonly Serializer<Knjiga> _serializer;
        private readonly string _filePath;

        public KnjigaDAO(string filePath)
        {
            _filePath = filePath;
            _serializer = new Serializer<Knjiga>();
            _knjige = _serializer.FromCSV(_filePath);
        }

        public List<Knjiga> GetAll()
        {
            return new List<Knjiga>(_knjige);
        }

        public Knjiga? GetByISBN(string isbn)
        {
            return _knjige.FirstOrDefault(k => k.ISBN == isbn);
        }

        public void Add(Knjiga knjiga)
        {
            Validate(knjiga);

            if (GetByISBN(knjiga.ISBN) != null)
                throw new InvalidOperationException(
                    "Knjiga sa tim ISBN brojem vec postoji.");

            _knjige.Add(knjiga);
            Save();
        }

        public void Update(Knjiga knjiga)
        {
            Validate(knjiga);

            Knjiga? existing = GetByISBN(knjiga.ISBN);

            if (existing == null)
                throw new InvalidOperationException(
                    "Knjiga ne postoji.");

            int index = _knjige.IndexOf(existing);
            _knjige[index] = knjiga;

            Save();
        }

        public void Delete(string isbn)
        {
            Knjiga? knjiga = GetByISBN(isbn);

            if (knjiga == null)
                throw new InvalidOperationException(
                    "Knjiga ne postoji.");

            _knjige.Remove(knjiga);
            Save();
        }

        public void Save()
        {
            string? directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            _serializer.ToCSV(_filePath, _knjige);
        }

        private void Validate(Knjiga knjiga)
        {
            if (knjiga == null)
                throw new ArgumentNullException(nameof(knjiga));

            if (!IsValidISBN(knjiga.ISBN))
                throw new ArgumentException(
                    "ISBN mora biti ispravan ISBN-10 ili ISBN-13.");

            if (string.IsNullOrWhiteSpace(knjiga.Naziv))
                throw new ArgumentException(
                    "Naziv knjige je obavezan.");

            if (string.IsNullOrWhiteSpace(knjiga.Zanr))
                throw new ArgumentException(
                    "Zanr knjige je obavezan.");

            if (knjiga.GodinaIzdanja < 1450 ||
                knjiga.GodinaIzdanja > DateTime.Now.Year)
                throw new ArgumentException(
                    "Godina izdanja nije ispravna.");

            if (knjiga.Cena < 0)
                throw new ArgumentException(
                    "Cena knjige ne moze biti negativna.");

            if (knjiga.BrojStrana <= 0)
                throw new ArgumentException(
                    "Broj strana mora biti veci od nule.");

            if (knjiga.AutoriLicneKarte == null ||
                knjiga.AutoriLicneKarte.Count == 0 ||
                knjiga.AutoriLicneKarte.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException(
                    "Knjiga mora imati najmanje jednog autora.");

            if (string.IsNullOrWhiteSpace(knjiga.SifraIzdavaca))
                throw new ArgumentException(
                    "Sifra izdavaca je obavezna.");
        }

        private bool IsValidISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            string value = isbn.Replace("-", "").Replace(" ", "");

            if (Regex.IsMatch(value, @"^\d{13}$"))
            {
                int sum = 0;

                for (int i = 0; i < 13; i++)
                {
                    int digit = value[i] - '0';
                    sum += digit * (i % 2 == 0 ? 1 : 3);
                }

                return sum % 10 == 0;
            }

            if (Regex.IsMatch(value, @"^\d{9}[\dXx]$"))
            {
                int sum = 0;

                for (int i = 0; i < 10; i++)
                {
                    int digit = i == 9 &&
                                (value[i] == 'X' || value[i] == 'x')
                        ? 10
                        : value[i] - '0';

                    sum += digit * (10 - i);
                }

                return sum % 11 == 0;
            }

            return false;
        }

    }
}
