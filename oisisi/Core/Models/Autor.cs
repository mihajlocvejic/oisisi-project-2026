
using System;
using System.Collections.Generic;
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
        public string BrojLicneKarte { get; set; }
        public int GodineIskustva { get; set; }

        public Autor()
        {
            Ime = string.Empty;
            Prezime = string.Empty;
            Adresa = new Adresa();
            Telefon = string.Empty;
            Email = string.Empty;
            BrojLicneKarte = string.Empty;
        }

        public Autor(string ime, string prezime,
                     DateTime datumRodjenja, Adresa adresa,
                     string telefon, string email,
                     string brojLicneKarte, int godineIskustva,
                     List<string> knjigeISBN)
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
                DatumRodjenja.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
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

            Ime = values[0];
            Prezime = values[1];

            DatumRodjenja = DateTime.ParseExact(
                values[2], "dd.MM.yyyy",
                CultureInfo.InvariantCulture);

            Adresa = new Adresa { Id = int.Parse(values[3], CultureInfo.InvariantCulture) };
            Telefon = values[4];
            Email = values[5];
            BrojLicneKarte = values[6];
            GodineIskustva = int.Parse(values[7], CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return $"{BrojLicneKarte} | {Ime} {Prezime} | " +
                   $"{DatumRodjenja:dd.MM.yyyy.} | {Adresa} | " +
                   $"{Telefon} | {Email} | {GodineIskustva}";
        }
    }
}
