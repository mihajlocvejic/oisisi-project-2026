using System.Collections.Generic;
using Core.Models;
using Core.Serialization;
using Core.Utils;
using System.Linq;

namespace Core.DAO
{
    public class KupovinaDAO
    {
        private readonly List<Kupovina> _kupovine;
        private readonly Serializer<Kupovina> _serializer;
        private readonly string _fileName;

        public KupovinaDAO(string fileName)
        {
            _fileName = fileName;
            _serializer = new Serializer<Kupovina>();
            _kupovine = _serializer.FromCSV(_fileName);
        }

        private int GenerateId()
        {
            int maxId = 0;

            foreach (Kupovina kupovina in _kupovine)
            {
                if (kupovina.Id > maxId)
                    maxId = kupovina.Id;
            }

            return maxId + 1;
        }

        public void Add(Kupovina kupovina)
        {
            Kupovina nova = new Kupovina(
                GenerateId(),
                kupovina.BrojClanskeKarte,
                kupovina.ISBN,
                kupovina.DatumKupovine,
                kupovina.Ocena,
                kupovina.Komentar
            );

            _kupovine.Add(nova);
            Save();
        }

        public bool Update(int id, Kupovina kupovina)
        {
            Kupovina? postojeca = GetById(id);

            if (postojeca == null)
                return false;

            if (kupovina.Id != id)
                return false;

            postojeca.BrojClanskeKarte = kupovina.BrojClanskeKarte;
            postojeca.ISBN = kupovina.ISBN;
            postojeca.DatumKupovine = kupovina.DatumKupovine;
            postojeca.Ocena = kupovina.Ocena;
            postojeca.Komentar = kupovina.Komentar;

            Save();

            return true;
        }

        public bool Remove(int id)
        {
            Kupovina? kupovina = GetById(id);

            if (kupovina == null)
                return false;

            _kupovine.Remove(kupovina);
            Save();

            return true;
        }

        public List<Kupovina> GetAll()
        {
            return new List<Kupovina>(_kupovine);
        }

        public Kupovina? GetById(int id)
        {
            return _kupovine.FirstOrDefault(k => k.Id == id);
        }

        public List<Kupovina> GetByPosetilac(string brojClanskeKarte)
        {
            return _kupovine.Where(k =>
                EntityComparison.SameIdentifier(
                    k.BrojClanskeKarte, brojClanskeKarte)).ToList();
        }

        public List<Kupovina> GetByISBN(string isbn)
        {
            return _kupovine.Where(k =>
                EntityComparison.SameISBN(k.ISBN, isbn)).ToList();
        }

        private void Save()
        {
            _serializer.ToCSV(_fileName, _kupovine);
        }
    }
}