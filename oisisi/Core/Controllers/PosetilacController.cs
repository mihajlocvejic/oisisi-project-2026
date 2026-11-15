using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Validation;

namespace Core.Controllers
{
    public class PosetilacController
    {
        private readonly PosetilacDAO _dao;

        public PosetilacController(string fileName, AdresaDAO? adresaDAO = null)
        {
            _dao = new PosetilacDAO(fileName, adresaDAO);
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
            return _dao.Remove(brojClanskeKarte);
        }

        public List<Posetilac> GetAll()
        {
            return _dao.GetAll();
        }
    }
}