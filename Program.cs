using System.Text.Json;

namespace TaskTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new Configuration();
            var fileHandler = new FileHandler(config.FilePath);
            var taskManager = new TaskManager(fileHandler);


            List<string> allowedCmds = new List<string> { "add", "update", "delete", "mark-in-progress", "mark-done", "list" };


            if (args.Length == 0 || !allowedCmds.Contains(args[0].ToLower()))
            {
                Console.WriteLine("Invalid command. Allowed commands: " + string.Join(", ", allowedCmds));
                return;
            }

            try
            {
                List<Task> tasks = taskManager.LoadTasks();
                string command = args[0].ToLower();

                switch (command)
                {
                    case "add":
                        AddCommand(args, tasks);
                        break;

                    case "update":
                        UpdateCommand(args, tasks);
                        break;

                    case "delete":
                        DeleteCommand(args, tasks);
                        break;

                    case "mark-in-progress":
                        StatusChange(args, tasks, Status.InProgress);
                        break;

                    case "mark-done":
                        StatusChange(args, tasks, Status.Done);
                        break;

                    case "list":
                        ListCommand(tasks);
                        break;

                }
                taskManager.SaveTasks(tasks);
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void AddCommand(string[] args, List<Task> tasks)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: add <description>");
                return;
            }

            string description = string.Join(" ", args.Skip(1));
            int newId = tasks.Any() ? tasks.Max(x => x.Id) + 1 : 1;

            tasks.Add(new Task { Id = newId, Description = description });
            Console.WriteLine($"Task {newId} created!");
        }

        private static void UpdateCommand(string[] args, List<Task> tasks)
        {
            if (args.Length < 3 || !int.TryParse(args[1], out int updateId))
            {
                Console.WriteLine("Usage: update <id> <description>");
                return;
            }

            var taskToUpdate = tasks.FirstOrDefault(t => t.Id == updateId);
            if (taskToUpdate != null)
            {
                string newDescription = string.Join(" ", args.Skip(2));
                taskToUpdate.UpdateDescription(newDescription);
                Console.WriteLine($"Task {updateId} updated.");
            }
            else
            {
                Console.WriteLine($"Task {updateId} not found.");
            }
        }

        private static void DeleteCommand(string[] args, List<Task> tasks)
        {
            if (args.Length < 2 || !int.TryParse(args[1], out int deleteId))
            {
                Console.WriteLine("Usage: delete <id>");
                return;
            }

            var taskToDelete = tasks.FirstOrDefault(x => x.Id == deleteId);
            if (taskToDelete != null)
            {
                tasks.Remove(taskToDelete);
                Console.WriteLine($"Task {deleteId} deleted.");
            }
            else
            {
                Console.WriteLine($"Task {deleteId} not found.");
            }
        }

        private static void ListCommand(List<Task> tasks)
        {
            if (!tasks.Any())
            {
                Console.WriteLine("No tasks found");
                return;
            }

            Console.WriteLine("Task List: ");
            foreach (var task in tasks)
            {
                Console.WriteLine(JsonSerializer.Serialize(task, new JsonSerializerOptions { WriteIndented = true }));
            }
        }

        private static void StatusChange(string[] args, List<Task> tasks, Status newStatus)
        {
            if (args.Length < 2 || !int.TryParse(args[1], out int taskId))
            {
                Console.WriteLine($"Usage: {args[0]} <id>");
                return;
            }

            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.UpdateStatus(newStatus);
                Console.WriteLine($"Task {taskId} marked as {newStatus}.");
            }
            else
            {
                Console.WriteLine($"Task {taskId} not found.");
            }
        }
    }

    internal class Configuration
    {
        public string FilePath { get; }

        public Configuration()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            FilePath = Path.Combine(baseDirectory, "tasks.json");
        }
    }
}
