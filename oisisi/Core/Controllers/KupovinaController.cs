using System.Collections.Generic;
using Core.DAO;
using Core.Models;

namespace Core.Controllers
{
    public class KupovinaController
    {
        private readonly KupovinaDAO _dao;

        public KupovinaController(string fileName)
        {
            _dao = new KupovinaDAO(fileName);
        }

        public void Add(Kupovina kupovina)
        {
            _dao.Add(kupovina);
        }

        public bool Update(int id, Kupovina kupovina)
        {
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