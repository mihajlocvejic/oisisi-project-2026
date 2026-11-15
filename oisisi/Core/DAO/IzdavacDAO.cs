using System.Collections.Generic;
using Core.Models;
using Core.Serialization;
using System;
using System.Linq;
using Core.Utils;

namespace Core.DAO
{
    public class IzdavacDAO
    {
        private readonly List<Izdavac> _izdavaci;
        private readonly Serializer<Izdavac> _serializer;
        private readonly string _fileName;
        private readonly AutorDAO _autorDAO;

        public IzdavacDAO(string fileName, AutorDAO autorDAO)
        {
            _fileName = fileName;
            _autorDAO = autorDAO;
            _serializer = new Serializer<Izdavac>();
            _izdavaci = _serializer.FromCSV(_fileName);

            foreach (Izdavac izdavac in _izdavaci)
            {
                if (izdavac.SefIzdavaca != null)
                {
                    izdavac.SefIzdavaca = _autorDAO.GetById(
                        izdavac.SefIzdavaca.BrojLicneKarte)
                        ?? throw new FormatException("Sef izdavaca ne postoji.");
                }

                izdavac.Autori = izdavac.Autori
                    .Select(a => _autorDAO.GetById(a.BrojLicneKarte)
                        ?? throw new FormatException("Povezani autor ne postoji."))
                    .ToList();
            }
        }

        public bool Add(Izdavac izdavac)
        {
            if (GetById(izdavac.Sifra) != null)
                return false;

            _izdavaci.Add(izdavac);
            Save();

            return true;
        }

        public bool Update(string sifra, Izdavac izdavac)
        {
            Izdavac? postojeci = GetById(sifra);

            if (postojeci == null)
                return false;

            if (postojeci.Sifra != izdavac.Sifra)
                return false;

            postojeci.Naziv = izdavac.Naziv;
            postojeci.SefIzdavaca = izdavac.SefIzdavaca;
            postojeci.Autori = new List<Autor>(izdavac.Autori);

            Save();

            return true;
        }

        public bool Remove(string sifra)
        {
            Izdavac? izdavac = GetById(sifra);

            if (izdavac == null)
                return false;

            _izdavaci.Remove(izdavac);
            Save();

            return true;
        }

        public List<Izdavac> GetAll()
        {
            return new List<Izdavac>(_izdavaci);
        }

        public Izdavac? GetById(string sifra)
        {
            return _izdavaci.FirstOrDefault(i =>
                EntityComparison.SameIdentifier(i.Sifra, sifra));
        }

        private void Save()
        {
            _serializer.ToCSV(_fileName, _izdavaci);
        }
    }
}