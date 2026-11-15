
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Models;
using Core.Utils;
using Core.Serialization;
using Core.Validation;

namespace Core.DAO
{
    public class AutorKnjigaDAO
    {
        private readonly List<AutorKnjiga> _veze;
        private readonly Serializer<AutorKnjiga> _serializer;
        private readonly string _filePath;

        public AutorKnjigaDAO(string filePath)
        {
            _filePath = filePath;
            _serializer = new Serializer<AutorKnjiga>();
            _veze = _serializer.FromCSV(_filePath);
        }

        public List<AutorKnjiga> GetAll()
        {
            return new List<AutorKnjiga>(_veze);
        }

        public List<string> GetAutoriZaKnjigu(string isbn)
        {
            return _veze
                .Where(v => EntityComparison.SameISBN(v.ISBN, isbn))
                .Select(v => v.BrojLicneKarte)
                .ToList();
        }

        public List<string> GetKnjigeZaAutora(string brojLicneKarte)
        {
            return _veze
                .Where(v => EntityComparison.SameIdentifier(v.BrojLicneKarte, brojLicneKarte))
                .Select(v => v.ISBN)
                .ToList();
        }

        public bool Exists(string brojLicneKarte, string isbn)
        {
            return _veze.Any(v =>
                EntityComparison.SameIdentifier(v.BrojLicneKarte, brojLicneKarte) &&
                EntityComparison.SameISBN(v.ISBN, isbn));
        }

        public void Add(string brojLicneKarte, string isbn)
        {
            AutorKnjiga veza = new AutorKnjiga(brojLicneKarte, isbn);
            AutorKnjigaValidator.Validate(veza);

            if (Exists(brojLicneKarte, isbn))
                throw new InvalidOperationException(
                    "Veza vec postoji.");

            List<AutorKnjiga> updated = new(_veze) { veza };
            Save(updated);
            _veze.Add(veza);
        }

        public void Remove(string brojLicneKarte, string isbn)
        {
            AutorKnjiga? veza = _veze.FirstOrDefault(v =>
                EntityComparison.SameIdentifier(v.BrojLicneKarte, brojLicneKarte) &&
                EntityComparison.SameISBN(v.ISBN, isbn));

            if (veza == null)
                throw new InvalidOperationException(
                    "Veza ne postoji.");

            List<AutorKnjiga> updated = new(_veze);
            updated.Remove(veza);
            Save(updated);
            _veze.Remove(veza);
        }

        private void Save(List<AutorKnjiga> veze)
        {
            string? directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            _serializer.ToCSV(_filePath, veze);
        }
    }
}
