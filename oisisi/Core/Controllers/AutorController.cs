
using Core.DAO;
using Core.Models;
using System.Collections.Generic;

namespace Core.Controllers
{
    public class AutorController
    {
        private readonly AutorDAO _dao;

        public AutorController(string fileName)
        {
            _dao = new AutorDAO(fileName);
        }

        public void Add(Autor autor)
        {
            _dao.Add(autor);
        }

        public bool Update(string brojLicneKarte, Autor autor)
        {
            Autor? existing = _dao.GetById(brojLicneKarte);

            if (existing == null)
                return false;

            // Trenutni DAO pronalazi autora po broju licne karte.
            // Zato taj identifikator za sada ne menjamo.
            autor.BrojLicneKarte = brojLicneKarte;

            _dao.Update(autor);
            return true;
        }

        public bool Remove(string brojLicneKarte)
        {
            if (_dao.GetById(brojLicneKarte) == null)
                return false;

            _dao.Delete(brojLicneKarte);
            return true;
        }

        public Autor? GetById(string brojLicneKarte)
        {
            return _dao.GetById(brojLicneKarte);
        }

        public List<Autor> GetAll()
        {
            return _dao.GetAll();
        }
    }
}
