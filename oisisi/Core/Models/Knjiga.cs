
using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Interfaces;

namespace Core.Models
{
    public class Knjiga : ISerializable
    {
        private string? _isbn;
        public string ISBN
        {
            get => _isbn ?? string.Empty;
            set
            {
                if (_isbn != null)
                    throw new InvalidOperationException("ISBN se ne moze menjati nakon postavljanja.");
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ISBN je obavezan.");
                _isbn = value;
            }
        }
        public string Naziv { get; set; }
        public string Zanr { get; set; }
        public int GodinaIzdanja { get; set; }
        public decimal Cena { get; set; }
        public int BrojStrana { get; set; }

        // Sifra izdavaca knjige.
        public string SifraIzdavaca { get; set; }

        // Brojevi clanskih karata posetilaca.
        public List<string> KupciClanskeKarte { get; set; }
        public List<string> ListaZeljaClanskeKarte { get; set; }

        public Knjiga()
        {
            Naziv = string.Empty;
            Zanr = string.Empty;
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
            SifraIzdavaca = sifraIzdavaca;
            KupciClanskeKarte = new List<string>(kupciClanskeKarte);
            ListaZeljaClanskeKarte = new List<string>(listaZeljaClanskeKarte);
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
                SifraIzdavaca,
                string.Join(";", KupciClanskeKarte),
                string.Join(";", ListaZeljaClanskeKarte)
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 9)
            {
                throw new FormatException(
                    "Neispravan format podataka knjige.");
            }

            if (_isbn != null)
                throw new InvalidOperationException("Postojeca knjiga se ne moze ponovo ucitavati.");

            int godina = int.Parse(values[3], CultureInfo.InvariantCulture);
            decimal cena = decimal.Parse(values[4], CultureInfo.InvariantCulture);
            int brojStrana = int.Parse(values[5], CultureInfo.InvariantCulture);
            List<string> kupci = ParseList(values[7]);
            List<string> zelje = ParseList(values[8]);
            ISBN = values[0];
            Naziv = values[1];
            Zanr = values[2];
            GodinaIzdanja = godina;
            Cena = cena;
            BrojStrana = brojStrana;
            SifraIzdavaca = values[6];
            KupciClanskeKarte = kupci;
            ListaZeljaClanskeKarte = zelje;
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
                   $"{BrojStrana} strana | Izdavac (sifra): {SifraIzdavaca}";
        }
    }
}
