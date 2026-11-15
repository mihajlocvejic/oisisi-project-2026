using System;
using System.IO;

namespace Core.Storage
{
    public static class DataPaths
    {
        public static string GetPath(string fileName)
        {
            char separator = Path.DirectorySeparatorChar;

            return Path.GetFullPath(
                $"..{separator}..{separator}..{separator}..{separator}Data{separator}{fileName}",
                AppContext.BaseDirectory);
        }
    }
}