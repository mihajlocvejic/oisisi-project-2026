using System;
using Core.Interfaces;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class Posetilac : ISerializable
    {
        public Adresa Adresa { get; set; } = new Adresa();

        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string KontaktTelefon { get; set; }
        public string Email { get; set; }
        public string BrojClanskeKarte { get; private set; }
        public int GodinaClanstva { get; set; }
        public StatusPosetioca Status { get; set; }
        public double ProsecnaOcena { get; private set; }
        public List<Kupovina> Kupovine { get; set; } = new List<Kupovina>();
        public List<string> ListaZeljaISBN { get; set; } = new List<string>();

        public Posetilac()
        {
            Ime = string.Empty;
            Prezime = string.Empty;
            KontaktTelefon = string.Empty;
            Email = string.Empty;
            BrojClanskeKarte = string.Empty;
        }

        public Posetilac(string ime, string prezime, DateTime datumRodjenja,
                         Adresa adresa, string kontaktTelefon, string email,
                         string brojClanskeKarte, int godinaClanstva,
                         StatusPosetioca status)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            Adresa = adresa;
            KontaktTelefon = kontaktTelefon;
            Email = email;
            BrojClanskeKarte = brojClanskeKarte;
            GodinaClanstva = godinaClanstva;
            Status = status;
            ProsecnaOcena = 0;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Ime,
                Prezime,
                DatumRodjenja.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                KontaktTelefon,
                Email,
                BrojClanskeKarte,
                GodinaClanstva.ToString(CultureInfo.InvariantCulture),
                Status.ToString(),
                ProsecnaOcena.ToString(CultureInfo.InvariantCulture),
                Adresa.Id.ToString(CultureInfo.InvariantCulture),
                string.Join(";", ListaZeljaISBN)
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 11)
                throw new FormatException("Neispravan format posetioca. Ocekivano 11 polja.");

            Ime = values[0];
            Prezime = values[1];
            DatumRodjenja = DateTime.ParseExact(
                values[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);

            KontaktTelefon = values[3];
            Email = values[4];
            BrojClanskeKarte = values[5];
            GodinaClanstva = int.Parse(values[6], CultureInfo.InvariantCulture);
            Status = Enum.Parse<StatusPosetioca>(values[7]);
            ProsecnaOcena = double.Parse(values[8], CultureInfo.InvariantCulture);

            Adresa = new Adresa
            {
                Id = int.Parse(values[9], CultureInfo.InvariantCulture)
            };

            ListaZeljaISBN = string.IsNullOrEmpty(values[10])
                ? new List<string>()
                : new List<string>(values[10].Split(';'));
        }

        public void IzracunajProsecnuOcenu()
        {
            if (Kupovine.Count == 0)
            {
                ProsecnaOcena = 0;
                return;
            }

            ProsecnaOcena = Kupovine.Average(k => k.Ocena);
        }

        public override string ToString()
        {
            return $"{BrojClanskeKarte} | {Ime} {Prezime} | " +
                   $"{DatumRodjenja:dd.MM.yyyy.} | {KontaktTelefon} | " +
                   $"{Email} | {Adresa} | {GodinaClanstva} | " +
                   $"{Status} | {ProsecnaOcena:F2} | " +
                   $"Broj kupovina: {Kupovine.Count} | " +
                   $"Lista zelja: {ListaZeljaISBN.Count}";
        }
    }
}