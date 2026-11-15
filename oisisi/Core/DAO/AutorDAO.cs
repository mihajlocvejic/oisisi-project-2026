using Core.Models;
using Core.Utils;
using Core.Serialization;
using Core.Validation;

namespace Core.DAO
{
    public class AutorDAO
    {
        private readonly AdresaDAO _adresaDAO;
        private readonly List<Autor> _autori;
        private readonly Serializer<Autor> _serializer = new();
        private readonly string _filePath;

        public AutorDAO(string filePath, AdresaDAO adresaDAO)
        {
            ArgumentNullException.ThrowIfNull(adresaDAO);
            _filePath = filePath;
            _adresaDAO = adresaDAO;
            _autori = _serializer.FromCSV(filePath);
            foreach (Autor item in _autori)
                item.Adresa = _adresaDAO.GetById(item.Adresa.Id)
                    ?? throw new FormatException("Povezana adresa ne postoji.");
        }

        public List<Autor> GetAll() => new(_autori);

        public Autor? GetById(string brojLicneKarte) => _autori.FirstOrDefault(
            a => EntityComparison.SameIdentifier(a.BrojLicneKarte, brojLicneKarte));

        public void Add(Autor autor)
        {
            AutorValidator.Validate(autor);
            if (GetById(autor.BrojLicneKarte) != null)
                throw new InvalidOperationException("Autor sa tim brojem licne karte vec postoji.");
            Autor stored = CopyForStorage(autor, autor.BrojLicneKarte);
            List<Autor> updated = new(_autori) { stored };
            Save(updated);
            _autori.Add(stored);
        }

        public void Update(Autor autor)
        {
            AutorValidator.Validate(autor);
            Autor existing = GetById(autor.BrojLicneKarte)
                ?? throw new InvalidOperationException("Autor ne postoji.");
            Autor stored = CopyForStorage(autor, existing.BrojLicneKarte);
            int index = _autori.IndexOf(existing);
            List<Autor> updated = new(_autori);
            updated[index] = stored;
            Save(updated);
            _autori[index] = stored;
        }

        public void Remove(string brojLicneKarte)
        {
            Autor existing = GetById(brojLicneKarte)
                ?? throw new InvalidOperationException("Autor ne postoji.");
            List<Autor> updated = new(_autori);
            updated.Remove(existing);
            Save(updated);
            _autori.Remove(existing);
        }

        private Autor CopyForStorage(Autor autor, string brojLicneKarte)
        {
            Adresa adresa = _adresaDAO.GetById(autor.Adresa.Id)
                ?? throw new ArgumentException("Izaberite postojecu adresu.");
            return new Autor(autor.Ime, autor.Prezime, autor.DatumRodjenja, adresa,
                autor.Telefon, autor.Email, brojLicneKarte, autor.GodineIskustva);
        }

        public void Save() => Save(_autori);

        private void Save(List<Autor> autori)
        {
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
            _serializer.ToCSV(_filePath, autori);
        }
    }
}
