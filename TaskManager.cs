using System;
using System.IO;
using System.Text.Json;

namespace TaskTracker
{
    internal class TaskManager
    { 
        private readonly FileHandler _fileHandler;

        public TaskManager(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public List<Task> LoadTasks()
        {
            string content = _fileHandler.ReadFile();
            return JsonSerializer.Deserialize<List<Task>>(content) ?? new List<Task>();
        }

        public void SaveTasks(List<Task> tasks)
        {
            string content = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            _fileHandler.WriteFile(content);
        }

    }
}
