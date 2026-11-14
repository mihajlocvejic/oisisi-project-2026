using System.Collections.Generic;
using Core.Models;
using Core.Serialization;

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
            _serializer.ToCSV(_fileName, _kupovine);
        }

        public bool Update(int id, Kupovina kupovina)
        {
            int index = _kupovine.FindIndex(k => k.Id == id);

            if (index == -1)
                return false;

            if (kupovina.Id != id)
                return false;

            _kupovine[index] = kupovina;
            _serializer.ToCSV(_fileName, _kupovine);

            return true;
        }

        public bool Remove(int id)
        {
            int index = _kupovine.FindIndex(k => k.Id == id);

            if (index == -1)
                return false;

            _kupovine.RemoveAt(index);
            _serializer.ToCSV(_fileName, _kupovine);

            return true;
        }

        public List<Kupovina> GetAll()
        {
            return new List<Kupovina>(_kupovine);
        }
    }
}