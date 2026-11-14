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