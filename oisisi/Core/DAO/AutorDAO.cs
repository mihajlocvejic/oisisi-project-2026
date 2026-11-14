
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Models;
using Core.Serialization;

namespace Core.DAO
{
    public class AutorDAO
    {
        private readonly List<Autor> _autori;
        private readonly Serializer<Autor> _serializer;
        private readonly string _filePath;

        public AutorDAO(string filePath)
        {
            _filePath = filePath;
            _serializer = new Serializer<Autor>();
            _autori = _serializer.FromCSV(_filePath);
        }

        public List<Autor> GetAll()
        {
            return new List<Autor>(_autori);
        }

        public Autor? GetById(string brojLicneKarte)
        {
            return _autori.FirstOrDefault(
                a => a.BrojLicneKarte == brojLicneKarte);
        }

        public void Add(Autor autor)
        {
            if (autor == null)
                throw new ArgumentNullException(nameof(autor));

            if (string.IsNullOrWhiteSpace(autor.BrojLicneKarte))
                throw new ArgumentException(
                    "Broj licne karte je obavezan.");

            if (GetById(autor.BrojLicneKarte) != null)
                throw new InvalidOperationException(
                    "Autor sa tim brojem licne karte vec postoji.");

            _autori.Add(autor);
            Save();
        }

        public void Update(Autor autor)
        {
            if (autor == null)
                throw new ArgumentNullException(nameof(autor));

            Autor? existing = GetById(autor.BrojLicneKarte);

            if (existing == null)
                throw new InvalidOperationException(
                    "Autor ne postoji.");

            int index = _autori.IndexOf(existing);
            _autori[index] = autor;

            Save();
        }

        public void Delete(string brojLicneKarte)
        {
            Autor? autor = GetById(brojLicneKarte);

            if (autor == null)
                throw new InvalidOperationException(
                    "Autor ne postoji.");

            _autori.Remove(autor);
            Save();
        }

        public void Save()
        {
            string? directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            _serializer.ToCSV(_filePath, _autori);
        }
    }
}
