
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
            Validate(autor);

            if (GetById(autor.BrojLicneKarte) != null)
                throw new InvalidOperationException(
                    "Autor sa tim brojem licne karte vec postoji.");

            _autori.Add(autor);
            Save();
        }

        public void Update(Autor autor)
        {
            Validate(autor);

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


        private void Validate(Autor autor)
        {
            if (autor == null)
                throw new ArgumentNullException(nameof(autor));

            if (string.IsNullOrWhiteSpace(autor.Ime))
                throw new ArgumentException("Ime autora je obavezno.");

            if (string.IsNullOrWhiteSpace(autor.Prezime))
                throw new ArgumentException("Prezime autora je obavezno.");

            if (autor.DatumRodjenja == default ||
                autor.DatumRodjenja.Date > DateTime.Today)
                throw new ArgumentException("Datum rodjenja nije ispravan.");

            if (autor.Adresa == null ||
                string.IsNullOrWhiteSpace(autor.Adresa.Ulica) ||
                string.IsNullOrWhiteSpace(autor.Adresa.Broj) ||
                string.IsNullOrWhiteSpace(autor.Adresa.Grad) ||
                string.IsNullOrWhiteSpace(autor.Adresa.Drzava))
                throw new ArgumentException("Sva polja adrese su obavezna.");

            if (string.IsNullOrWhiteSpace(autor.Telefon) ||
                !System.Text.RegularExpressions.Regex.IsMatch(
                    autor.Telefon, @"^\+?[\d\s()/\-]{6,20}$"))
                throw new ArgumentException("Broj telefona nije ispravan.");

            if (string.IsNullOrWhiteSpace(autor.Email) ||
                !MailAddress.TryCreate(autor.Email, out MailAddress? email) ||
                email.Address != autor.Email ||
                !email.Host.Contains('.'))
                throw new ArgumentException("Email adresa nije ispravna.");

            if (string.IsNullOrWhiteSpace(autor.BrojLicneKarte))
                throw new ArgumentException("Broj licne karte je obavezan.");

            if (autor.GodineIskustva < 0)
                throw new ArgumentException(
                    "Godine iskustva ne mogu biti negativne.");

            if (autor.GodineIskustva >
                DateTime.Today.Year - autor.DatumRodjenja.Year)
                throw new ArgumentException(
                    "Godine iskustva ne mogu biti vece od starosti autora.");
        }

    }
}
