using System.Collections.Generic;
using Core.Models;
using Core.Serialization;

namespace Core.DAO
{
    public class IzdavacDAO
    {
        private readonly List<Izdavac> _izdavaci;
        private readonly Serializer<Izdavac> _serializer;
        private readonly string _fileName;

        public IzdavacDAO(string fileName)
        {
            _fileName = fileName;
            _serializer = new Serializer<Izdavac>();
            _izdavaci = _serializer.FromCSV(_fileName);
        }

        public bool Add(Izdavac izdavac)
        {
            if (_izdavaci.Exists(i => i.Sifra == izdavac.Sifra))
                return false;

            _izdavaci.Add(izdavac);
            _serializer.ToCSV(_fileName, _izdavaci);

            return true;
        }

        public bool Update(string sifra, Izdavac izdavac)
        {
            int index = _izdavaci.FindIndex(i => i.Sifra == sifra);

            if (index == -1)
                return false;

            if (izdavac.Sifra != sifra)
                return false;

            _izdavaci[index] = izdavac;
            _serializer.ToCSV(_fileName, _izdavaci);

            return true;
        }

        public bool Remove(string sifra)
        {
            int index = _izdavaci.FindIndex(i => i.Sifra == sifra);

            if (index == -1)
                return false;

            _izdavaci.RemoveAt(index);
            _serializer.ToCSV(_fileName, _izdavaci);

            return true;
        }

        public List<Izdavac> GetAll()
        {
            return new List<Izdavac>(_izdavaci);
        }
    }
}