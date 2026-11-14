using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.Models;

namespace ConsoleClient.Views
{
    public class IzdavacConsoleView
    {
        private readonly IzdavacController _controller;

        public IzdavacConsoleView(IzdavacController controller)
        {
            _controller = controller;
        }

        public void AddIzdavac()
        {
            string sifra = ReadRequiredString("Sifra izdavaca: ");
            string naziv = ReadRequiredString("Naziv izdavaca: ");

            Izdavac izdavac = new Izdavac(sifra, naziv);

            if (_controller.Add(izdavac))
            {
                Console.WriteLine("Izdavac je uspesno dodat.");
            }
            else
            {
                Console.WriteLine("Podaci nisu validni ili sifra vec postoji.");
            }
        }

        public void UpdateIzdavac()
        {
            string sifra = ReadRequiredString("Unesite sifru izdavaca za izmenu: ");

            Izdavac? postojeci = _controller.GetAll()
                .Find(i => i.Sifra == sifra);

            if (postojeci == null)
            {
                Console.WriteLine("Izdavac nije pronadjen.");
                return;
            }

            Console.WriteLine($"Trenutni podaci: {postojeci}");

            string naziv = ReadRequiredString("Novi naziv izdavaca: ");

            Izdavac izmenjeni = new Izdavac(sifra, naziv);

            if (_controller.Update(sifra, izmenjeni))
            {
                Console.WriteLine("Izdavac je uspesno izmenjen.");
            }
            else
            {
                Console.WriteLine("Izmena izdavaca nije uspela.");
            }
        }

        public void RemoveIzdavac()
        {
            string sifra = ReadRequiredString("Unesite sifru izdavaca za brisanje: ");

            if (_controller.Remove(sifra))
            {
                Console.WriteLine("Izdavac je uspesno obrisan.");
            }
            else
            {
                Console.WriteLine("Izdavac nije pronadjen.");
            }
        }

        public void ShowIzdavaci()
        {
            List<Izdavac> izdavaci = _controller.GetAll();

            if (izdavaci.Count == 0)
            {
                Console.WriteLine("Nema registrovanih izdavaca.");
                return;
            }

            Console.WriteLine("Registrovani izdavaci:");

            foreach (Izdavac izdavac in izdavaci)
            {
                Console.WriteLine(izdavac);
            }
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Upravljanje izdavacima ---");
                Console.WriteLine("1. Dodavanje izdavaca");
                Console.WriteLine("2. Izmena izdavaca");
                Console.WriteLine("3. Brisanje izdavaca");
                Console.WriteLine("4. Prikaz izdavaca");
                Console.WriteLine("0. Izlaz");
                Console.Write("Izaberite opciju: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddIzdavac();
                        break;
                    case "2":
                        UpdateIzdavac();
                        break;
                    case "3":
                        RemoveIzdavac();
                        break;
                    case "4":
                        ShowIzdavaci();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Neispravna opcija.");
                        break;
                }
            }
        }

        private string ReadRequiredString(string message)
        {
            while (true)
            {
                Console.Write(message);
                string value = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine("Polje ne sme biti prazno.");
                    continue;
                }

                if (value.Contains('|'))
                {
                    Console.WriteLine("Znak | nije dozvoljen.");
                    continue;
                }

                return value;
            }
        }
    }
}