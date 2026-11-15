using System.Collections.Generic;
using System.IO;
using Core.Models;
using Core.Serialization;

namespace Core.DAO
{
    public class PosetilacDAO
    {
        private readonly AdresaDAO _adresaDAO;
        private readonly List<Posetilac> _posetioci;
        private readonly Serializer<Posetilac> _serializer;
        private readonly string _fileName;

        public PosetilacDAO(string fileName, AdresaDAO? adresaDAO = null)
        {
            _fileName = fileName;
            _serializer = new Serializer<Posetilac>();
            _posetioci = _serializer.FromCSV(_fileName);
            _adresaDAO = adresaDAO ?? new AdresaDAO(Path.Combine(Path.GetDirectoryName(Path.GetFullPath(fileName))!, "adrese.csv"));
            foreach (Posetilac item in _posetioci)
                item.Adresa = _adresaDAO.GetById(item.Adresa.Id)
                    ?? throw new FormatException("Povezana adresa ne postoji.");
        }

        public bool Add(Posetilac posetilac)
        {
            if (_posetioci.Exists(p => p.BrojClanskeKarte == posetilac.BrojClanskeKarte))
                return false;

            Adresa? adresa = _adresaDAO.GetById(posetilac.Adresa.Id);
            if (adresa == null) return false;
            posetilac.Adresa = adresa;

            _posetioci.Add(posetilac);
            _serializer.ToCSV(_fileName, _posetioci);

            return true;
        }

        public bool Update(string brojClanskeKarte, Posetilac posetilac)
        {
            int index = _posetioci.FindIndex(p => p.BrojClanskeKarte == brojClanskeKarte);

            if (index == -1)
                return false;

            if (brojClanskeKarte != posetilac.BrojClanskeKarte)
                return false;

            Adresa? adresa = _adresaDAO.GetById(posetilac.Adresa.Id);
            if (adresa == null) return false;
            posetilac.Adresa = adresa;

            _posetioci[index] = posetilac;
            _serializer.ToCSV(_fileName, _posetioci);

            return true;
        }

        public bool Remove(string brojClanskeKarte)
        {
            int index = _posetioci.FindIndex(p => p.BrojClanskeKarte == brojClanskeKarte);

            if (index == -1)
                return false;

            _posetioci.RemoveAt(index);
            _serializer.ToCSV(_fileName, _posetioci);

            return true;
        }

        public List<Posetilac> GetAll()
        {
            return new List<Posetilac>(_posetioci);
        }
    }
}