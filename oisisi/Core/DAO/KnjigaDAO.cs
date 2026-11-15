
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
            if (string.IsNullOrWhiteSpace(isbn))
                return null;

            string isbnBezCrtica = isbn.Replace("-", "");

            return _knjige.FirstOrDefault(k =>
                k.ISBN.Replace("-", "") == isbnBezCrtica);
        }

        public void Add(Knjiga knjiga)
        {
            KnjigaValidator.Validate(knjiga);

            if (GetByISBN(knjiga.ISBN) != null)
                throw new InvalidOperationException(
                    "Knjiga sa tim ISBN brojem vec postoji.");

            _knjige.Add(knjiga);
            Save();
        }

        public void Update(Knjiga knjiga)
        {
            KnjigaValidator.Validate(knjiga);

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

    }
}
