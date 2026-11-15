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
    }
}
