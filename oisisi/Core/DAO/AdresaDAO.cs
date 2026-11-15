using Core.Models;
using Core.Serialization;
using Core.Validation;

namespace Core.DAO
{
    public class AdresaDAO
    {
        private readonly List<Adresa> _adrese;
        private readonly Serializer<Adresa> _serializer = new();
        private readonly string _filePath;

        public AdresaDAO(string filePath)
        {
            _filePath = filePath;
            _adrese = _serializer.FromCSV(filePath);
            if (_adrese.Any(a => a.Id <= 0) || _adrese.Select(a => a.Id).Distinct().Count() != _adrese.Count)
                throw new FormatException("ID adresa mora biti pozitivan i jedinstven.");
            foreach (Adresa adresa in _adrese)
                AdresaValidator.Validate(adresa);
        }

        public List<Adresa> GetAll() => new(_adrese);
        public Adresa? GetById(int id) => _adrese.FirstOrDefault(a => a.Id == id);

        public Adresa Add(Adresa adresa)
        {
            AdresaValidator.Validate(adresa);
            int id = checked(_adrese.Select(a => a.Id).DefaultIfEmpty(0).Max() + 1);
            Adresa nova = new(adresa.Ulica, adresa.Broj, adresa.Grad, adresa.Drzava) { Id = id };
            List<Adresa> updated = new(_adrese) { nova };
            Save(updated);
            _adrese.Add(nova);
            return nova;
        }

        public bool Update(int id, Adresa adresa)
        {
            AdresaValidator.Validate(adresa);
            Adresa? existing = GetById(id);
            if (existing == null) return false;
            Adresa nova = new(adresa.Ulica, adresa.Broj, adresa.Grad, adresa.Drzava) { Id = id };
            List<Adresa> updated = _adrese.Select(a => a.Id == id ? nova : a).ToList();
            Save(updated);
            // Zadrzavamo objekat koji vec koriste povezani autori i posetioci.
            existing.Ulica = nova.Ulica;
            existing.Broj = nova.Broj;
            existing.Grad = nova.Grad;
            existing.Drzava = nova.Drzava;
            return true;
        }

        public bool Remove(int id)
        {
            Adresa? existing = GetById(id);
            if (existing == null) return false;
            Save(_adrese.Where(a => a.Id != id).ToList());
            _adrese.Remove(existing);
            return true;
        }

        private void Save(List<Adresa> adrese)
        {
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
            _serializer.ToCSV(_filePath, adrese);
        }
    }
}
