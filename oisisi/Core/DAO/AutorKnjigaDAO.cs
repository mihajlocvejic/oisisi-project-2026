
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Models;
using Core.Serialization;

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
                .Where(v => v.ISBN == isbn)
                .Select(v => v.BrojLicneKarte)
                .ToList();
        }

        public List<string> GetKnjigeZaAutora(string brojLicneKarte)
        {
            return _veze
                .Where(v => v.BrojLicneKarte == brojLicneKarte)
                .Select(v => v.ISBN)
                .ToList();
        }

        public bool Exists(string brojLicneKarte, string isbn)
        {
            return _veze.Any(v =>
                v.BrojLicneKarte == brojLicneKarte &&
                v.ISBN == isbn);
        }

        public void Add(string brojLicneKarte, string isbn)
        {
            if (string.IsNullOrWhiteSpace(brojLicneKarte) ||
                string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException(
                    "Autor i ISBN su obavezni.");

            if (Exists(brojLicneKarte, isbn))
                throw new InvalidOperationException(
                    "Veza vec postoji.");

            _veze.Add(new AutorKnjiga(brojLicneKarte, isbn));
            Save();
        }

        public void Delete(string brojLicneKarte, string isbn)
        {
            AutorKnjiga? veza = _veze.FirstOrDefault(v =>
                v.BrojLicneKarte == brojLicneKarte &&
                v.ISBN == isbn);

            if (veza == null)
                throw new InvalidOperationException(
                    "Veza ne postoji.");

            _veze.Remove(veza);
            Save();
        }

        private void Save()
        {
            string? directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            _serializer.ToCSV(_filePath, _veze);
        }
    }
}
