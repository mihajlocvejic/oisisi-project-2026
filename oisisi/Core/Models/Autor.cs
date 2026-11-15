
using System;
using System.Globalization;
using Core.Interfaces;

namespace Core.Models
{
    public class Autor : ISerializable
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public Adresa Adresa { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        private string? _brojLicneKarte;
        public string BrojLicneKarte
        {
            get => _brojLicneKarte ?? string.Empty;
            set
            {
                if (_brojLicneKarte != null)
                    throw new InvalidOperationException("Broj licne karte se ne moze menjati nakon postavljanja.");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Broj licne karte je obavezan.");
                _brojLicneKarte = value;
            }
        }
        public int GodineIskustva { get; set; }

        public Autor()
        {
            Ime = string.Empty;
            Prezime = string.Empty;
            Adresa = new Adresa();
            Telefon = string.Empty;
            Email = string.Empty;
        }

        public Autor(string ime, string prezime,
                     DateTime datumRodjenja, Adresa adresa,
                     string telefon, string email,
                     string brojLicneKarte, int godineIskustva)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            Adresa = adresa;
            Telefon = telefon;
            Email = email;
            BrojLicneKarte = brojLicneKarte;
            GodineIskustva = godineIskustva;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Ime,
                Prezime,
                DatumRodjenja.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Adresa.Id.ToString(CultureInfo.InvariantCulture),
                Telefon,
                Email,
                BrojLicneKarte,
                GodineIskustva.ToString(CultureInfo.InvariantCulture)
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 8)
            {
                throw new FormatException(
                    "Neispravan format autora. Ocekivano 8 polja sa ID-em adrese.");
            }

            if (_brojLicneKarte != null)
                throw new InvalidOperationException("Postojeci autor se ne moze ponovo ucitavati.");

            DateTime datum = DateTime.ParseExact(values[2],
                new[] { "yyyy-MM-dd", "dd.MM.yyyy" },
                CultureInfo.InvariantCulture, DateTimeStyles.None);
            Adresa adresa = new Adresa { Id = int.Parse(values[3], CultureInfo.InvariantCulture) };
            int iskustvo = int.Parse(values[7], CultureInfo.InvariantCulture);
            BrojLicneKarte = values[6];
            Ime = values[0];
            Prezime = values[1];
            DatumRodjenja = datum;
            Adresa = adresa;
            Telefon = values[4];
            Email = values[5];
            GodineIskustva = iskustvo;
        }

        public override string ToString()
        {
            return $"{BrojLicneKarte} | {Ime} {Prezime} | " +
                   $"{DatumRodjenja:dd.MM.yyyy.} | {Adresa} | " +
                   $"{Telefon} | {Email} | {GodineIskustva}";
        }
    }
}
