using System.Collections.Generic;
using Core.DAO;
using Core.Models;
using Core.Validation;

namespace Core.Controllers
{
    public class KupovinaController
    {
        private readonly KupovinaDAO _dao;

        public KupovinaController(string fileName)
        {
            _dao = new KupovinaDAO(fileName);
        }

        public bool Add(Kupovina kupovina)
        {
            if (!KupovinaValidator.IsValid(kupovina))
                return false;

            _dao.Add(kupovina);

            return true;
        }

        public bool Update(int id, Kupovina kupovina)
        {
            if (!KupovinaValidator.IsValid(kupovina))
                return false;

            return _dao.Update(id, kupovina);
        }

        public bool Remove(int id)
        {
            return _dao.Remove(id);
        }

        public List<Kupovina> GetAll()
        {
            return _dao.GetAll();
        }
    }
}