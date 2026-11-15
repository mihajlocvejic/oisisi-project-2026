using Core.Controllers;
using Core.Models;

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

        public Adresa SelectAddress()
        {
            ShowAll();
            Console.Write("ID adrese (prvo je dodajte u meniju Adrese): ");
            int id = ReadId();
            return _controller.GetById(id) ?? throw new ArgumentException("Adresa ne postoji.");
        }

        private static int ReadId()
        {
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                throw new ArgumentException("ID mora biti pozitivan ceo broj.");
            return id;
        }

        private static Adresa ReadAddress()
        {
            Console.Write("Ulica: "); string ulica = Console.ReadLine() ?? "";
            Console.Write("Broj: "); string broj = Console.ReadLine() ?? "";
            Console.Write("Grad: "); string grad = Console.ReadLine() ?? "";
            Console.Write("Drzava: "); string drzava = Console.ReadLine() ?? "";
            return new Adresa(ulica, broj, grad, drzava);
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Adrese ---\n1. Dodaj\n2. Izmeni\n3. Obrisi\n4. Prikazi\n0. Nazad");
                string? izbor = Console.ReadLine();
                if (izbor == null || izbor == "0") return;
                try
                {
                    switch (izbor)
                    {
                        case "1": Console.WriteLine($"Dodata adresa: {_controller.Add(ReadAddress()).Id}"); break;
                        case "2":
                            Console.Write("ID adrese: "); int id = ReadId();
                            if (_controller.GetById(id) == null) { Console.WriteLine("Adresa nije pronadjena."); break; }
                            Console.WriteLine(_controller.Update(id, ReadAddress()) ? "Adresa izmenjena." : "Adresa nije pronadjena."); break;
                        case "3":
                            Console.Write("ID adrese: ");
                            Console.WriteLine(_controller.Remove(ReadId()) ? "Adresa obrisana." : "Adresa nije pronadjena."); break;
                        case "4": ShowAll(); break;
                        default: Console.WriteLine("Nepoznata opcija."); break;
                    }
                }
                catch (ArgumentException ex) { Console.WriteLine(ex.Message); }
                catch (InvalidOperationException ex) { Console.WriteLine(ex.Message); }
            }
        }
    }
}
