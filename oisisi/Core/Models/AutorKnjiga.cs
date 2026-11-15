using Core.Interfaces;

namespace Core.Models
{
    public class AutorKnjiga : ISerializable
    {
        private bool _initialized;
        public string BrojLicneKarte { get; private set; } = string.Empty;
        public string ISBN { get; private set; } = string.Empty;

        public AutorKnjiga() { }

        public AutorKnjiga(string brojLicneKarte, string isbn)
        {
            BrojLicneKarte = brojLicneKarte;
            ISBN = isbn;
            _initialized = true;
        }

        public string[] ToCSV() => new[] { BrojLicneKarte, ISBN };

        public void FromCSV(string[] values)
        {
            if (values.Length != 2)
                throw new FormatException("Neispravan format veze autor-knjiga.");
            if (_initialized)
                throw new InvalidOperationException("Postojeca veza se ne moze menjati. Uklonite je i dodajte novu.");
            BrojLicneKarte = values[0];
            ISBN = values[1];
            _initialized = true;
        }

        public override string ToString() => $"{BrojLicneKarte} | {ISBN}";
    }
}
