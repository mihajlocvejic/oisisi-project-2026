using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Validation;

namespace Core.Controllers
{
    public class IzdavacController
    {
        private readonly IzdavacDAO _dao;

        public IzdavacController(string fileName)
        {
            _dao = new IzdavacDAO(fileName);
        }

        public bool Add(Izdavac izdavac)
        {
            if (!IzdavacValidator.IsValid(izdavac))
                return false;

            return _dao.Add(izdavac);
        }

        public bool Update(string sifra, Izdavac izdavac)
        {
            if (!IzdavacValidator.IsValid(izdavac))
                return false;

            return _dao.Update(sifra, izdavac);
        }

        public bool Remove(string sifra)
        {
            return _dao.Remove(sifra);
        }

        public List<Izdavac> GetAll()
        {
            return _dao.GetAll();
        }
    }
}