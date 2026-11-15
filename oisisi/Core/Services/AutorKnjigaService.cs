using Core.DAO;
using Core.Models;
using Core.Utils;
using Core.Validation;

namespace Core.Services
{
    public class AutorKnjigaService
    {
        private readonly AutorDAO _autorDAO;
        private readonly KnjigaDAO _knjigaDAO;
        private readonly AutorKnjigaDAO _autorKnjigaDAO;

        public AutorKnjigaService(AutorDAO autorDAO, KnjigaDAO knjigaDAO, AutorKnjigaDAO autorKnjigaDAO)
        {
            ArgumentNullException.ThrowIfNull(autorDAO);
            ArgumentNullException.ThrowIfNull(knjigaDAO);
            ArgumentNullException.ThrowIfNull(autorKnjigaDAO);
            _autorDAO = autorDAO;
            _knjigaDAO = knjigaDAO;
            _autorKnjigaDAO = autorKnjigaDAO;
        }

        public bool AutorPostoji(string brojLicneKarte) =>
            AutorValidator.IsValidIdentityCard(brojLicneKarte) && _autorDAO.GetById(brojLicneKarte) != null;

        public void DodajAutoraNaKnjigu(string brojLicneKarte, string isbn)
        {
            AutorKnjigaValidator.Validate(new AutorKnjiga(brojLicneKarte, isbn));
            Autor autor = _autorDAO.GetById(brojLicneKarte)
                ?? throw new InvalidOperationException("Autor ne postoji.");
            Knjiga knjiga = _knjigaDAO.GetByISBN(isbn)
                ?? throw new InvalidOperationException("Knjiga ne postoji.");
            if (_autorKnjigaDAO.Exists(autor.BrojLicneKarte, knjiga.ISBN))
                throw new InvalidOperationException("Autor je vec povezan sa ovom knjigom.");
            _autorKnjigaDAO.Add(autor.BrojLicneKarte, knjiga.ISBN);
        }

        public void UkloniAutoraSaKnjige(string brojLicneKarte, string isbn)
        {
            AutorKnjigaValidator.Validate(new AutorKnjiga(brojLicneKarte, isbn));
            Knjiga knjiga = _knjigaDAO.GetByISBN(isbn)
                ?? throw new InvalidOperationException("Knjiga ne postoji.");
            if (!_autorKnjigaDAO.Exists(brojLicneKarte, knjiga.ISBN))
                throw new InvalidOperationException("Veza autora i knjige ne postoji.");
            // Knjiga moze ostati bez autora.
            _autorKnjigaDAO.Remove(brojLicneKarte, knjiga.ISBN);
        }

        public List<string> GetAutoriZaKnjigu(string isbn) => _autorKnjigaDAO.GetAutoriZaKnjigu(isbn);
        public List<string> GetKnjigeZaAutora(string brojLicneKarte) => _autorKnjigaDAO.GetKnjigeZaAutora(brojLicneKarte);

        public void DodajKnjigu(Knjiga knjiga, List<string> brojeviLicnihKarata)
        {
            ArgumentNullException.ThrowIfNull(knjiga);
            ArgumentNullException.ThrowIfNull(brojeviLicnihKarata);
            KnjigaValidator.Validate(knjiga);
            if (brojeviLicnihKarata.Any(id => !AutorValidator.IsValidIdentityCard(id)))
                throw new ArgumentException("Broj licne karte autora nije ispravan.");

            // Svi autori se pronalaze pre upisa; u veze ulaze njihovi sacuvani identifikatori.
            List<Autor> autori = brojeviLicnihKarata.DistinctBy(EntityComparison.IdentifierKey)
                .Select(id => _autorDAO.GetById(id)
                    ?? throw new InvalidOperationException($"Autor sa licnom kartom {id} ne postoji."))
                .ToList();

            // Prazna lista autora je dozvoljena.
            _knjigaDAO.Add(knjiga);
            List<Action> undo = new() { () => _knjigaDAO.Remove(knjiga.ISBN) };
            try
            {
                foreach (Autor autor in autori)
                {
                    _autorKnjigaDAO.Add(autor.BrojLicneKarte, knjiga.ISBN);
                    undo.Add(() => _autorKnjigaDAO.Remove(autor.BrojLicneKarte, knjiga.ISBN));
                }
            }
            catch (Exception original)
            {
                Rollback(original, undo);
                throw;
            }
        }

        public void ObrisiKnjigu(string isbn)
        {
            Knjiga knjiga = _knjigaDAO.GetByISBN(isbn)
                ?? throw new InvalidOperationException("Knjiga ne postoji.");
            List<AutorKnjiga> veze = _autorKnjigaDAO.GetAll()
                .Where(v => EntityComparison.SameISBN(v.ISBN, knjiga.ISBN)).ToList();
            List<Action> undo = new();
            try
            {
                foreach (AutorKnjiga veza in veze)
                {
                    _autorKnjigaDAO.Remove(veza.BrojLicneKarte, veza.ISBN);
                    undo.Add(() => _autorKnjigaDAO.Add(veza.BrojLicneKarte, veza.ISBN));
                }
                _knjigaDAO.Remove(knjiga.ISBN);
            }
            catch (Exception original)
            {
                Rollback(original, undo);
                throw;
            }
        }

        public List<Autor> GetAutoriKnjige(string isbn)
        {
            List<Autor> autori = new();
            foreach (string id in _autorKnjigaDAO.GetAutoriZaKnjigu(isbn))
            {
                Autor? autor = _autorDAO.GetById(id);
                if (autor != null) autori.Add(autor);
            }
            return autori;
        }

        public void ObrisiAutora(string brojLicneKarte)
        {
            Autor autor = _autorDAO.GetById(brojLicneKarte)
                ?? throw new InvalidOperationException("Autor ne postoji.");
            List<AutorKnjiga> veze = _autorKnjigaDAO.GetAll()
                .Where(v => EntityComparison.SameIdentifier(v.BrojLicneKarte, autor.BrojLicneKarte)).ToList();
            List<Action> undo = new();
            try
            {
                foreach (AutorKnjiga veza in veze)
                {
                    _autorKnjigaDAO.Remove(veza.BrojLicneKarte, veza.ISBN);
                    undo.Add(() => _autorKnjigaDAO.Add(veza.BrojLicneKarte, veza.ISBN));
                }
                _autorDAO.Remove(autor.BrojLicneKarte);
            }
            catch (Exception original)
            {
                Rollback(original, undo);
                throw;
            }
        }

        private static void Rollback(Exception original, List<Action> undo)
        {
            List<Exception> errors = new();
            // Svaki uspesan korak ima svoju suprotnu operaciju; pokusavamo sve obrnutim redom.
            for (int i = undo.Count - 1; i >= 0; i--)
            {
                try { undo[i](); }
                catch (Exception rollbackError) { errors.Add(rollbackError); }
            }
            // Zadrzavamo originalni izuzetak i njegov stack trace; oporavak nije transakcija.
            if (errors.Count > 0) original.Data["AutorKnjigaRollbackErrors"] = errors.ToArray();
        }
    }
}
