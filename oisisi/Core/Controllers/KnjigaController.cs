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
        private readonly IzdavacDAO _izdavacDAO;
        private readonly KupovinaDAO _kupovinaDAO;
        private readonly PosetilacDAO _posetilacDAO;

        public KnjigaController(
            KnjigaDAO dao,
            AutorKnjigaService service,
            KupovinaDAO kupovinaDAO,
            PosetilacDAO posetilacDAO,
            IzdavacDAO izdavacDAO)
        {
            ArgumentNullException.ThrowIfNull(dao);
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(kupovinaDAO);
            ArgumentNullException.ThrowIfNull(posetilacDAO);
            ArgumentNullException.ThrowIfNull(izdavacDAO);

            _dao = dao;
            _service = service;
            _kupovinaDAO = kupovinaDAO;
            _posetilacDAO = posetilacDAO;
            _izdavacDAO = izdavacDAO;
        }

        public bool Add(Knjiga? knjiga, List<string>? brojeviLicnihKarata)
        {
            if (!KnjigaValidator.IsValid(knjiga)
                || brojeviLicnihKarata == null
                || brojeviLicnihKarata.Any(id =>
                    !AutorValidator.IsValidIdentityCard(id)))
                return false;

            if (_izdavacDAO.GetById(knjiga!.SifraIzdavaca) == null)
                return false;

            try
            {
                _service.DodajKnjigu(knjiga!, brojeviLicnihKarata);
                RefreshPublisherBooks();
                return true;
            }
            catch (ArgumentException) { return false; }
            catch (InvalidOperationException) { return false; }
        }

        public bool Update(string isbn, Knjiga? knjiga)
        {
            if (!KnjigaValidator.IsValidISBN(isbn)
                || !KnjigaValidator.IsValid(knjiga)
                || !EntityComparison.SameISBN(isbn, knjiga!.ISBN)
                || _dao.GetByISBN(isbn) == null)
                return false;

            if (_izdavacDAO.GetById(knjiga!.SifraIzdavaca) == null)
                return false;

            try
            {
                _dao.Update(knjiga!);
                RefreshPublisherBooks();
                return true;
            }
            catch (ArgumentException) { return false; }
            catch (InvalidOperationException) { return false; }
        }

        public bool Remove(string isbn)
        {
            if (!KnjigaValidator.IsValidISBN(isbn)
                || _dao.GetByISBN(isbn) == null)
                return false;

            try
            {
                _service.ObrisiKnjigu(isbn);

                _kupovinaDAO.RemoveByISBN(isbn);

                foreach (Posetilac posetilac in _posetilacDAO.GetAll())
                {
                    posetilac.ListaZeljaISBN.RemoveAll(k =>
                        EntityComparison.SameISBN(k, isbn));

                    posetilac.Kupovine = _kupovinaDAO.GetByPosetilac(
                        posetilac.BrojClanskeKarte);

                    posetilac.IzracunajProsecnuOcenu();
                }

                _posetilacDAO.Save();

                RefreshPublisherBooks();

                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
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

        private void RefreshPublisherBooks()
        {
            foreach (Izdavac izdavac in _izdavacDAO.GetAll())
            {
                izdavac.Knjige = _dao.GetAll()
                    .Where(k => EntityComparison.SameIdentifier(
                        k.SifraIzdavaca, izdavac.Sifra))
                    .ToList();
            }
        }
    }
}
