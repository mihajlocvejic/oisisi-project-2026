using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Controllers;
using Core.Models;

namespace ConsoleClient.Views
{
    public class KupovinaConsoleView
    {
        private readonly KupovinaController _controller;

        public KupovinaConsoleView(KupovinaController controller)
        {
            _controller = controller;
        }

        public void AddKupovina()
        {
            string brojClanskeKarte = ReadRequiredString("Broj clanske karte: ");
            string isbn = ReadRequiredString("ISBN knjige: ");
            DateTime datumKupovine = ReadDate("Datum kupovine (dd.MM.yyyy): ");
            int ocena = ReadOcena("Ocena (1-5): ");
            string komentar = ReadKomentar("Komentar: ");

            Kupovina kupovina = new Kupovina(
                0, brojClanskeKarte, isbn,
                datumKupovine, ocena, komentar
            );

            _controller.Add(kupovina);

            Console.WriteLine("Kupovina je uspesno dodata.");
        }

        public void UpdateKupovina()
        {
            Console.Write("Unesite ID kupovine za izmenu: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Neispravan ID.");
                return;
            }

            Kupovina? postojeca = _controller.GetAll()
                .Find(k => k.Id == id);

            if (postojeca == null)
            {
                Console.WriteLine("Kupovina nije pronadjena.");
                return;
            }

            Console.WriteLine($"Trenutni podaci: {postojeca}");

            string brojClanskeKarte = ReadRequiredString("Novi broj clanske karte: ");
            string isbn = ReadRequiredString("Novi ISBN knjige: ");
            DateTime datumKupovine = ReadDate("Novi datum kupovine (dd.MM.yyyy): ");
            int ocena = ReadOcena("Nova ocena (1-5): ");
            string komentar = ReadKomentar("Novi komentar: ");

            Kupovina izmenjena = new Kupovina(
                id, brojClanskeKarte, isbn,
                datumKupovine, ocena, komentar
            );

            if (_controller.Update(id, izmenjena))
            {
                Console.WriteLine("Kupovina je uspesno izmenjena.");
            }
            else
            {
                Console.WriteLine("Izmena kupovine nije uspela.");
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

                Console.WriteLine("Neispravan datum kupovine.");
            }
        }

        private int ReadOcena(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int ocena)
                    && ocena >= 1 && ocena <= 5)
                {
                    return ocena;
                }

                Console.WriteLine("Ocena mora biti od 1 do 5.");
            }
        }

        private string ReadKomentar(string message)
        {
            while (true)
            {
                Console.Write(message);
                string komentar = Console.ReadLine()?.Trim() ?? string.Empty;

                if (!komentar.Contains('|'))
                    return komentar;

                Console.WriteLine("Znak | nije dozvoljen.");
            }
        }
    }
}