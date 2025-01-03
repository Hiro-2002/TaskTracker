using System;
using System.IO;

namespace TaskTracker
{
    internal class FileHandler
    {
        private readonly string _filePath;

        public FileHandler(string filePath)
        {
            _filePath = filePath;

            EnsureFileExists();
        }

        public void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                using (StreamWriter sw = new StreamWriter(_filePath))
                {
                    sw.WriteLine("[]");
                }

                Console.WriteLine($"File created at: {_filePath}");
            }
        }

        public string ReadFile()
        {
            return File.ReadAllText(_filePath);
        }

        public void WriteFile(string filePath)
        {
            File.WriteAllText(_filePath, filePath);
        }
    }
}
