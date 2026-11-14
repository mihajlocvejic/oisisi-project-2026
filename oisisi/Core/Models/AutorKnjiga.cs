
using System;
using Core.Interfaces;

namespace Core.Models
{
    public class AutorKnjiga : ISerializable
    {
        public string BrojLicneKarte { get; set; }
        public string ISBN { get; set; }

        public AutorKnjiga()
        {
            BrojLicneKarte = string.Empty;
            ISBN = string.Empty;
        }

        public AutorKnjiga(string brojLicneKarte, string isbn)
        {
            BrojLicneKarte = brojLicneKarte;
            ISBN = isbn;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                BrojLicneKarte,
                ISBN
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 2)
                throw new FormatException(
                    "Neispravan format veze autor-knjiga.");

            BrojLicneKarte = values[0];
            ISBN = values[1];
        }

        public override string ToString()
        {
            return $"{BrojLicneKarte} | {ISBN}";
        }
    }
}
