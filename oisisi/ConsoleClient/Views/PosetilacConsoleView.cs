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
            string ime = ReadRequiredString("Ime: ");
            string prezime = ReadRequiredString("Prezime: ");
            DateTime datumRodjenja = ReadDate("Datum rodjenja (dd.MM.yyyy): ");
            string kontaktTelefon = ReadPhone("Kontakt telefon: ");
            string email = ReadEmail("Email: ");
            string brojClanskeKarte = ReadRequiredString("Broj clanske karte: ");
            int godinaClanstva = ReadMembershipYear("Godina clanstva: ");
            StatusPosetioca status = ReadStatus("Status (R/V): ");

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

            string ime = ReadRequiredString("Novo ime: ");
            string prezime = ReadRequiredString("Novo prezime: ");
            DateTime datumRodjenja = ReadDate("Novi datum rodjenja (dd.MM.yyyy): ");
            string kontaktTelefon = ReadPhone("Novi kontakt telefon: ");
            string email = ReadEmail("Novi email: ");
            string noviBrojClanskeKarte = postojeci.BrojClanskeKarte;
            int godinaClanstva = ReadMembershipYear("Nova godina clanstva: ");
            StatusPosetioca status = ReadStatus("Novi status (R/V): ");

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
                Console.WriteLine("Izmena posetioca nije uspela.");
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

        private DateTime ReadDate(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (DateTime.TryParseExact(
                    Console.ReadLine(),
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date) && date <= DateTime.Today)
                {
                    return date;
                }

                Console.WriteLine("Neispravan datum. Pokusajte ponovo.");
            }
        }

        private int ReadMembershipYear(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int year) && year > 0)
                {
                    return year;
                }

                Console.WriteLine("Unesite ispravnu godinu clanstva.");
            }
        }

        private StatusPosetioca ReadStatus(string message)
        {
            while (true)
            {
                Console.Write(message);
                string value = Console.ReadLine()?.Trim().ToUpperInvariant() ?? "";

                if (value == "R")
                    return StatusPosetioca.R;

                if (value == "V")
                    return StatusPosetioca.V;

                Console.WriteLine("Neispravan status. Unesite R ili V.");
            }
        }

        private string ReadEmail(string message)
        {
            while (true)
            {
                string email = ReadRequiredString(message);

                if (System.Net.Mail.MailAddress.TryCreate(email, out var address)
                    && address.Address == email)
                {
                    return email;
                }

                Console.WriteLine("Neispravna email adresa.");
            }
        }

        private string ReadPhone(string message)
        {
            while (true)
            {
                string phone = ReadRequiredString(message);

                if (System.Text.RegularExpressions.Regex.IsMatch(
                    phone, @"^\+?[0-9]{6,15}$"))
                {
                    return phone;
                }

                Console.WriteLine("Neispravan broj telefona.");
            }
        }
    }
}