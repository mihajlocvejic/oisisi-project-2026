using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.Models;
using Core.Validation;
using System.Linq;
using Core.Utils;

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

            List<Autor> autori = ReadAutori();
            Autor? sef = ReadSefIzdavaca(autori);

            Izdavac izdavac = new Izdavac(
                sifra, naziv, sef, autori
            );

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

            Izdavac? postojeci = _controller.GetById(sifra);

            if (postojeci == null)
            {
                Console.WriteLine("Izdavac nije pronadjen.");
                return;
            }

            Console.WriteLine($"Trenutni podaci: {postojeci}");

            string naziv = ReadRequiredString("Novi naziv izdavaca: ");

            List<Autor> autori = ReadAutori();
            Autor? sef = ReadSefIzdavaca(autori);

            Izdavac izmenjeni = new Izdavac(
                postojeci.Sifra, naziv, sef, autori
            );

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
            string sifra = ReadRequiredString(
                "Unesite sifru izdavaca za brisanje: ");

            if (_controller.GetById(sifra) == null)
            {
                Console.WriteLine("Izdavac nije pronadjen.");
                return;
            }

            if (_controller.Remove(sifra))
            {
                Console.WriteLine("Izdavac je uspesno obrisan.");
            }
            else
            {
                Console.WriteLine(
                    "Izdavac ima povezane knjige i ne moze biti obrisan.");
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

                Console.WriteLine("Autori:");

                foreach (Autor autor in izdavac.Autori)
                {
                    Console.WriteLine($"  - {autor.Ime} {autor.Prezime}");
                }

                Console.WriteLine("Knjige:");

                foreach (Knjiga knjiga in izdavac.Knjige)
                {
                    Console.WriteLine($"  - {knjiga.ISBN} | {knjiga.Naziv}");
                }

                Console.WriteLine();
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

                if (IzdavacValidator.IsValidText(value))
                    return value;

                Console.WriteLine("Polje ne sme biti prazno niti sadrzati znak |.");
            }
        }

        private List<Autor> ReadAutori()
        {
            List<Autor> dostupni = _controller.GetAllAutori();
            List<Autor> izabrani = new List<Autor>();

            if (dostupni.Count == 0)
            {
                Console.WriteLine("Nema dostupnih autora.");
                return izabrani;
            }

            Console.WriteLine("Dostupni autori:");

            foreach (Autor autor in dostupni)
            {
                Console.WriteLine(
                    $"{autor.BrojLicneKarte} | {autor.Ime} {autor.Prezime}");
            }

            while (true)
            {
                Console.Write("Broj licne karte autora (0 za kraj): ");
                string broj = Console.ReadLine()?.Trim() ?? "0";

                if (broj == "0")
                    return izabrani;

                Autor? autor = _controller.GetAutorById(broj);

                if (autor == null)
                {
                    Console.WriteLine("Autor ne postoji.");
                    continue;
                }

                if (izabrani.Any(a => EntityComparison.SameIdentifier(
                    a.BrojLicneKarte, autor.BrojLicneKarte)))
                {
                    Console.WriteLine("Autor je vec izabran.");
                    continue;
                }

                izabrani.Add(autor);
            }
        }

        private Autor? ReadSefIzdavaca(List<Autor> autori)
        {
            List<Autor> kandidati = autori
                .Where(a => a.GodineIskustva >= 5)
                .ToList();

            if (kandidati.Count == 0)
            {
                Console.WriteLine("Nema dostupnih kandidata za sefa izdavaca.");
                return null;
            }

            Console.WriteLine("Dostupni kandidati:");

            foreach (Autor autor in kandidati)
            {
                Console.WriteLine(
                    $"{autor.BrojLicneKarte} | {autor.Ime} {autor.Prezime} | " +
                    $"Godine iskustva: {autor.GodineIskustva}");
            }

            while (true)
            {
                Console.Write("Broj licne karte sefa (0 za preskakanje): ");
                string broj = Console.ReadLine()?.Trim() ?? "0";

                if (broj == "0")
                    return null;

                Autor? sef = kandidati.FirstOrDefault(a =>
                    EntityComparison.SameIdentifier(a.BrojLicneKarte, broj));

                if (sef != null)
                    return sef;

                Console.WriteLine("Neispravan izbor sefa izdavaca.");
            }
        }
    }
}