using System.Collections.Generic;
using Core.Interfaces;

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

        public Izdavac(string sifra, string naziv)
        {
            Sifra = sifra;
            Naziv = naziv;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Sifra,
                Naziv
            };
        }

        public void FromCSV(string[] values)
        {
            Sifra = values[0];
            Naziv = values[1];
        }
        public override string ToString()
        {
            return $"{Sifra} | {Naziv}";
        }

    }
}