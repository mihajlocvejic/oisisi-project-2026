using System.Collections.Generic;
using System.IO;
using Core.Interfaces;

namespace Core.Serialization
{
    public class Serializer<T> where T : ISerializable, new()
    {
        private const char Delimiter = '|';

        public void ToCSV(string fileName, List<T> objects)
        {
            string? directory = Path.GetDirectoryName(fileName);

            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            using StreamWriter writer = new StreamWriter(fileName);

            foreach (T obj in objects)
            {
                string line = string.Join(Delimiter.ToString(), obj.ToCSV());
                writer.WriteLine(line);
            }
        }

        public List<T> FromCSV(string fileName)
        {
            List<T> objects = new List<T>();

            if (!File.Exists(fileName))
                return objects;

            foreach (string line in File.ReadLines(fileName))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                T obj = new T();
                obj.FromCSV(line.Split(Delimiter));
                objects.Add(obj);
            }

            return objects;
        }
    }
}