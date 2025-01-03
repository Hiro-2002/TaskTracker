using System;
using System.IO;

namespace TaskTracker
{
    internal class FileHandler
    {
        private readonly string _filePath;

        public FileHandler(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty", nameof(filePath));

            _filePath = filePath;
            EnsureFileExists();
        }

        public void EnsureFileExists()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    string? directoryPath = Path.GetDirectoryName(_filePath);
                    if (!string.IsNullOrWhiteSpace(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (StreamWriter sw = new StreamWriter(_filePath))
                    {
                        sw.WriteLine("[]");
                    }
                    Console.WriteLine($"File created at: {Path.GetFullPath(_filePath)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file: {ex.Message}");
                throw;
            }
        }

        public string ReadFile()
        {
            try
            {
                return File.ReadAllText(_filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return "[]";
            }
        }

        public void WriteFile(string content)
        {
            try
            {
                File.WriteAllText(_filePath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
                throw;
            }
        }
    }
}