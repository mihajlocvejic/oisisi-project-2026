using System;
using Core.Interfaces;
using System.Globalization;

namespace Core.Models
{
    public class Kupovina : ISerializable
    {
        public int Id { get; private set; }
        public DateTime DatumKupovine { get; set; }
        public int Ocena { get; set; }
        public string Komentar { get; set; }

        public string BrojClanskeKarte { get; set; }
        public string ISBN { get; set; }

        public Kupovina()
        {
            BrojClanskeKarte = string.Empty;
            ISBN = string.Empty;
            Komentar = string.Empty;
        }

        public Kupovina(int id, string brojClanskeKarte, string isbn,
                        DateTime datumKupovine, int ocena, string komentar)
        {
            Id = id;
            BrojClanskeKarte = brojClanskeKarte;
            ISBN = isbn;
            DatumKupovine = datumKupovine;
            Ocena = ocena;
            Komentar = komentar;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(CultureInfo.InvariantCulture),
                BrojClanskeKarte,
                ISBN,
                DatumKupovine.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Ocena.ToString(CultureInfo.InvariantCulture),
                Komentar
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 6)
                throw new FormatException("Neispravan format kupovine. Ocekivano 6 polja.");

            Id = int.Parse(values[0], CultureInfo.InvariantCulture);
            BrojClanskeKarte = values[1];
            ISBN = values[2];
            DatumKupovine = DateTime.ParseExact(
                values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Ocena = int.Parse(values[4], CultureInfo.InvariantCulture);
            Komentar = values[5];
        }

        public override string ToString()
        {
            return $"{Id} | {BrojClanskeKarte} | {ISBN} | " +
                   $"{DatumKupovine:dd.MM.yyyy.} | {Ocena}/5 | {Komentar}";
        }
    }
}