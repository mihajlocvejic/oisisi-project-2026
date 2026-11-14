
using Core.DAO;
using Core.Models;
using Core.Services;
using System.Collections.Generic;

namespace Core.Controllers
{
    public class KnjigaController
    {
        private readonly KnjigaDAO _dao;
        private readonly AutorKnjigaService _service;

        public KnjigaController(
            KnjigaDAO dao,
            AutorKnjigaService service)
        {
            _dao = dao;
            _service = service;
        }

        public List<Autor> GetAutoriKnjige(string isbn)
        {
            return _service.GetAutoriKnjige(isbn);
        }

        public void Add(
            Knjiga knjiga,
            List<string> brojeviLicnihKarata)
        {
            _service.DodajKnjigu(
                knjiga,
                brojeviLicnihKarata);
        }

        public bool Update(string isbn, Knjiga knjiga)
        {
            Knjiga? existing = _dao.GetByISBN(isbn);

            if (existing == null)
                return false;

            knjiga.ISBN = isbn;
            _dao.Update(knjiga);

            return true;
        }

        public bool Remove(string isbn)
        {
            if (_dao.GetByISBN(isbn) == null)
                return false;

            _service.ObrisiKnjigu(isbn);
            return true;
        }

        public Knjiga? GetByISBN(string isbn)
        {
            return _dao.GetByISBN(isbn);
        }

        public List<Knjiga> GetAll()
        {
            return _dao.GetAll();
        }

        public List<string> GetAutoriZaKnjigu(string isbn)
        {
            return _service.GetAutoriZaKnjigu(isbn);
        }

        public void UkloniAutoraSaKnjige(string brojLicneKarte, string isbn)
        {
            _service.UkloniAutoraSaKnjige(brojLicneKarte, isbn);
        }

        public void DodajAutoraNaKnjigu(string brojLicneKarte, string isbn)
        {
            _service.DodajAutoraNaKnjigu(brojLicneKarte, isbn);
        }


    }
}
