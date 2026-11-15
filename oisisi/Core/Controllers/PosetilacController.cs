using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Validation;
using Core.Utils;

namespace Core.Controllers
{
    public class PosetilacController
    {
        private readonly PosetilacDAO _dao;
        private readonly KupovinaDAO _kupovinaDAO;
        private readonly KnjigaDAO _knjigaDAO;

        public PosetilacController(
            PosetilacDAO dao,
            KupovinaDAO kupovinaDAO,
            KnjigaDAO knjigaDAO)
        {
            _dao = dao;
            _kupovinaDAO = kupovinaDAO;
            _knjigaDAO = knjigaDAO;
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
            Posetilac? posetilac = _dao.GetById(brojClanskeKarte);

            if (posetilac == null)
                return false;

            _kupovinaDAO.RemoveByPosetilac(brojClanskeKarte);

            foreach (Knjiga knjiga in _knjigaDAO.GetAll())
            {
                knjiga.KupciClanskeKarte.RemoveAll(k =>
                    EntityComparison.SameIdentifier(k, brojClanskeKarte));

                knjiga.ListaZeljaClanskeKarte.RemoveAll(k =>
                    EntityComparison.SameIdentifier(k, brojClanskeKarte));
            }

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