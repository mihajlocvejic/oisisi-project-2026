using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Validation;

namespace Core.Controllers
{
    public class KupovinaController
    {
        private readonly KupovinaDAO _dao;
        private readonly PosetilacDAO _posetilacDAO;
        private readonly KnjigaDAO _knjigaDAO;

        public KupovinaController(
            KupovinaDAO dao,
            PosetilacDAO posetilacDAO,
            KnjigaDAO knjigaDAO)
        {
            _dao = dao;
            _posetilacDAO = posetilacDAO;
            _knjigaDAO = knjigaDAO;
        }

        public bool Add(Kupovina kupovina)
        {
            if (!KupovinaValidator.IsValid(kupovina))
                return false;

            Posetilac? posetilac = _posetilacDAO.GetById(
                kupovina.BrojClanskeKarte);

            Knjiga? knjiga = _knjigaDAO.GetByISBN(kupovina.ISBN);

            if (posetilac == null || knjiga == null)
                return false;

            kupovina.BrojClanskeKarte = posetilac.BrojClanskeKarte;
            kupovina.ISBN = knjiga.ISBN;

            _dao.Add(kupovina);

            posetilac.Kupovine = _dao.GetByPosetilac(
                posetilac.BrojClanskeKarte);

            posetilac.IzracunajProsecnuOcenu();
            _posetilacDAO.Save();

            return true;
        }

        public bool Update(int id, Kupovina kupovina)
        {
            if (!KupovinaValidator.IsValid(kupovina))
                return false;

            Kupovina? postojeca = _dao.GetById(id);

            if (postojeca == null)
                return false;

            Posetilac? stariPosetilac = _posetilacDAO.GetById(
                postojeca.BrojClanskeKarte);

            Posetilac? noviPosetilac = _posetilacDAO.GetById(
                kupovina.BrojClanskeKarte);

            Knjiga? knjiga = _knjigaDAO.GetByISBN(kupovina.ISBN);

            if (stariPosetilac == null || noviPosetilac == null || knjiga == null)
                return false;

            kupovina.BrojClanskeKarte = noviPosetilac.BrojClanskeKarte;
            kupovina.ISBN = knjiga.ISBN;

            if (!_dao.Update(id, kupovina))
                return false;

            stariPosetilac.Kupovine = _dao.GetByPosetilac(
                stariPosetilac.BrojClanskeKarte);

            stariPosetilac.IzracunajProsecnuOcenu();

            if (stariPosetilac != noviPosetilac)
            {
                noviPosetilac.Kupovine = _dao.GetByPosetilac(
                    noviPosetilac.BrojClanskeKarte);

                noviPosetilac.IzracunajProsecnuOcenu();
            }

            _posetilacDAO.Save();

            return true;
        }

        public bool Remove(int id)
        {
            Kupovina? kupovina = _dao.GetById(id);

            if (kupovina == null)
                return false;

            Posetilac? posetilac = _posetilacDAO.GetById(
                kupovina.BrojClanskeKarte);

            if (posetilac == null)
                return false;

            if (!_dao.Remove(id))
                return false;

            posetilac.Kupovine = _dao.GetByPosetilac(
                posetilac.BrojClanskeKarte);

            posetilac.IzracunajProsecnuOcenu();

            _posetilacDAO.Save();

            return true;
        }

        public List<Kupovina> GetAll()
        {
            return _dao.GetAll();
        }

        public Kupovina? GetById(int id)
        {
            return _dao.GetById(id);
        }

        public bool PosetilacExists(string brojClanskeKarte)
        {
            return _posetilacDAO.GetById(brojClanskeKarte) != null;
        }

        public bool KnjigaExists(string isbn)
        {
            return _knjigaDAO.GetByISBN(isbn) != null;
        }

        public bool CanAddPurchase()
        {
            return _posetilacDAO.GetAll().Count > 0
                && _knjigaDAO.GetAll().Count > 0;
        }
    }
}