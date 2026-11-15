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
        private readonly IzdavacDAO _izdavacDAO;

        public AutorController(
            AutorDAO dao,
            AutorKnjigaService service,
            IzdavacDAO izdavacDAO)
        {
            ArgumentNullException.ThrowIfNull(dao);
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNull(izdavacDAO);

            _dao = dao;
            _service = service;
            _izdavacDAO = izdavacDAO;
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

            if (IsPublisherChief(brojLicneKarte) && autor!.GodineIskustva < 5)
                return false;

            try { _dao.Update(autor); return true; }
            catch (ArgumentException) { return false; }
            catch (InvalidOperationException) { return false; }
        }

        public bool Remove(string brojLicneKarte)
        {
            if (!AutorValidator.IsValidIdentityCard(brojLicneKarte)
                || _dao.GetById(brojLicneKarte) == null)
                return false;

            try
            {
                _service.ObrisiAutora(brojLicneKarte);

                foreach (Izdavac izdavac in _izdavacDAO.GetAll())
                {
                    izdavac.Autori.RemoveAll(a =>
                        EntityComparison.SameIdentifier(
                            a.BrojLicneKarte, brojLicneKarte));

                    if (izdavac.SefIzdavaca != null &&
                        EntityComparison.SameIdentifier(
                            izdavac.SefIzdavaca.BrojLicneKarte,
                            brojLicneKarte))
                    {
                        izdavac.SefIzdavaca = null;
                    }
                }

                _izdavacDAO.Save();

                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public Autor? GetById(string brojLicneKarte) =>
            AutorValidator.IsValidIdentityCard(brojLicneKarte) ? _dao.GetById(brojLicneKarte) : null;

        public List<Autor> GetAll() => _dao.GetAll();

        private bool IsPublisherChief(string brojLicneKarte)
        {
            return _izdavacDAO.GetAll().Any(i =>
                i.SefIzdavaca != null &&
                EntityComparison.SameIdentifier(
                    i.SefIzdavaca.BrojLicneKarte, brojLicneKarte));
        }
    }
}
