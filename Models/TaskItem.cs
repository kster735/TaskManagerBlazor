namespace TaskManagerBlazor.Models;

public class TaskItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public DateTime Due { get; set; }
    public TimeSpan? AlertBefore { get; set; }
}