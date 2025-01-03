

using Newtonsoft.Json;
using System.Xml;

namespace TaskTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Users\mhdka\source\repos\TaskTracker\tasks.json";
            List<string> allowedCmds = new List<string> { "add", "update", "delete", "mark-in-progress", "mark-done", "list" };

            var fileHandler = new FileHandler(filePath);
            var taskManager = new TaskManager(fileHandler);

            // load tasks if no task creates an emtpy list
            List<Task> tasks = taskManager.LoadTasks();

            if (args.Length == 0 || !allowedCmds.Contains(args[0].ToLower()))
            {
                Console.WriteLine("Invalid command. Allowed commands: " + string.Join(", ", allowedCmds));
                return;
            }

            string command = args[0].ToLower();


            switch (command)
            {
                case "add":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: add <description>");
                        return;
                    }

                    string description = args[1];
                    int newId = tasks.Any() ? tasks.Max(x => x.Id) + 1 : 1;

                    tasks.Add(new Task { Id = newId, Description = description});
                    Console.WriteLine($"Task Created!");
                    break;

                case "delete":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: delete <id>");
                        return;
                    }

                    if (int.TryParse(args[1], out int deleteId))
                    {
                        var taskToDelete = tasks.FirstOrDefault(x => x.Id == deleteId);
                        if (taskToDelete != null)
                        {
                            tasks.Remove(taskToDelete);
                            Console.WriteLine($"Task {deleteId} deleted.");
                        }
                    }   
                    break;

                case "update":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Usage: update <id> <description>");
                        return;
                    }

                    if (!int.TryParse(args[1], out int updateId))
                    {
                        var taskToUpdate = tasks.FirstOrDefault(t => t.Id == updateId);
                        if (taskToUpdate != null)
                        {
                            taskToUpdate.UpdateDescription(args[2]);
                            Console.WriteLine($"Task {updateId} updated.");
                        }
                        else
                        {
                            Console.WriteLine("Task not found");
                        }
                    }
                    break;

                case "list":
                    if (!tasks.Any())
                    {
                        Console.WriteLine("No task found!");
                    }
                    else
                    {
                        Console.WriteLine("Task List: ");
                        foreach (var task in tasks)
                        {
                            var taskDict = new Dictionary<string, string>
                            {
                                { "Id", task.Id.ToString() },
                                { "Description", task.Description },
                                { "Status", task.Status.ToString() },
                                { "CreatedAt", task.CreatedAt.ToString() },
                                { "UpdatedAt", task.UpdatedAt.ToString() }
                            };

                            Console.WriteLine(JsonConvert.SerializeObject(taskDict, Formatting.Indented));
                        }
                    }
                    break;

            }

            taskManager.SaveTasks(tasks);

        }
    }
}
