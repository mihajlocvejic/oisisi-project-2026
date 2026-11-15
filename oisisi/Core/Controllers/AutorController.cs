using Core.DAO;
using Core.Models;
using Core.Services;
using Core.Utils;
using Core.Validation;

namespace Core.Controllers
{
    public class AutorController
    {
        private readonly AutorDAO _dao;
        private readonly AutorKnjigaService _service;

        public AutorController(AutorDAO dao, AutorKnjigaService service)
        {
            ArgumentNullException.ThrowIfNull(dao);
            ArgumentNullException.ThrowIfNull(service);
            _dao = dao;
            _service = service;
        }

        public bool Add(Autor? autor)
        {
            if (!AutorValidator.IsValid(autor)) return false;
            try { _dao.Add(autor!); return true; }
            catch (ArgumentException) { return false; }
            catch (InvalidOperationException) { return false; }
        }

        public bool Update(string brojLicneKarte, Autor? autor)
        {
            if (!AutorValidator.IsValidIdentityCard(brojLicneKarte)
                || !AutorValidator.IsValid(autor)
                || !EntityComparison.SameIdentifier(brojLicneKarte, autor!.BrojLicneKarte)
                || _dao.GetById(brojLicneKarte) == null) return false;
            try { _dao.Update(autor); return true; }
            catch (ArgumentException) { return false; }
            catch (InvalidOperationException) { return false; }
        }

        public bool Remove(string brojLicneKarte)
        {
            if (!AutorValidator.IsValidIdentityCard(brojLicneKarte)
                || _dao.GetById(brojLicneKarte) == null) return false;
            try { _service.ObrisiAutora(brojLicneKarte); return true; }
            catch (InvalidOperationException) { return false; }
        }

        public Autor? GetById(string brojLicneKarte) =>
            AutorValidator.IsValidIdentityCard(brojLicneKarte) ? _dao.GetById(brojLicneKarte) : null;

        public List<Autor> GetAll() => _dao.GetAll();
    }
}
