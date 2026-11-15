using Core.Controllers;
using Core.Models;
using Core.Validation;

namespace ConsoleClient.Views
{
    public class AdresaConsoleView
    {
        private readonly AdresaController _controller;
        public AdresaConsoleView(AdresaController controller) { _controller = controller; }

        public void ShowAll()
        {
            List<Adresa> adrese = _controller.GetAll();
            if (adrese.Count == 0) Console.WriteLine("Nema adresa.");
            foreach (Adresa adresa in adrese) Console.WriteLine($"{adresa.Id} | {adresa}");
        }

        public Adresa? SelectAddress()
        {
            if (_controller.GetAll().Count == 0)
            {
                Console.WriteLine("Nema adresa. Prvo dodajte adresu u meniju Adrese.");
                return null;
            }
            ShowAll();
            while (true)
            {
                int? id = ReadId();
                if (id == null) return null;
                Adresa? adresa = _controller.GetById(id.Value);
                if (adresa != null) return adresa;
                Console.WriteLine("Adresa ne postoji. Izaberite ID iz prikazane liste.");
            }
        }

        private static int? ReadId()
        {
            while (true)
            {
                Console.Write("ID adrese (0 za odustajanje): ");
                string? input = Console.ReadLine();
                if (input == null || input.Trim() == "0") return null;
                if (int.TryParse(input, out int id) && id > 0) return id;
                Console.WriteLine("ID mora biti pozitivan ceo broj.");
            }
        }

        private static string? ReadText(string label)
        {
            while (true)
            {
                Console.Write($"{label} (/nazad za odustajanje): ");
                string? input = Console.ReadLine();
                if (input == null || input.Trim().Equals("/nazad", StringComparison.OrdinalIgnoreCase)) return null;
                if (AdresaValidator.IsValidText(input)) return input.Trim();
                Console.WriteLine("Polje je obavezno i ne sme sadrzati | ili novi red. Pokusajte ponovo.");
            }
        }

        private static Adresa? ReadAddress()
        {
            string? ulica = ReadText("Ulica"); if (ulica == null) return null;
            string? broj = ReadText("Broj"); if (broj == null) return null;
            string? grad = ReadText("Grad"); if (grad == null) return null;
            string? drzava = ReadText("Drzava"); if (drzava == null) return null;
            return new Adresa(ulica, broj, grad, drzava);
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Adrese ---\n1. Dodaj\n2. Izmeni\n3. Obrisi\n4. Prikazi\n0. Nazad");
                string? izbor = Console.ReadLine();
                if (izbor == null || izbor.Trim() == "0") return;
                try
                {
                    switch (izbor.Trim())
                    {
                        case "1":
                            Adresa? nova = ReadAddress();
                            if (nova == null) { Console.WriteLine("Dodavanje otkazano."); break; }
                            if (_controller.TryAdd(nova, out Adresa? created, out string addError))
                                Console.WriteLine($"Dodata adresa: {created!.Id}");
                            else Console.WriteLine(addError);
                            break;
                        case "2":
                            Adresa? existing = SelectAddress();
                            if (existing == null) { Console.WriteLine("Izmena otkazana."); break; }
                            Console.WriteLine($"Trenutna adresa: {existing}");
                            Adresa? edited = ReadAddress();
                            if (edited == null) { Console.WriteLine("Izmena otkazana."); break; }
                            Console.WriteLine(_controller.TryUpdate(existing.Id, edited, out string updateError)
                                ? "Adresa izmenjena." : updateError);
                            break;
                        case "3":
                            Adresa? selected = SelectAddress();
                            if (selected == null) { Console.WriteLine("Brisanje otkazano."); break; }
                            Console.WriteLine(_controller.TryRemove(selected.Id, out string removeError)
                                ? "Adresa obrisana." : removeError);
                            break;
                        case "4": ShowAll(); break;
                        default: Console.WriteLine("Nepoznata opcija."); break;
                    }
                }
                catch (IOException ex) { Console.WriteLine($"Greska pri cuvanju adrese: {ex.Message}"); }
                catch (UnauthorizedAccessException ex) { Console.WriteLine($"Pristup datoteci nije dozvoljen: {ex.Message}"); }
                catch (OverflowException) { Console.WriteLine("Nije moguce generisati novi ID adrese."); }
            }
        }
    }
}
