using System;
using System.Collections.Generic;
using System.Text;
using Core.Interfaces;

namespace Core.Models
{
    public class Adresa : ISerializable
    {
        public string Ulica { get; set; } = string.Empty;
        public string Broj { get; set; } = string.Empty;
        public string Grad { get; set; } = string.Empty;
        public string Drzava { get; set; } = string.Empty;

        public Adresa()
        {
        }

        public Adresa(string ulica, string broj,
                      string grad, string drzava)
        {
            Ulica = ulica;
            Broj = broj;
            Grad = grad;
            Drzava = drzava;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Ulica,
                Broj,
                Grad,
                Drzava
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 4)
            {
                throw new FormatException(
                    "Neispravan format adrese.");
            }

            Ulica = values[0];
            Broj = values[1];
            Grad = values[2];
            Drzava = values[3];
        }

        public override string ToString()
        {
            return $"{Ulica} {Broj}, {Grad}, {Drzava}";
        }
    }
}
