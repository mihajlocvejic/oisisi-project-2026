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

        public bool Add(Kupovina kupovina)
        {
            if (!IsValid(kupovina))
                return false;

            _dao.Add(kupovina);

            return true;
        }

        public bool Update(int id, Kupovina kupovina)
        {
            if (!IsValid(kupovina))
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

        private bool IsValid(Kupovina kupovina)
        {
            return !string.IsNullOrWhiteSpace(kupovina.BrojClanskeKarte)
                && !string.IsNullOrWhiteSpace(kupovina.ISBN)
                && kupovina.DatumKupovine <= DateTime.Today
                && kupovina.DatumKupovine != DateTime.MinValue
                && kupovina.Ocena >= 1
                && kupovina.Ocena <= 5
                && IsValidText(kupovina.BrojClanskeKarte)
                && IsValidText(kupovina.ISBN)
                && IsValidText(kupovina.Komentar);
        }

        private bool IsValidText(string value)
        {
            return value != null
                && !value.Contains('|')
                && !value.Contains('\n')
                && !value.Contains('\r');
        }
    }
}