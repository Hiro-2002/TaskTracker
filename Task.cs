using System.Text.Json.Serialization;

namespace TaskTracker
{
    // enum for each status of a task
    public enum Status
    {
        Todo,
        InProgress,
        Done
    }

    internal class Task
    {
        public int Id { get; set; }
        public required string Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; } = Status.Todo;

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Task()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void UpdateStatus(Status newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.Now;
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
            UpdatedAt = DateTime.Now;
        }
    }
}
