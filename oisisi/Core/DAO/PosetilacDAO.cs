using System.Collections.Generic;
using Core.Models;
using Core.Serialization;
using Core.Utils;
using System.Linq;

namespace Core.DAO
{
    public class PosetilacDAO
    {
        private readonly AdresaDAO _adresaDAO;
        private readonly List<Posetilac> _posetioci;
        private readonly Serializer<Posetilac> _serializer;
        private readonly string _fileName;

        public PosetilacDAO(string fileName, AdresaDAO adresaDAO)
        {
            _fileName = fileName;
            _serializer = new Serializer<Posetilac>();
            _adresaDAO = adresaDAO;
            _posetioci = _serializer.FromCSV(_fileName);

            foreach (Posetilac posetilac in _posetioci)
            {
                posetilac.Adresa = _adresaDAO.GetById(posetilac.Adresa.Id)
                    ?? throw new FormatException("Povezana adresa ne postoji.");
            }
        }

        public bool Add(Posetilac posetilac)
        {
            if (GetById(posetilac.BrojClanskeKarte) != null)
                return false;

            Adresa? adresa = _adresaDAO.GetById(posetilac.Adresa.Id);

            if (adresa == null)
                return false;

            posetilac.Adresa = adresa;

            _posetioci.Add(posetilac);
            Save();

            return true;
        }

        public bool Update(string brojClanskeKarte, Posetilac posetilac)
        {
            Posetilac? postojeci = GetById(brojClanskeKarte);

            if (postojeci == null)
                return false;

            if (postojeci.BrojClanskeKarte != posetilac.BrojClanskeKarte)
                return false;

            Adresa? adresa = _adresaDAO.GetById(posetilac.Adresa.Id);

            if (adresa == null)
                return false;

            postojeci.Ime = posetilac.Ime;
            postojeci.Prezime = posetilac.Prezime;
            postojeci.DatumRodjenja = posetilac.DatumRodjenja;
            postojeci.Adresa = adresa;
            postojeci.KontaktTelefon = posetilac.KontaktTelefon;
            postojeci.Email = posetilac.Email;
            postojeci.GodinaClanstva = posetilac.GodinaClanstva;
            postojeci.Status = posetilac.Status;

            Save();

            return true;
        }

        public bool Remove(string brojClanskeKarte)
        {
            Posetilac? posetilac = GetById(brojClanskeKarte);

            if (posetilac == null)
                return false;

            _posetioci.Remove(posetilac);
            Save();

            return true;
        }

        public List<Posetilac> GetAll()
        {
            return new List<Posetilac>(_posetioci);
        }

        public Posetilac? GetById(string brojClanskeKarte)
        {
            return _posetioci.FirstOrDefault(p =>
                EntityComparison.SameIdentifier(
                    p.BrojClanskeKarte, brojClanskeKarte));
        }

        public void Save()
        {
            _serializer.ToCSV(_fileName, _posetioci);
        }
    }
}