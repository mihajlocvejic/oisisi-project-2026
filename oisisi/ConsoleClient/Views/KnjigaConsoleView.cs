
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
        }
    }
}
