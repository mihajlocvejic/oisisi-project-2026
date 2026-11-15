
using System;
using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Utils;
using System.Linq;

namespace Core.Services
{
    public class AutorKnjigaService
    {
        private readonly AutorDAO _autorDAO;
        private readonly KnjigaDAO _knjigaDAO;
        private readonly AutorKnjigaDAO _autorKnjigaDAO;

        public AutorKnjigaService(
            AutorDAO autorDAO,
            KnjigaDAO knjigaDAO,
            AutorKnjigaDAO autorKnjigaDAO)
        {
            _autorDAO = autorDAO;
            _knjigaDAO = knjigaDAO;
            _autorKnjigaDAO = autorKnjigaDAO;
        }

        public void DodajAutoraNaKnjigu(
            string brojLicneKarte,
            string isbn)
        {
            Autor? autor = _autorDAO.GetById(brojLicneKarte);

            if (autor == null)
                throw new InvalidOperationException(
                    "Autor ne postoji.");

            Knjiga? knjiga = _knjigaDAO.GetByISBN(isbn);

            if (knjiga == null)
                throw new InvalidOperationException(
                    "Knjiga ne postoji.");

            if (_autorKnjigaDAO.Exists(brojLicneKarte, isbn))
                throw new InvalidOperationException(
                    "Autor je vec povezan sa ovom knjigom.");

            _autorKnjigaDAO.Add(autor.BrojLicneKarte, knjiga.ISBN);
        }

        public void UkloniAutoraSaKnjige(
            string brojLicneKarte,
            string isbn)
        {
            if (_knjigaDAO.GetByISBN(isbn) == null)
                throw new InvalidOperationException(
                    "Knjiga ne postoji.");

            if (!_autorKnjigaDAO.Exists(brojLicneKarte, isbn))
                throw new InvalidOperationException(
                    "Veza autora i knjige ne postoji.");

            _autorKnjigaDAO.Delete(brojLicneKarte, isbn);
        }

        public List<string> GetAutoriZaKnjigu(string isbn)
        {
            return _autorKnjigaDAO.GetAutoriZaKnjigu(isbn);
        }

        public List<string> GetKnjigeZaAutora(
            string brojLicneKarte)
        {
            return _autorKnjigaDAO.GetKnjigeZaAutora(
                brojLicneKarte);
        }


        public void DodajKnjigu(
            Knjiga knjiga,
            List<string> brojeviLicnihKarata)
        {
            if (knjiga == null)
                throw new ArgumentNullException(nameof(knjiga));

            if (brojeviLicnihKarata == null)
                throw new ArgumentNullException(nameof(brojeviLicnihKarata));

            if (brojeviLicnihKarata.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException(
                    "Broj licne karte autora ne moze biti prazan.");

            List<string> autori = brojeviLicnihKarata.DistinctBy(EntityComparison.IdentifierKey).ToList();

            foreach (string brojLicneKarte in autori)
            {
                if (_autorDAO.GetById(brojLicneKarte) == null)
                    throw new InvalidOperationException(
                        $"Autor sa licnom kartom {brojLicneKarte} ne postoji.");
            }

            // Knjiga se prvo cuva u knjige.csv.
            _knjigaDAO.Add(knjiga);

            try
            {
                // Zatim se veze cuvaju u autor_knjiga.csv.
                foreach (string brojLicneKarte in autori)
                {
                    _autorKnjigaDAO.Add(brojLicneKarte, knjiga.ISBN);
                }
            }
            catch
            {
                // Ako cuvanje veze ne uspije, pokusavamo
                // ukloniti prethodno dodate veze i knjigu.
                foreach (string brojLicneKarte in autori)
                {
                    if (_autorKnjigaDAO.Exists(brojLicneKarte, knjiga.ISBN))
                        _autorKnjigaDAO.Delete(brojLicneKarte, knjiga.ISBN);
                }

                _knjigaDAO.Delete(knjiga.ISBN);
                throw;
            }
        }


        public void ObrisiKnjigu(string isbn)
        {
            Knjiga? knjiga = _knjigaDAO.GetByISBN(isbn);

            if (knjiga == null)
                throw new InvalidOperationException(
                    "Knjiga ne postoji.");

            // Pronalazimo sve autore povezane sa knjigom.
            List<string> autori =
                _autorKnjigaDAO.GetAutoriZaKnjigu(isbn);

            // Prvo uklanjamo veze.
            List<string> uklonjeniAutori = new List<string>();

            try
            {
                foreach (string brojLicneKarte in autori)
                {
                    _autorKnjigaDAO.Delete(brojLicneKarte, isbn);
                    uklonjeniAutori.Add(brojLicneKarte);
                }

                // Kada su veze uklonjene, brisemo knjigu.
                _knjigaDAO.Delete(isbn);
            }
            catch
            {
                // Pokusavamo vratiti prethodno uklonjene veze.
                foreach (string brojLicneKarte in uklonjeniAutori)
                {
                    if (!_autorKnjigaDAO.Exists(brojLicneKarte, isbn))
                    {
                        _autorKnjigaDAO.Add(brojLicneKarte, isbn);
                    }
                }

                throw;
            }
        }

        public List<Autor> GetAutoriKnjige(string isbn)
        {
            List<string> brojeviLicnihKarata =
                _autorKnjigaDAO.GetAutoriZaKnjigu(isbn);

            List<Autor> autori = new List<Autor>();

            foreach (string brojLicneKarte in brojeviLicnihKarata)
            {
                Autor? autor = _autorDAO.GetById(brojLicneKarte);

                if (autor != null)
                    autori.Add(autor);
            }

            return autori;
        }


        public void ObrisiAutora(string brojLicneKarte)
        {
            Autor? autor = _autorDAO.GetById(brojLicneKarte);

            if (autor == null)
                throw new InvalidOperationException("Autor ne postoji.");

            List<string> knjige =
                _autorKnjigaDAO.GetKnjigeZaAutora(brojLicneKarte);

            // Uklanjamo sve veze autora sa knjigama.
            List<string> uklonjeneVeze = new List<string>();

            try
            {
                foreach (string isbn in knjige)
                {
                    _autorKnjigaDAO.Delete(brojLicneKarte, isbn);
                    uklonjeneVeze.Add(isbn);
                }

                _autorDAO.Delete(brojLicneKarte);
            }
            catch
            {
                // Pokusavamo vratiti veze ako brisanje ne uspije.
                foreach (string isbn in uklonjeneVeze)
                {
                    if (!_autorKnjigaDAO.Exists(brojLicneKarte, isbn))
                        _autorKnjigaDAO.Add(brojLicneKarte, isbn);
                }

                throw;
            }
        }


    }
}
