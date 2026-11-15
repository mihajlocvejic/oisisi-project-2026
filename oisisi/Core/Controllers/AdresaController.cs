using Core.DAO;
using Core.Models;

namespace Core.Controllers
{
    public class AdresaController
    {
        private readonly AdresaDAO _dao;
        private readonly AutorDAO _autori;
        private readonly PosetilacController _posetioci;

        public AdresaController(AdresaDAO dao, AutorDAO autori, PosetilacController posetioci)
        {
            _dao = dao;
            _autori = autori;
            _posetioci = posetioci;
        }

        public List<Adresa> GetAll() => _dao.GetAll();
        public Adresa? GetById(int id) => _dao.GetById(id);
        public Adresa Add(Adresa adresa) => _dao.Add(adresa);
        public bool Update(int id, Adresa adresa) => _dao.Update(id, adresa);

        public bool Remove(int id)
        {
            if (_autori.GetAll().Any(a => a.Adresa.Id == id)
                || _posetioci.GetAll().Any(p => p.Adresa.Id == id))
                throw new InvalidOperationException("Adresa se koristi. Prvo promenite adresu osobe ili obrisite osobu.");
            return _dao.Remove(id);
        }
        // Ocekivani neuspesi dostupni su i konzoli i buducem WPF klijentu.
        // Postojece CRUD metode ostaju dostupne radi kompatibilnosti.
        public bool TryAdd(Adresa? adresa, out Adresa? created, out string error)
        {
            created = null;
            if (adresa == null)
            {
                error = "Adresa je obavezna.";
                return false;
            }
            try
            {
                created = Add(adresa);
                error = string.Empty;
                return true;
            }
            catch (ArgumentException ex) { error = ex.Message; return false; }
            catch (InvalidOperationException ex) { error = ex.Message; return false; }
        }

        public bool TryUpdate(int id, Adresa? adresa, out string error)
        {
            if (id <= 0 || adresa == null)
            {
                error = "Unesite pozitivan ID i podatke adrese.";
                return false;
            }
            try
            {
                if (!Update(id, adresa))
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

        public bool TryRemove(int id, out string error)
        {
            if (id <= 0)
            {
                error = "ID mora biti pozitivan ceo broj.";
                return false;
            }
            try
            {
                if (!Remove(id))
                {
                    error = "Adresa nije pronadjena.";
                    return false;
                }
                error = string.Empty;
                return true;
            }
            catch (InvalidOperationException ex) { error = ex.Message; return false; }
        }
    }
}
