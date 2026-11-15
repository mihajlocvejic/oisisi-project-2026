using Core.DAO;
using Core.Models;
using Core.Validation;

namespace Core.Controllers
{
    public class AdresaController
    {
        private readonly AdresaDAO _dao;
        private readonly AutorDAO _autori;
        private readonly PosetilacController _posetioci;

        public AdresaController(AdresaDAO dao, AutorDAO autori, PosetilacController posetioci)
        {
            ArgumentNullException.ThrowIfNull(dao);
            ArgumentNullException.ThrowIfNull(autori);
            ArgumentNullException.ThrowIfNull(posetioci);
            _dao = dao;
            _autori = autori;
            _posetioci = posetioci;
        }

        public List<Adresa> GetAll() => _dao.GetAll();
        public Adresa? GetById(int id) => id > 0 ? _dao.GetById(id) : null;

        public bool Add(Adresa? adresa) => Add(adresa, out _, out _);

        public bool Add(Adresa? adresa, out Adresa? created, out string error)
        {
            created = null;
            if (!AdresaValidator.IsValid(adresa))
            {
                error = "Sva polja adrese su obavezna i ne smeju sadrzati | ili novi red.";
                return false;
            }
            try
            {
                created = _dao.Add(adresa!);
                error = string.Empty;
                return true;
            }
            catch (ArgumentException ex) { error = ex.Message; return false; }
            catch (InvalidOperationException ex) { error = ex.Message; return false; }
        }

        public bool Update(int id, Adresa? adresa) => Update(id, adresa, out _);

        public bool Update(int id, Adresa? adresa, out string error)
        {
            if (id <= 0 || !AdresaValidator.IsValid(adresa))
            {
                error = "Unesite pozitivan ID i sva polja adrese bez | ili novih redova.";
                return false;
            }
            try
            {
                if (!_dao.Update(id, adresa!))
                {
                    error = "Adresa nije pronadjena.";
                    return false;
                }
                error = string.Empty;
                return true;
            }
            catch (ArgumentException ex) { error = ex.Message; return false; }
            catch (InvalidOperationException ex) { error = ex.Message; return false; }
        }

        public bool Remove(int id) => Remove(id, out _);

        public bool Remove(int id, out string error)
        {
            if (id <= 0)
            {
                error = "ID mora biti pozitivan ceo broj.";
                return false;
            }
            if (_dao.GetById(id) == null)
            {
                error = "Adresa nije pronadjena.";
                return false;
            }
            if (_autori.GetAll().Any(a => a.Adresa.Id == id)
                || _posetioci.GetAll().Any(p => p.Adresa.Id == id))
            {
                error = "Adresa se koristi. Prvo promenite adresu osobe ili obrisite osobu.";
                return false;
            }
            bool removed = _dao.Remove(id);
            error = removed ? string.Empty : "Adresa nije pronadjena.";
            return removed;
        }
    }
}
