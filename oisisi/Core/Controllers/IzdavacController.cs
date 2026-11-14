using System.Collections.Generic;
using Core.DAO;
using Core.Models;

namespace Core.Controllers
{
    public class IzdavacController
    {
        private readonly IzdavacDAO _dao;

        public IzdavacController(string fileName)
        {
            _dao = new IzdavacDAO(fileName);
        }

        private bool IsValid(Izdavac izdavac)
        {
            return IsValidText(izdavac.Sifra)
                && IsValidText(izdavac.Naziv);
        }

        private bool IsValidText(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                && !value.Contains('|')
                && !value.Contains('\n')
                && !value.Contains('\r');
        }

        public bool Add(Izdavac izdavac)
        {
            if (!IsValid(izdavac))
                return false;

            return _dao.Add(izdavac);
        }

        public bool Update(string sifra, Izdavac izdavac)
        {
            if (!IsValid(izdavac))
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