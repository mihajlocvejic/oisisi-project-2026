using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Validation;

namespace Core.Controllers
{
    public class PosetilacController
    {
        private readonly PosetilacDAO _dao;
        private readonly KupovinaDAO _kupovinaDAO;

        public PosetilacController(
            PosetilacDAO dao,
            KupovinaDAO kupovinaDAO)
        {
            _dao = dao;
            _kupovinaDAO = kupovinaDAO;
        }

        public bool Add(Posetilac posetilac)
        {
            if (!PosetilacValidator.IsValid(posetilac))
                return false;

            return _dao.Add(posetilac);
        }

        public bool Update(string brojClanskeKarte, Posetilac posetilac)
        {
            if (!PosetilacValidator.IsValid(posetilac))
                return false;

            return _dao.Update(brojClanskeKarte, posetilac);
        }

        public bool Remove(string brojClanskeKarte)
        {
            if (_kupovinaDAO.GetByPosetilac(brojClanskeKarte).Count > 0)
                return false;

            return _dao.Remove(brojClanskeKarte);
        }

        public Posetilac? GetById(string brojClanskeKarte)
        {
            return _dao.GetById(brojClanskeKarte);
        }

        public List<Posetilac> GetAll()
        {
            return _dao.GetAll();
        }
    }
}