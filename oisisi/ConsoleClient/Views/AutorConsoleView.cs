
using Core.Controllers;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ConsoleClient.Views
{
    public class AutorConsoleView
    {
        private readonly AutorController _controller;

        public AutorConsoleView(AutorController controller)
        {
            _controller = controller;
        }

        public void AddAutor()
        {
            Console.Write("Ime: ");
            string ime = Console.ReadLine() ?? string.Empty;

            Console.Write("Prezime: ");
            string prezime = Console.ReadLine() ?? string.Empty;

            Console.Write("Datum rodjenja (dd.MM.yyyy): ");
            DateTime datumRodjenja = DateTime.ParseExact(
                Console.ReadLine() ?? "",
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture);

            Console.Write("Ulica: ");
            string ulica = Console.ReadLine() ?? string.Empty;

            Console.Write("Broj: ");
            string broj = Console.ReadLine() ?? string.Empty;

            Console.Write("Grad: ");
            string grad = Console.ReadLine() ?? string.Empty;

            Console.Write("Drzava: ");
            string drzava = Console.ReadLine() ?? string.Empty;

            Adresa adresa = new Adresa(ulica, broj, grad, drzava);

            Console.Write("Telefon: ");
            string telefon = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            Console.Write("Broj licne karte: ");
            string brojLicneKarte = Console.ReadLine() ?? string.Empty;

            Console.Write("Godine iskustva: ");
            int godineIskustva = int.Parse(Console.ReadLine() ?? "");

            Autor autor = new Autor(
                ime,
                prezime,
                datumRodjenja,
                adresa,
                telefon,
                email,
                brojLicneKarte,
                godineIskustva,
                new List<string>()
            );

            _controller.Add(autor);

            Console.WriteLine("Autor je uspesno dodat.");
        }

        public void ShowAll()
        {
            List<Autor> autori = _controller.GetAll();

            if (autori.Count == 0)
            {
                Console.WriteLine("Nema autora.");
                return;
            }

            foreach (Autor autor in autori)
            {
                Console.WriteLine(autor);
            }
        }

        public void RemoveAutor()
        {
            Console.Write("Broj licne karte autora: ");
            string brojLicneKarte = Console.ReadLine() ?? string.Empty;

            if (_controller.Remove(brojLicneKarte))
                Console.WriteLine("Autor je uspesno obrisan.");
            else
                Console.WriteLine("Autor nije pronadjen.");
        }


        public void UpdateAutor()
        {
            Console.Write("Broj licne karte autora: ");
            string brojLicneKarte = Console.ReadLine() ?? string.Empty;

            Autor? existing = _controller.GetById(brojLicneKarte);

            if (existing == null)
            {
                Console.WriteLine("Autor nije pronadjen.");
                return;
            }

            try
            {
                Autor autor = new Autor { BrojLicneKarte = brojLicneKarte };

                Console.WriteLine("Unesite nove podatke autora.");

                Console.Write("Ime: ");
                autor.Ime = Console.ReadLine() ?? string.Empty;

                Console.Write("Prezime: ");
                autor.Prezime = Console.ReadLine() ?? string.Empty;

                Console.Write("Datum rodjenja (dd.MM.yyyy): ");
                autor.DatumRodjenja = DateTime.ParseExact(
                    Console.ReadLine() ?? "",
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture
                );

                Console.Write("Ulica: ");
                string ulica = Console.ReadLine() ?? string.Empty;

                Console.Write("Broj: ");
                string broj = Console.ReadLine() ?? string.Empty;

                Console.Write("Grad: ");
                string grad = Console.ReadLine() ?? string.Empty;

                Console.Write("Drzava: ");
                string drzava = Console.ReadLine() ?? string.Empty;

                autor.Adresa = new Adresa(ulica, broj, grad, drzava);

                Console.Write("Telefon: ");
                autor.Telefon = Console.ReadLine() ?? string.Empty;

                Console.Write("Email: ");
                autor.Email = Console.ReadLine() ?? string.Empty;

                Console.Write("Godine iskustva: ");
                autor.GodineIskustva = int.Parse(Console.ReadLine() ?? "");

                if (_controller.Update(brojLicneKarte, autor))
                    Console.WriteLine("Autor je uspesno izmenjen.");
                else
                    Console.WriteLine("Izmena nije uspela.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Izmena nije sacuvana. Datum ili broj nisu u ispravnom formatu.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Izmena nije sacuvana. Uneti broj je van dozvoljenog opsega.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Izmena nije sacuvana. {ex.Message}");
            }
        }

    }
}
