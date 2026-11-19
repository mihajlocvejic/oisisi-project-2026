using Core.Controllers;
using Core.Models;
using Core.Validation;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace ConsoleClient.Views
{
    public class PosetilacConsoleView
    {
        private readonly PosetilacController _controller;
        private readonly AdresaConsoleView _adrese;

        public PosetilacConsoleView(PosetilacController controller, AdresaConsoleView adrese)
        {
            _controller = controller;
            _adrese = adrese;
        }

        public void AddPosetilac()
        {
            string ime = ReadRequiredString("Ime: ");
            string prezime = ReadRequiredString("Prezime: ");
            DateTime datumRodjenja = ReadDate("Datum rodjenja (dd.MM.yyyy): ");
            string kontaktTelefon = ReadPhone("Kontakt telefon: ");
            string email = ReadEmail("Email: ");
            string brojClanskeKarte = ReadMembershipCard("Broj clanske karte: ");
            int godinaClanstva = ReadMembershipYear("Godina clanstva: ");
            StatusPosetioca status = ReadStatus("Status (R/V): ");
            Adresa? adresa = ReadAddress();

            if (adresa == null)
            {
                Console.WriteLine("Operacija je otkazana.");
                return;
            }

            Posetilac posetilac = new Posetilac(
                ime, prezime, datumRodjenja,
                adresa, kontaktTelefon, email,
                brojClanskeKarte, godinaClanstva, status
            );

            if (_controller.Add(posetilac))
            {
                Console.WriteLine("Posetilac je uspesno dodat.");
            }
            else
            {
                Console.WriteLine("Podaci nisu validni ili broj clanske karte vec postoji.");
            }
        }

        public void UpdatePosetilac()
        {
            Console.Write("Unesite broj clanske karte posetioca za izmenu: ");
            string brojClanskeKarte = Console.ReadLine() ?? string.Empty;

            Posetilac? postojeci = _controller.GetById(brojClanskeKarte);

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
            Adresa? adresa = ReadAddress();

            if (adresa == null)
            {
                Console.WriteLine("Operacija je otkazana.");
                return;
            }

            Posetilac izmenjeni = new Posetilac(
                ime, prezime, datumRodjenja,
                adresa, kontaktTelefon, email,
                noviBrojClanskeKarte, godinaClanstva, status
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
            string brojClanskeKarte = Console.ReadLine()?.Trim() ?? string.Empty;

            if (_controller.GetById(brojClanskeKarte) == null)
            {
                Console.WriteLine("Posetilac nije pronadjen.");
                return;
            }

            if (_controller.Remove(brojClanskeKarte))
            {
                Console.WriteLine("Posetilac je uspesno obrisan.");
            }
            else
            {
                Console.WriteLine("Brisanje posetioca nije uspelo.");
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

                Console.WriteLine("Kupovine:");

                foreach (Kupovina kupovina in posetilac.Kupovine)
                {
                    Console.WriteLine($"  - {kupovina}");
                }

                Console.WriteLine("Lista zelja:");

                foreach (string isbn in posetilac.ListaZeljaISBN)
                {
                    Console.WriteLine($"  - {isbn}");
                }

                Console.WriteLine();
            }
        }

        public void AddZelja()
        {
            if (!_controller.CanAddZelja())
            {
                Console.WriteLine("Nema posetilaca ili knjiga.");
                return;
            }

            string brojClanskeKarte =
                ReadMembershipCard("Broj clanske karte: ");

            string isbn = ReadRequiredString("ISBN knjige: ");

            if (_controller.AddZelja(brojClanskeKarte, isbn))
            {
                Console.WriteLine("Knjiga je dodata na listu zelja.");
            }
            else
            {
                Console.WriteLine("Dodavanje knjige na listu zelja nije uspelo.");
            }
        }

        public void RemoveZelja()
        {
            string brojClanskeKarte =
                ReadMembershipCard("Broj clanske karte: ");

            string isbn = ReadRequiredString("ISBN knjige: ");

            if (_controller.RemoveZelja(brojClanskeKarte, isbn))
            {
                Console.WriteLine("Knjiga je uklonjena sa liste zelja.");
            }
            else
            {
                Console.WriteLine("Uklanjanje knjige sa liste zelja nije uspelo.");
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
                Console.WriteLine("5. Dodavanje knjige na listu zelja");
                Console.WriteLine("6. Uklanjanje knjige sa liste zelja");
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
                    case "5":
                        AddZelja();
                        break;
                    case "6":
                        RemoveZelja();
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

                if (PosetilacValidator.IsValidText(value))
                    return value;

                Console.WriteLine("Polje ne sme biti prazno niti sadrzati znak |.");
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
                    out DateTime date)
                    && PosetilacValidator.IsValidBirthDate(date))
                {
                    return date;
                }

                Console.WriteLine("Neispravan datum. Pokusajte ponovo.");
            }
        }

        private string ReadMembershipCard(string message)
        {
            while (true)
            {
                string card = ReadRequiredString(message);

                if (PosetilacValidator.IsValidMembershipCard(card))
                    return card;

                Console.WriteLine("Neispravan format. Primer: SK-1-2023.");
            }
        }

        private int ReadMembershipYear(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int year)
                    && PosetilacValidator.IsValidMembershipYear(year))
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

                if (Enum.TryParse<StatusPosetioca>(value, out var status)
                    && PosetilacValidator.IsValidStatus(status)
                    && value == status.ToString())
                {
                    return status;
                }

                Console.WriteLine("Neispravan status. Unesite R ili V.");
            }
        }

        private string ReadPhone(string message)
        {
            while (true)
            {
                string phone = ReadRequiredString(message);

                if (PosetilacValidator.IsValidPhone(phone))
                    return phone;

                Console.WriteLine("Neispravan broj telefona.");
            }
        }

        private string ReadEmail(string message)
        {
            while (true)
            {
                string email = ReadRequiredString(message);

                if (PosetilacValidator.IsValidEmail(email))
                    return email;

                Console.WriteLine("Neispravna email adresa.");
            }
        }
        private Adresa? ReadAddress()
        {
            return _adrese.SelectAddress();
        }
    }
}