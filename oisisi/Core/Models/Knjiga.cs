
using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Interfaces;

namespace Core.Models
{
    public class Knjiga : ISerializable
    {
        public string ISBN { get; set; }
        public string Naziv { get; set; }
        public string Zanr { get; set; }
        public int GodinaIzdanja { get; set; }
        public decimal Cena { get; set; }
        public int BrojStrana { get; set; }

        // Brojevi licnih karata autora knjige.
        public List<string> AutoriLicneKarte { get; set; }

        // Sifra izdavaca knjige.
        public string SifraIzdavaca { get; set; }

        // Brojevi clanskih karata posetilaca.
        public List<string> KupciClanskeKarte { get; set; }
        public List<string> ListaZeljaClanskeKarte { get; set; }

        public Knjiga()
        {
            ISBN = string.Empty;
            Naziv = string.Empty;
            Zanr = string.Empty;
            AutoriLicneKarte = new List<string>();
            SifraIzdavaca = string.Empty;
            KupciClanskeKarte = new List<string>();
            ListaZeljaClanskeKarte = new List<string>();
        }

        public Knjiga(
            string isbn,
            string naziv,
            string zanr,
            int godinaIzdanja,
            decimal cena,
            int brojStrana,
            List<string> autoriLicneKarte,
            string sifraIzdavaca,
            List<string> kupciClanskeKarte,
            List<string> listaZeljaClanskeKarte)
        {
            ISBN = isbn;
            Naziv = naziv;
            Zanr = zanr;
            GodinaIzdanja = godinaIzdanja;
            Cena = cena;
            BrojStrana = brojStrana;
            AutoriLicneKarte = autoriLicneKarte;
            SifraIzdavaca = sifraIzdavaca;
            KupciClanskeKarte = kupciClanskeKarte;
            ListaZeljaClanskeKarte = listaZeljaClanskeKarte;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                ISBN,
                Naziv,
                Zanr,
                GodinaIzdanja.ToString(CultureInfo.InvariantCulture),
                Cena.ToString(CultureInfo.InvariantCulture),
                BrojStrana.ToString(CultureInfo.InvariantCulture),
                string.Join(";", AutoriLicneKarte),
                SifraIzdavaca,
                string.Join(";", KupciClanskeKarte),
                string.Join(";", ListaZeljaClanskeKarte)
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 10)
            {
                throw new FormatException(
                    "Neispravan format podataka knjige.");
            }

            ISBN = values[0];
            Naziv = values[1];
            Zanr = values[2];

            GodinaIzdanja = int.Parse(
                values[3], CultureInfo.InvariantCulture);

            Cena = decimal.Parse(
                values[4], CultureInfo.InvariantCulture);

            BrojStrana = int.Parse(
                values[5], CultureInfo.InvariantCulture);

            AutoriLicneKarte = ParseList(values[6]);
            SifraIzdavaca = values[7];
            KupciClanskeKarte = ParseList(values[8]);
            ListaZeljaClanskeKarte = ParseList(values[9]);
        }

        private static List<string> ParseList(string value)
        {
            return string.IsNullOrEmpty(value)
                ? new List<string>()
                : new List<string>(value.Split(';'));
        }

        public override string ToString()
        {
            return $"{ISBN} | {Naziv} | {Zanr} | " +
                   $"{GodinaIzdanja} | {Cena:F2} | " +
                   $"{BrojStrana} strana | " +
                   $"Broj autora: {AutoriLicneKarte.Count}";
        }
    }
}
