using System.Text.Json;

namespace TaskTracker
{
    internal class TaskManager
    {
        private readonly FileHandler _fileHandler;
        private readonly JsonSerializerOptions _jsonOptions;

        public TaskManager(FileHandler fileHandler)
        {
            _fileHandler = fileHandler ?? throw new ArgumentNullException(nameof(fileHandler));
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public List<Task> LoadTasks()
        {
            try
            {
                string content = _fileHandler.ReadFile();
                var tasks = JsonSerializer.Deserialize<List<Task>>(content, _jsonOptions);
                return tasks ?? new List<Task>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing tasks: {ex.Message}");
                return new List<Task>();
            }
        }

        public void SaveTasks(List<Task> tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            try
            {
                string content = JsonSerializer.Serialize(tasks, _jsonOptions);
                _fileHandler.WriteFile(content);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error serializing tasks: {ex.Message}");
                throw;
            }
        }
    }
}