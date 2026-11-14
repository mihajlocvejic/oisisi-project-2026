using Core.Controllers;
using Core.Models;
using System;
using System.Globalization;

namespace ConsoleClient.Views
{
    public class PosetilacConsoleView
    {
        private readonly PosetilacController _controller;

        public PosetilacConsoleView(PosetilacController controller)
        {
            _controller = controller;
        }

        public void AddPosetilac()
        {
            Console.Write("Ime: ");
            string ime = Console.ReadLine() ?? string.Empty;

            Console.Write("Prezime: ");
            string prezime = Console.ReadLine() ?? string.Empty;

            Console.Write("Datum rodjenja (dd.MM.yyyy): ");
            DateTime datumRodjenja = DateTime.ParseExact(Console.ReadLine() ?? "", "dd.MM.yyyy", CultureInfo.InvariantCulture);

            Console.Write("Kontakt telefon: ");
            string kontaktTelefon = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? string.Empty;

            Console.Write("Broj clanske karte: ");
            string brojClanskeKarte = Console.ReadLine() ?? string.Empty;

            Console.Write("Godina clanstva: ");
            int godinaClanstva = int.Parse(Console.ReadLine() ?? "");

            Console.Write("Status (R/V): ");
            StatusPosetioca status = Enum.Parse<StatusPosetioca>(Console.ReadLine() ?? "", true);

            Posetilac posetilac = new Posetilac(
                ime, prezime, datumRodjenja,
                kontaktTelefon, email, brojClanskeKarte,
                godinaClanstva, status, 0
            );

            _controller.Add(posetilac);

            Console.WriteLine("Posetilac je uspesno dodat.");
        }
    }
}