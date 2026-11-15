
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
    public class AutorDAO
    {
        private readonly AdresaDAO _adresaDAO;
        private readonly List<Autor> _autori;
        private readonly Serializer<Autor> _serializer;
        private readonly string _filePath;

        public AutorDAO(string filePath, AdresaDAO? adresaDAO = null)
        {
            _filePath = filePath;
            _serializer = new Serializer<Autor>();
            _autori = _serializer.FromCSV(_filePath);
            _adresaDAO = adresaDAO ?? new AdresaDAO(Path.Combine(Path.GetDirectoryName(Path.GetFullPath(filePath))!, "adrese.csv"));
            foreach (Autor item in _autori)
                item.Adresa = _adresaDAO.GetById(item.Adresa.Id)
                    ?? throw new FormatException("Povezana adresa ne postoji.");
        }

        public List<Autor> GetAll()
        {
            return new List<Autor>(_autori);
        }

        public Autor? GetById(string brojLicneKarte)
        {
            return _autori.FirstOrDefault(
                a => EntityComparison.SameIdentifier(a.BrojLicneKarte, brojLicneKarte));
        }

        public void Add(Autor autor)
        {
            AutorValidator.Validate(autor);
            autor.Adresa = _adresaDAO.GetById(autor.Adresa.Id)
                ?? throw new ArgumentException("Izaberite postojecu adresu.");

            if (GetById(autor.BrojLicneKarte) != null)
                throw new InvalidOperationException(
                    "Autor sa tim brojem licne karte vec postoji.");

            _autori.Add(autor);
            Save();
        }

        public void Update(Autor autor)
        {
            AutorValidator.Validate(autor);
            autor.Adresa = _adresaDAO.GetById(autor.Adresa.Id)
                ?? throw new ArgumentException("Izaberite postojecu adresu.");

            Autor? existing = GetById(autor.BrojLicneKarte);

            if (existing == null)
                throw new InvalidOperationException(
                    "Autor ne postoji.");

            autor.BrojLicneKarte = existing.BrojLicneKarte;
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
