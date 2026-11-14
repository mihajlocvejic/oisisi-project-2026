using Core.Controllers;
using Core.Models;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace ConsoleClient.Views
{
    public class PosetilacConsoleView
    {
        private readonly PosetilacController _controller;

        public PosetilacConsoleView(PosetilacController controller)
        {
            _controller = controller;
        }

        public void AddPosetilac()
        {
            Console.Write("Ime: ");
            string ime = Console.ReadLine() ?? string.Empty;

            Console.Write("Prezime: ");
            string prezime = Console.ReadLine() ?? string.Empty;

            Console.Write("Datum rodjenja (dd.MM.yyyy): ");
            DateTime datumRodjenja = DateTime.ParseExact(Console.ReadLine() ?? "", "dd.MM.yyyy", CultureInfo.InvariantCulture);

            Console.Write("Kontakt telefon: ");
            string kontaktTelefon = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            Console.Write("Broj clanske karte: ");
            string brojClanskeKarte = Console.ReadLine() ?? string.Empty;

            Console.Write("Godina clanstva: ");
            int godinaClanstva = int.Parse(Console.ReadLine() ?? "");

            Console.Write("Status (R/V): ");
            StatusPosetioca status = Enum.Parse<StatusPosetioca>(Console.ReadLine() ?? "", true);

            Posetilac posetilac = new Posetilac(
                ime, prezime, datumRodjenja,
                kontaktTelefon, email, brojClanskeKarte,
                godinaClanstva, status, 0
            );

            if (_controller.Add(posetilac))
            {
                Console.WriteLine("Posetilac je uspesno dodat.");
            }
            else
            {
                Console.WriteLine("Broj clanske karte vec postoji.");
            }
        }

        public void UpdatePosetilac()
        {
            Console.Write("Unesite broj clanske karte posetioca za izmenu: ");
            string brojClanskeKarte = Console.ReadLine() ?? string.Empty;

            Posetilac? postojeci = _controller.GetAll().Find(p => p.BrojClanskeKarte == brojClanskeKarte);

            if (postojeci == null)
            {
                Console.WriteLine("Posetilac nije pronadjen.");
                return;
            }

            Console.WriteLine($"Trenutni podaci: {postojeci}");

            Console.Write("Novo ime: ");
            string ime = Console.ReadLine() ?? string.Empty;

            Console.Write("Novo prezime: ");
            string prezime = Console.ReadLine() ?? string.Empty;

            Console.Write("Novi datum rodjenja (dd.MM.yyyy): ");
            DateTime datumRodjenja = DateTime.ParseExact(
                Console.ReadLine() ?? "", "dd.MM.yyyy",
                CultureInfo.InvariantCulture);

            Console.Write("Novi kontakt telefon: ");
            string kontaktTelefon = Console.ReadLine() ?? string.Empty;

            Console.Write("Novi email: ");
            string email = Console.ReadLine() ?? string.Empty;

            Console.Write("Novi broj clanske karte: ");
            string noviBrojClanskeKarte = Console.ReadLine() ?? string.Empty;

            Console.Write("Nova godina clanstva: ");
            int godinaClanstva = int.Parse(Console.ReadLine() ?? "");

            Console.Write("Novi status (R/V): ");
            StatusPosetioca status = Enum.Parse<StatusPosetioca>(
                Console.ReadLine() ?? "", true);

            Posetilac izmenjeni = new Posetilac(
                ime, prezime, datumRodjenja,
                kontaktTelefon, email, noviBrojClanskeKarte,
                godinaClanstva, status, postojeci.ProsecnaOcena
            );

            if (_controller.Update(brojClanskeKarte, izmenjeni))
            {
                Console.WriteLine("Posetilac je uspesno izmenjen.");
            }
            else
            {
                Console.WriteLine("Izmena nije uspela. Broj clanske karte vec postoji ili posetilac nije pronadjen.");
            }
        }

        public void RemovePosetilac()
        {
            Console.Write("Unesite broj clanske karte za brisanje: ");
            string brojClanskeKarte = Console.ReadLine() ?? string.Empty;

            if (_controller.Remove(brojClanskeKarte))
            {
                Console.WriteLine("Posetilac je uspesno obrisan.");
            }
            else
            {
                Console.WriteLine("Posetilac nije pronadjen.");
            }
        }

        public void ShowPosetioci()
        {
            List<Posetilac> posetioci = _controller.GetAll();

            if (posetioci.Count == 0)
            {
                Console.WriteLine("Nema registrovanih posetilaca.");
                return;
            }

            Console.WriteLine("Registrovani posetioci:");

            foreach (Posetilac posetilac in posetioci)
            {
                Console.WriteLine(posetilac);
            }
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Upravljanje posetiocima ---");
                Console.WriteLine("1. Dodavanje posetioca");
                Console.WriteLine("2. Izmena posetioca");
                Console.WriteLine("3. Brisanje posetioca");
                Console.WriteLine("4. Prikaz posetilaca");
                Console.WriteLine("0. Izlaz");
                Console.Write("Izaberite opciju: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddPosetilac();
                        break;
                    case "2":
                        UpdatePosetilac();
                        break;
                    case "3":
                        RemovePosetilac();
                        break;
                    case "4":
                        ShowPosetioci();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Neispravna opcija.");
                        break;
                }
            }
        }
    }
}