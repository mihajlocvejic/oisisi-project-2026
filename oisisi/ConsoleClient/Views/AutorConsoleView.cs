using Core.Controllers;
using Core.Models;
using Core.Validation;
using System.Globalization;

namespace ConsoleClient.Views
{
    public class AutorConsoleView
    {
        private readonly AutorController _controller;
        private readonly AdresaConsoleView _adrese;

        public AutorConsoleView(AutorController controller, AdresaConsoleView adrese)
        {
            ArgumentNullException.ThrowIfNull(controller);
            ArgumentNullException.ThrowIfNull(adrese);
            _controller = controller;
            _adrese = adrese;
        }

        public void AddAutor()
        {
            Autor? autor = ReadAuthor();
            if (autor == null) { Console.WriteLine("Dodavanje autora otkazano."); return; }
            Console.WriteLine(_controller.Add(autor)
                ? "Autor je uspesno dodat."
                : "Autor nije dodat. Proverite podatke, adresu i jedinstvenost broja licne karte.");
        }

        public void ShowAll()
        {
            List<Autor> autori = _controller.GetAll();
            if (autori.Count == 0) Console.WriteLine("Nema autora.");
            foreach (Autor autor in autori) Console.WriteLine(autor);
        }

        public void RemoveAutor()
        {
            Autor? autor = ReadExistingAuthor();
            if (autor == null) { Console.WriteLine("Brisanje autora otkazano."); return; }
            Console.WriteLine(_controller.Remove(autor.BrojLicneKarte)
                ? "Autor je uspesno obrisan." : "Autor nije obrisan.");
        }

        public void UpdateAutor()
        {
            Autor? existing = ReadExistingAuthor();
            if (existing == null) { Console.WriteLine("Izmena autora otkazana."); return; }
            Console.WriteLine($"Trenutni podaci: {existing}");
            Autor? edited = ReadAuthor(existing.BrojLicneKarte);
            if (edited == null) { Console.WriteLine("Izmena autora otkazana."); return; }
            Console.WriteLine(_controller.Update(existing.BrojLicneKarte, edited)
                ? "Autor je uspesno izmenjen." : "Autor nije izmenjen. Proverite podatke i adresu.");
        }

        private Autor? ReadExistingAuthor()
        {
            if (_controller.GetAll().Count == 0) { Console.WriteLine("Nema autora."); return null; }
            while (true)
            {
                string? id = ReadText("Broj licne karte", AutorValidator.IsValidIdentityCard);
                if (id == null) return null;
                Autor? autor = _controller.GetById(id);
                if (autor != null) return autor;
                Console.WriteLine("Autor nije pronadjen. Pokusajte ponovo.");
            }
        }

        private Autor? ReadAuthor(string? existingId = null)
        {
            string? ime = ReadText("Ime", AutorValidator.IsValidText); if (ime == null) return null;
            string? prezime = ReadText("Prezime", AutorValidator.IsValidText); if (prezime == null) return null;
            DateTime? datum = ReadDate(); if (datum == null) return null;
            Adresa? adresa = _adrese.SelectAddress(); if (adresa == null) return null;
            string? telefon = ReadText("Telefon (6-15 cifara, opcioni +)", AutorValidator.IsValidPhone); if (telefon == null) return null;
            string? email = ReadText("Email", AutorValidator.IsValidEmail); if (email == null) return null;
            string? id = existingId;
            if (id == null)
            {
                id = ReadText("Broj licne karte", value => AutorValidator.IsValidIdentityCard(value)
                    && _controller.GetById(value) == null);
                if (id == null) return null;
            }
            int? iskustvo = ReadExperience(datum.Value); if (iskustvo == null) return null;
            return new Autor(ime, prezime, datum.Value, adresa, telefon, email, id, iskustvo.Value);
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
                Console.WriteLine("Unos nije ispravan ili je identifikator zauzet. Pokusajte ponovo.");
            }
        }

        private static DateTime? ReadDate()
        {
            while (true)
            {
                Console.Write("Datum rodjenja (dd.MM.yyyy, /nazad za odustajanje): ");
                string? input = Console.ReadLine();
                if (IsCancelled(input)) return null;
                if (DateTime.TryParseExact(input!.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime date) && AutorValidator.IsValidBirthDate(date)) return date;
                Console.WriteLine("Unesite postojeci datum koji nije u buducnosti.");
            }
        }

        private static int? ReadExperience(DateTime birthDate)
        {
            while (true)
            {
                Console.Write($"Godine iskustva (0-{AutorValidator.GetAge(birthDate)}, /nazad za odustajanje): ");
                string? input = Console.ReadLine();
                if (IsCancelled(input)) return null;
                if (int.TryParse(input, out int years) && AutorValidator.IsValidExperience(years, birthDate)) return years;
                Console.WriteLine("Godine iskustva moraju biti ceo broj izmedju 0 i navrsene starosti.");
            }
        }

        private static bool IsCancelled(string? input) => input == null
            || input.Trim().Equals("/nazad", StringComparison.OrdinalIgnoreCase);

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Upravljanje autorima ---");
                Console.WriteLine("1. Dodavanje autora");
                Console.WriteLine("2. Izmena autora");
                Console.WriteLine("3. Brisanje autora");
                Console.WriteLine("4. Prikaz autora");
                Console.WriteLine("0. Izlaz");
                Console.Write("Izaberite opciju: ");
                string? choice = Console.ReadLine();
                if (choice == null || choice.Trim() == "0") return;
                try
                {
                    switch (choice.Trim())
                    {
                        case "1": AddAutor(); break;
                        case "2": UpdateAutor(); break;
                        case "3": RemoveAutor(); break;
                        case "4": ShowAll(); break;
                        default: Console.WriteLine("Nepoznata opcija."); break;
                    }
                }
                catch (IOException ex) { Console.WriteLine($"Greska pri radu sa datotekom: {ex.Message}"); }
                catch (UnauthorizedAccessException ex) { Console.WriteLine($"Pristup datoteci nije dozvoljen: {ex.Message}"); }
            }
        }
    }
}
