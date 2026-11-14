using Core.DAO;
using Core.Models;
using System.Collections.Generic;

namespace Core.Controllers
{
    public class PosetilacController
    {
        private readonly PosetilacDAO _dao;

        public PosetilacController(string fileName)
        {
            _dao = new PosetilacDAO(fileName);
        }

        public bool Add(Posetilac posetilac)
        {
            return _dao.Add(posetilac);
        }

        public bool Update(string brojClanskeKarte, Posetilac posetilac)
        {
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