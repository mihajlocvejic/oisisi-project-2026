
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

        // ISBN brojevi knjiga koje je autor napisao.
        public List<string> KnjigeISBN { get; set; }

        public Autor()
        {
            Ime = string.Empty;
            Prezime = string.Empty;
            Adresa = new Adresa();
            Telefon = string.Empty;
            Email = string.Empty;
            BrojLicneKarte = string.Empty;
            KnjigeISBN = new List<string>();
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
            KnjigeISBN = knjigeISBN;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Ime,
                Prezime,
                DatumRodjenja.ToString("yyyy-MM-dd"),
                Adresa.Ulica,
                Adresa.Broj,
                Adresa.Grad,
                Adresa.Drzava,
                Telefon,
                Email,
                BrojLicneKarte,
                GodineIskustva.ToString(CultureInfo.InvariantCulture),
                string.Join(";", KnjigeISBN)
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 12)
            {
                throw new FormatException(
                    "Neispravan format podataka autora.");
            }

            Ime = values[0];
            Prezime = values[1];

            DatumRodjenja = DateTime.ParseExact(
                values[2], "yyyy-MM-dd",
                CultureInfo.InvariantCulture);

            Adresa = new Adresa(
                values[3],
                values[4],
                values[5],
                values[6]
            );

            Telefon = values[7];
            Email = values[8];
            BrojLicneKarte = values[9];

            GodineIskustva = int.Parse(
                values[10], CultureInfo.InvariantCulture);

            KnjigeISBN = string.IsNullOrEmpty(values[11])
                ? new List<string>()
                : new List<string>(
                    values[11].Split(';'));
        }

        public override string ToString()
        {
            return $"{BrojLicneKarte} | {Ime} {Prezime} | " +
                   $"{DatumRodjenja:dd.MM.yyyy.} | {Adresa} | " +
                   $"{Telefon} | {Email} | {GodineIskustva} | " +
                   $"Broj knjiga: {KnjigeISBN.Count}";
        }
    }
}
