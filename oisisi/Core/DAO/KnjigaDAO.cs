
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Models;
using Core.Serialization;
using Core.Validation;

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

            if (!ISBNValidator.IsValid(knjiga.ISBN))
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

            if (string.IsNullOrWhiteSpace(knjiga.SifraIzdavaca))
                throw new ArgumentException(
                    "Sifra izdavaca je obavezna.");
        }

    }
}
