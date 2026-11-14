
using Core.DAO;
using Core.Models;
using System.Collections.Generic;

namespace Core.Controllers
{
    public class KnjigaController
    {
        private readonly KnjigaDAO _dao;

        public KnjigaController(string fileName)
        {
            _dao = new KnjigaDAO(fileName);
        }

        public void Add(Knjiga knjiga)
        {
            _dao.Add(knjiga);
        }

        public bool Update(string isbn, Knjiga knjiga)
        {
            Knjiga? existing = _dao.GetByISBN(isbn);

            if (existing == null)
                return false;

            // ISBN je jedinstveni identifikator knjige.
            knjiga.ISBN = isbn;

            _dao.Update(knjiga);
            return true;
        }

        public bool Remove(string isbn)
        {
            if (_dao.GetByISBN(isbn) == null)
                return false;

            _dao.Delete(isbn);
            return true;
        }

        public Knjiga? GetByISBN(string isbn)
        {
            return _dao.GetByISBN(isbn);
        }

        public List<Knjiga> GetAll()
        {
            return _dao.GetAll();
        }
    }
}
