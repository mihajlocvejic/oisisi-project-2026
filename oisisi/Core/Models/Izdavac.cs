using System.Collections.Generic;
using Core.Interfaces;
using System.Linq;
using System;

namespace Core.Models
{
    public class Izdavac : ISerializable
    {
        public string Sifra { get; private set; }
        public string Naziv { get; set; }
        public Autor? SefIzdavaca { get; set; }
        public List<Autor> Autori { get; set; } = new List<Autor>();
        public List<Knjiga> Knjige { get; set; } = new List<Knjiga>();

        public Izdavac()
        {
            Sifra = string.Empty;
            Naziv = string.Empty;
        }

        public Izdavac(string sifra, string naziv,
                       Autor? sefIzdavaca, List<Autor> autori)
        {
            Sifra = sifra;
            Naziv = naziv;
            SefIzdavaca = sefIzdavaca;
            Autori = new List<Autor>(autori);
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Sifra,
                Naziv,
                SefIzdavaca?.BrojLicneKarte ?? string.Empty,
                string.Join(";", Autori.Select(a => a.BrojLicneKarte))
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 4)
                throw new FormatException(
                    "Neispravan format izdavaca. Ocekivano 4 polja.");

            Sifra = values[0];
            Naziv = values[1];

            SefIzdavaca = string.IsNullOrEmpty(values[2])
                ? null
                : new Autor { BrojLicneKarte = values[2] };

            Autori = string.IsNullOrEmpty(values[3])
                ? new List<Autor>()
                : values[3].Split(';')
                    .Select(broj => new Autor { BrojLicneKarte = broj })
                    .ToList();
        }

        public override string ToString()
        {
            string sef = SefIzdavaca == null
                ? "Nije postavljen"
                : $"{SefIzdavaca.Ime} {SefIzdavaca.Prezime}";

            return $"{Sifra} | {Naziv} | " +
                   $"Sef: {sef} | " +
                   $"Broj autora: {Autori.Count} | " +
                   $"Broj knjiga: {Knjige.Count}";
        }

    }
}