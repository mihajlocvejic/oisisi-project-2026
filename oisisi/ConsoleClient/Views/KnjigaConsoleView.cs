
using Core.Controllers;
using Core.Models;
using System;
using System.Collections.Generic;

namespace ConsoleClient.Views
{
    public class KnjigaConsoleView
    {
        private readonly KnjigaController _controller;

        public KnjigaConsoleView(KnjigaController controller)
        {
            _controller = controller;
        }


        public void ShowAll()
        {
            List<Knjiga> knjige = _controller.GetAll();

            if (knjige.Count == 0)
            {
                Console.WriteLine("Nema knjiga.");
                return;
            }

            foreach (Knjiga knjiga in knjige)
            {
                Console.WriteLine(knjiga);

                PrikaziAutore(knjiga.ISBN);

                Console.WriteLine();
            }
        }

        public void ShowByISBN()
        {
            Console.Write("Unesite ISBN knjige: ");
            string isbn = Console.ReadLine() ?? string.Empty;

            Knjiga? knjiga = _controller.GetByISBN(isbn);

            if (knjiga == null)
            {
                Console.WriteLine("Knjiga nije pronadjena.");
                return;
            }

            Console.WriteLine(knjiga);

            PrikaziAutore(isbn);
        }


        public void AddKnjiga()
        {
            try
            {
                Knjiga knjiga = new Knjiga();

                Console.Write("ISBN: ");
                knjiga.ISBN = Console.ReadLine() ?? "";

                Console.Write("Naziv: ");
                knjiga.Naziv = Console.ReadLine() ?? "";

                Console.Write("Zanr: ");
                knjiga.Zanr = Console.ReadLine() ?? "";

                Console.Write("Godina izdanja: ");
                knjiga.GodinaIzdanja = int.Parse(
                    Console.ReadLine() ?? "");

                Console.Write("Cena: ");
                knjiga.Cena = decimal.Parse(
                    Console.ReadLine() ?? "");

                Console.Write("Broj strana: ");
                knjiga.BrojStrana = int.Parse(
                    Console.ReadLine() ?? "");

                Console.Write("Sifra izdavaca: ");
                knjiga.SifraIzdavaca = Console.ReadLine() ?? "";

                Console.Write("Koliko autora ima knjiga (0 ako nema autora)? ");
                int brojAutora = int.Parse(
                    Console.ReadLine() ?? "");

                if (brojAutora < 0)
                    throw new ArgumentException("Broj autora ne moze biti negativan.");

                List<string> autori = new List<string>();

                for (int i = 0; i < brojAutora; i++)
                {
                    Console.Write($"Licna karta autora {i + 1}: ");
                    string brojLicneKarte =
                        Console.ReadLine() ?? "";

                    autori.Add(brojLicneKarte);
                }

                _controller.Add(knjiga, autori);

                Console.WriteLine(
                    "Knjiga je uspesno sacuvana.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska: {ex.Message}");
            }
        }

        public void RemoveKnjiga()
        {
            Console.Write("Unesite ISBN knjige: ");
            string isbn = Console.ReadLine() ?? string.Empty;

            if (_controller.Remove(isbn))
                Console.WriteLine("Knjiga je uspesno obrisana.");
            else
                Console.WriteLine("Knjiga nije pronadjena.");
        }

        private void PrikaziAutore(string isbn)
        {
            List<Autor> autori = _controller.GetAutoriKnjige(isbn);

            Console.WriteLine("Autori:");

            if (autori.Count == 0)
            {
                Console.WriteLine("  Nema povezanih autora.");
                return;
            }

            foreach (Autor autor in autori)
            {
                Console.WriteLine($"  - {autor.Ime} {autor.Prezime}");
            }
        }


        public void UkloniAutora()
        {
            Console.Write("Unesite ISBN knjige: ");
            string isbn = Console.ReadLine() ?? "";

            Knjiga? knjiga = _controller.GetByISBN(isbn);

            if (knjiga == null)
            {
                Console.WriteLine("Knjiga ne postoji.");
                return;
            }

            PrikaziAutore(isbn);

            Console.Write("Unesite broj licne karte autora za uklanjanje: ");
            string brojLicneKarte = Console.ReadLine() ?? "";

            try
            {
                _controller.UkloniAutoraSaKnjige(brojLicneKarte, isbn);
                Console.WriteLine("Autor je uspesno uklonjen sa knjige.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska: {ex.Message}");
            }
        }


        public void DodajAutora()
        {
            Console.Write("Unesite ISBN knjige: ");
            string isbn = Console.ReadLine() ?? "";

            Knjiga? knjiga = _controller.GetByISBN(isbn);

            if (knjiga == null)
            {
                Console.WriteLine("Knjiga ne postoji.");
                return;
            }

            PrikaziAutore(isbn);

            Console.Write("Unesite broj licne karte autora za dodavanje: ");
            string brojLicneKarte = Console.ReadLine() ?? "";

            try
            {
                _controller.DodajAutoraNaKnjigu(brojLicneKarte, isbn);
                Console.WriteLine("Autor je uspesno dodat na knjigu.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska: {ex.Message}");
            }
        }


    }
}
