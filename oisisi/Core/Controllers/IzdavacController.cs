using Core.DAO;
using Core.Models;
using Core.Utils;
using Core.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Core.Controllers
{
    public class IzdavacController
    {
        private readonly IzdavacDAO _dao;
        private readonly AutorDAO _autorDAO;
        private readonly KnjigaDAO _knjigaDAO;

        public IzdavacController(
            IzdavacDAO dao,
            AutorDAO autorDAO,
            KnjigaDAO knjigaDAO)
        {
            _dao = dao;
            _autorDAO = autorDAO;
            _knjigaDAO = knjigaDAO;
        }

        public bool Add(Izdavac izdavac)
        {
            if (!IzdavacValidator.IsValid(izdavac))
                return false;

            if (!ValidateRelations(izdavac))
                return false;

            return _dao.Add(izdavac);
        }

        public bool Update(string sifra, Izdavac izdavac)
        {
            if (!IzdavacValidator.IsValid(izdavac))
                return false;

            if (!ValidateRelations(izdavac))
                return false;

            return _dao.Update(sifra, izdavac);
        }

        public bool Remove(string sifra)
        {
            if (_knjigaDAO.GetAll().Any(k =>
                EntityComparison.SameIdentifier(k.SifraIzdavaca, sifra)))
                return false;

            return _dao.Remove(sifra);
        }

        private bool ValidateRelations(Izdavac izdavac)
        {
            List<Autor> autori = new List<Autor>();

            foreach (Autor autor in izdavac.Autori)
            {
                Autor? postojeci = _autorDAO.GetById(autor.BrojLicneKarte);

                if (postojeci == null)
                    return false;

                if (autori.Any(a => EntityComparison.SameIdentifier(
                    a.BrojLicneKarte, postojeci.BrojLicneKarte)))
                    return false;

                autori.Add(postojeci);
            }

            Autor? sef = null;

            if (izdavac.SefIzdavaca != null)
            {
                sef = _autorDAO.GetById(
                    izdavac.SefIzdavaca.BrojLicneKarte);

                if (sef == null || sef.GodineIskustva < 5)
                    return false;

                if (!autori.Contains(sef))
                    return false;
            }

            izdavac.Autori = autori;
            izdavac.SefIzdavaca = sef;

            return true;
        }

        public List<Izdavac> GetAll()
        {
            List<Izdavac> izdavaci = _dao.GetAll();

            foreach (Izdavac izdavac in izdavaci)
            {
                izdavac.Knjige = GetKnjigeIzdavaca(izdavac.Sifra);
            }

            return izdavaci;
        }

        public Izdavac? GetById(string sifra)
        {
            Izdavac? izdavac = _dao.GetById(sifra);

            if (izdavac != null)
            {
                izdavac.Knjige = GetKnjigeIzdavaca(izdavac.Sifra);
            }

            return izdavac;
        }

        public Autor? GetAutorById(string brojLicneKarte)
        {
            return _autorDAO.GetById(brojLicneKarte);
        }

        public List<Autor> GetAllAutori()
        {
            return _autorDAO.GetAll();
        }

        public List<Knjiga> GetKnjigeIzdavaca(string sifra)
        {
            return _knjigaDAO.GetAll()
                .Where(k => EntityComparison.SameIdentifier(
                    k.SifraIzdavaca, sifra))
                .ToList();
        }
    }
}