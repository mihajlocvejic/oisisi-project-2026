using System.Globalization;
using Core.Controllers;
using Core.Models;
using Core.Utils;
using Core.Validation;

namespace ConsoleClient.Views
{
    public class KnjigaConsoleView
    {
        private readonly KnjigaController _controller;
        public KnjigaConsoleView(KnjigaController controller)
        {
            ArgumentNullException.ThrowIfNull(controller);
            _controller = controller;
        }

        public void ShowAll()
        {
            List<Knjiga> knjige = _controller.GetAll();
            if (knjige.Count == 0) Console.WriteLine("Nema knjiga.");
            foreach (Knjiga knjiga in knjige) ShowBook(knjiga);
        }

        public void ShowByISBN()
        {
            Knjiga? knjiga = ReadExistingBook();
            if (knjiga != null) ShowBook(knjiga);
        }

        private void ShowBook(Knjiga knjiga)
        {
            Console.WriteLine(knjiga);
            PrikaziAutore(knjiga.ISBN);
            Console.WriteLine($"Kupci (kartice): {string.Join(", ", knjiga.KupciClanskeKarte)}");
            Console.WriteLine($"Na listi zelja (kartice): {string.Join(", ", knjiga.ListaZeljaClanskeKarte)}");
        }

        public void AddKnjiga()
        {
            string? isbn = ReadText("ISBN", value => KnjigaValidator.IsValidISBN(value)
                && _controller.GetByISBN(value) == null);
            if (isbn == null) { Console.WriteLine("Dodavanje otkazano."); return; }
            Knjiga? knjiga = ReadBook(isbn);
            if (knjiga == null) { Console.WriteLine("Dodavanje otkazano."); return; }
            int? count = ReadInteger("Broj autora (0 ako nema autora)", value => value >= 0);
            if (count == null) { Console.WriteLine("Dodavanje otkazano."); return; }
            List<string> autori = new();
            for (int i = 0; i < count.Value; i++)
            {
                string? id = ReadText($"Licna karta autora {i + 1}", value =>
                    _controller.AutorPostoji(value) && !autori.Any(id => EntityComparison.SameIdentifier(id, value)));
                if (id == null) { Console.WriteLine("Dodavanje otkazano."); return; }
                autori.Add(id);
            }
            Console.WriteLine(_controller.Add(knjiga, autori)
                ? "Knjiga je uspesno sacuvana." : "Knjiga nije dodata. Proverite podatke, ISBN i autore.");
        }

        public void UpdateKnjiga()
        {
            Knjiga? existing = ReadExistingBook();
            if (existing == null) { Console.WriteLine("Izmena otkazana."); return; }
            Console.WriteLine($"Trenutni podaci: {existing}");
            Console.WriteLine($"ISBN ostaje: {existing.ISBN}");
            Knjiga? edited = ReadBook(existing.ISBN);
            if (edited == null) { Console.WriteLine("Izmena otkazana."); return; }
            Console.WriteLine(_controller.Update(existing.ISBN, edited)
                ? "Knjiga je uspesno izmenjena." : "Knjiga nije izmenjena. Proverite podatke.");
        }

        public void RemoveKnjiga()
        {
            Knjiga? knjiga = ReadExistingBook();
            if (knjiga == null) { Console.WriteLine("Brisanje otkazano."); return; }
            Console.WriteLine(_controller.Remove(knjiga.ISBN) ? "Knjiga je uspesno obrisana." : "Knjiga nije obrisana.");
        }

        private Knjiga? ReadBook(string isbn)
        {
            string? naziv = ReadText("Naziv", KnjigaValidator.IsValidText); if (naziv == null) return null;
            string? zanr = ReadText("Zanr", KnjigaValidator.IsValidText); if (zanr == null) return null;
            int? godina = ReadInteger($"Godina izdanja (1450-{DateTime.Today.Year})", KnjigaValidator.IsValidYear);
            if (godina == null) return null;
            decimal? cena = ReadPrice(); if (cena == null) return null;
            int? strane = ReadInteger("Broj strana", KnjigaValidator.IsValidPageCount); if (strane == null) return null;
            string? izdavac = ReadText("Sifra izdavaca", KnjigaValidator.IsValidPublisherCode); if (izdavac == null) return null;
            return new Knjiga(isbn, naziv, zanr, godina.Value, cena.Value, strane.Value,
                izdavac, new List<string>(), new List<string>());
        }

        private Knjiga? ReadExistingBook()
        {
            if (_controller.GetAll().Count == 0) { Console.WriteLine("Nema knjiga."); return null; }
            while (true)
            {
                string? isbn = ReadText("ISBN knjige", KnjigaValidator.IsValidISBN);
                if (isbn == null) return null;
                Knjiga? knjiga = _controller.GetByISBN(isbn);
                if (knjiga != null) return knjiga;
                Console.WriteLine("Knjiga nije pronadjena. Pokusajte ponovo.");
            }
        }

        private void PrikaziAutore(string isbn)
        {
            List<Autor> autori = _controller.GetAutoriKnjige(isbn);
            Console.WriteLine("Autori:");
            if (autori.Count == 0) Console.WriteLine("  Nema povezanih autora.");
            foreach (Autor autor in autori)
                Console.WriteLine($"  {autor.BrojLicneKarte} | {autor.Ime} {autor.Prezime}");
        }

        public void DodajAutora()
        {
            Knjiga? knjiga = ReadExistingBook();
            if (knjiga == null) return;
            PrikaziAutore(knjiga.ISBN);
            string? id = ReadText("Licna karta autora", value => _controller.AutorPostoji(value)
                && !_controller.GetAutoriZaKnjigu(knjiga.ISBN).Any(existing => EntityComparison.SameIdentifier(existing, value)));
            if (id == null) return;
            Console.WriteLine(_controller.DodajAutoraNaKnjigu(id, knjiga.ISBN)
                ? "Autor je uspesno dodat na knjigu." : "Povezivanje nije uspelo. Proverite autora, knjigu i postojecu vezu.");
        }

        public void UkloniAutora()
        {
            Knjiga? knjiga = ReadExistingBook();
            if (knjiga == null) return;
            PrikaziAutore(knjiga.ISBN);
            List<string> autori = _controller.GetAutoriZaKnjigu(knjiga.ISBN);
            if (autori.Count == 0) return;
            string? id = ReadText("Licna karta autora za uklanjanje", value =>
                autori.Any(existing => EntityComparison.SameIdentifier(existing, value)));
            if (id == null) return;
            Console.WriteLine(_controller.UkloniAutoraSaKnjige(id, knjiga.ISBN)
                ? "Autor je uspesno uklonjen sa knjige." : "Uklanjanje veze nije uspelo.");
        }

        private static string? ReadText(string label, Func<string, bool> validate)
        {
            while (true)
            {
                Console.Write($"{label} (/nazad za odustajanje): ");
                string? input = Console.ReadLine();
                if (IsCancelled(input)) return null;
                string value = input!.Trim();
                if (validate(value)) return value;
                Console.WriteLine("Unos nije ispravan, zapis ne postoji ili vec postoji. Pokusajte ponovo.");
            }
        }

        private static int? ReadInteger(string label, Func<int, bool> validate)
        {
            while (true)
            {
                Console.Write($"{label} (/nazad za odustajanje): ");
                string? input = Console.ReadLine();
                if (IsCancelled(input)) return null;
                if (int.TryParse(input, out int value) && validate(value)) return value;
                Console.WriteLine("Unesite ceo broj u dozvoljenom opsegu.");
            }
        }

        private static decimal? ReadPrice()
        {
            while (true)
            {
                Console.Write($"Cena (decimalni znak '{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}', /nazad za odustajanje): ");
                string? input = Console.ReadLine();
                if (IsCancelled(input)) return null;
                if (decimal.TryParse(input!.Trim(), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                    CultureInfo.CurrentCulture, out decimal price) && KnjigaValidator.IsValidPrice(price)) return price;
                Console.WriteLine("Unesite nenegativnu cenu sa prikazanim decimalnim znakom, bez separatora hiljada.");
            }
        }

        private static bool IsCancelled(string? input) => input == null
            || input.Trim().Equals("/nazad", StringComparison.OrdinalIgnoreCase);

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Knjige ---\n1. Dodaj\n2. Izmeni\n3. Obrisi\n4. Prikazi sve\n5. Pronadji po ISBN-u\n6. Dodaj autora\n7. Ukloni autora\n0. Nazad");
                string? choice = Console.ReadLine();
                if (choice == null || choice.Trim() == "0") return;
                try
                {
                    switch (choice.Trim())
                    {
                        case "1": AddKnjiga(); break;
                        case "2": UpdateKnjiga(); break;
                        case "3": RemoveKnjiga(); break;
                        case "4": ShowAll(); break;
                        case "5": ShowByISBN(); break;
                        case "6": DodajAutora(); break;
                        case "7": UkloniAutora(); break;
                        default: Console.WriteLine("Nepoznata opcija."); break;
                    }
                }
                catch (IOException ex) { Console.WriteLine($"Greska pri radu sa datotekom: {ex.Message}"); }
                catch (UnauthorizedAccessException ex) { Console.WriteLine($"Pristup datoteci nije dozvoljen: {ex.Message}"); }
            }
        }
    }
}
