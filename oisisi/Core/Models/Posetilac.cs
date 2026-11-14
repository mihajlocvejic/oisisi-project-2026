using System;
using Core.Interfaces;
using System.Globalization;

namespace Core.Models
{
    public class Posetilac : ISerializable
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string KontaktTelefon { get; set; }
        public string Email { get; set; }
        public string BrojClanskeKarte { get; private set; }
        public int GodinaClanstva { get; set; }
        public StatusPosetioca Status { get; set; }
        public double ProsecnaOcena { get; set; }

        public Posetilac()
        {
            Ime = string.Empty;
            Prezime = string.Empty;
            KontaktTelefon = string.Empty;
            Email = string.Empty;
            BrojClanskeKarte = string.Empty;
        }

        public Posetilac(string ime, string prezime, DateTime datumRodjenja,
                         string kontaktTelefon, string email, string brojClanskeKarte,
                         int godinaClanstva, StatusPosetioca status, double prosecnaOcena)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            KontaktTelefon = kontaktTelefon;
            Email = email;
            BrojClanskeKarte = brojClanskeKarte;
            GodinaClanstva = godinaClanstva;
            Status = status;
            ProsecnaOcena = prosecnaOcena;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Ime,
                Prezime,
                DatumRodjenja.ToString("yyyy-MM-dd"),
                KontaktTelefon,
                Email,
                BrojClanskeKarte,
                GodinaClanstva.ToString(),
                Status.ToString(),
                ProsecnaOcena.ToString(CultureInfo.InvariantCulture)
            };
        }

        public void FromCSV(string[] values)
        {
            Ime = values[0];
            Prezime = values[1];
            DatumRodjenja = DateTime.ParseExact(values[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            KontaktTelefon = values[3];
            Email = values[4];
            BrojClanskeKarte = values[5];
            GodinaClanstva = int.Parse(values[6]);
            Status = Enum.Parse<StatusPosetioca>(values[7]);
            ProsecnaOcena = double.Parse(values[8], CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return $"{BrojClanskeKarte} | {Ime} {Prezime} | " +
                   $"{DatumRodjenja:dd.MM.yyyy.} | {KontaktTelefon} | " +
                   $"{Email} | {GodinaClanstva} | {Status} | {ProsecnaOcena:F2}";
        }
    }
}