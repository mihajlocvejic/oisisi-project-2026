using Core.DAO;
using Core.Models;
using Core.Services;
using Core.Utils;
using Core.Validation;

namespace Core.Controllers
{
    public class KnjigaController
    {
        private readonly KnjigaDAO _dao;
        private readonly AutorKnjigaService _service;

        public KnjigaController(KnjigaDAO dao, AutorKnjigaService service)
        {
            ArgumentNullException.ThrowIfNull(dao);
            ArgumentNullException.ThrowIfNull(service);
            _dao = dao;
            _service = service;
        }

        public bool Add(Knjiga? knjiga, List<string>? brojeviLicnihKarata)
        {
            if (!KnjigaValidator.IsValid(knjiga) || brojeviLicnihKarata == null
                || brojeviLicnihKarata.Any(id => !AutorValidator.IsValidIdentityCard(id))) return false;
            try { _service.DodajKnjigu(knjiga!, brojeviLicnihKarata); return true; }
            catch (ArgumentException) { return false; }
            catch (InvalidOperationException) { return false; }
        }

        public bool Update(string isbn, Knjiga? knjiga)
        {
            if (!KnjigaValidator.IsValidISBN(isbn) || !KnjigaValidator.IsValid(knjiga)
                || !EntityComparison.SameISBN(isbn, knjiga!.ISBN) || _dao.GetByISBN(isbn) == null) return false;
            try { _dao.Update(knjiga); return true; }
            catch (ArgumentException) { return false; }
            catch (InvalidOperationException) { return false; }
        }

        public bool Remove(string isbn)
        {
            if (!KnjigaValidator.IsValidISBN(isbn) || _dao.GetByISBN(isbn) == null) return false;
            try { _service.ObrisiKnjigu(isbn); return true; }
            catch (InvalidOperationException) { return false; }
        }

        public Knjiga? GetByISBN(string isbn) => _dao.GetByISBN(isbn);
        public List<Knjiga> GetAll() => _dao.GetAll();
        public List<Autor> GetAutoriKnjige(string isbn) => _service.GetAutoriKnjige(isbn);
        public List<string> GetAutoriZaKnjigu(string isbn) => _service.GetAutoriZaKnjigu(isbn);
        public bool AutorPostoji(string id) => AutorValidator.IsValidIdentityCard(id) && _service.AutorPostoji(id);

        public bool UkloniAutoraSaKnjige(string brojLicneKarte, string isbn)
        {
            if (!AutorValidator.IsValidIdentityCard(brojLicneKarte) || !KnjigaValidator.IsValidISBN(isbn)) return false;
            try { _service.UkloniAutoraSaKnjige(brojLicneKarte, isbn); return true; }
            catch (InvalidOperationException) { return false; }
        }

        public bool DodajAutoraNaKnjigu(string brojLicneKarte, string isbn)
        {
            if (!AutorValidator.IsValidIdentityCard(brojLicneKarte) || !KnjigaValidator.IsValidISBN(isbn)) return false;
            try { _service.DodajAutoraNaKnjigu(brojLicneKarte, isbn); return true; }
            catch (ArgumentException) { return false; }
            catch (InvalidOperationException) { return false; }
        }
    }
}
