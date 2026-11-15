using Core.Models;
using Core.Utils;
using Core.Serialization;
using Core.Validation;

namespace Core.DAO
{
    public class KnjigaDAO
    {
        private readonly List<Knjiga> _knjige;
        private readonly Serializer<Knjiga> _serializer = new();
        private readonly string _filePath;

        public KnjigaDAO(string filePath)
        {
            _filePath = filePath;
            _knjige = _serializer.FromCSV(filePath);
        }

        public List<Knjiga> GetAll() => new(_knjige);

        public Knjiga? GetByISBN(string isbn) => string.IsNullOrWhiteSpace(isbn) ? null
            : _knjige.FirstOrDefault(k => EntityComparison.SameISBN(k.ISBN, isbn));

        public void Add(Knjiga knjiga)
        {
            KnjigaValidator.Validate(knjiga);
            if (GetByISBN(knjiga.ISBN) != null)
                throw new InvalidOperationException("Knjiga sa tim ISBN brojem vec postoji.");
            Knjiga stored = Copy(knjiga, knjiga);
            List<Knjiga> updated = new(_knjige) { stored };
            Save(updated);
            _knjige.Add(stored);
        }

        public void Update(Knjiga knjiga)
        {
            KnjigaValidator.Validate(knjiga);
            Knjiga existing = GetByISBN(knjiga.ISBN)
                ?? throw new InvalidOperationException("Knjiga ne postoji.");
            // Osnovni podaci se menjaju odvojeno od kupovina i liste zelja.
            Knjiga stored = Copy(knjiga, existing);
            int index = _knjige.IndexOf(existing);
            List<Knjiga> updated = new(_knjige);
            updated[index] = stored;
            Save(updated);
            _knjige[index] = stored;
        }

        private static Knjiga Copy(Knjiga data, Knjiga relatedData) => new(
            relatedData.ISBN, data.Naziv, data.Zanr, data.GodinaIzdanja, data.Cena,
            data.BrojStrana, data.SifraIzdavaca, relatedData.KupciClanskeKarte,
            relatedData.ListaZeljaClanskeKarte);

        public void Remove(string isbn)
        {
            Knjiga existing = GetByISBN(isbn)
                ?? throw new InvalidOperationException("Knjiga ne postoji.");
            List<Knjiga> updated = new(_knjige);
            updated.Remove(existing);
            Save(updated);
            _knjige.Remove(existing);
        }

        public void Save() => Save(_knjige);

        private void Save(List<Knjiga> knjige)
        {
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
            _serializer.ToCSV(_filePath, knjige);
        }
    }
}
