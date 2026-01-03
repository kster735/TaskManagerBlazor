namespace TaskManagerBlazor.Services;

using TaskManagerBlazor.Models;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllAsync();
    Task<TaskItem> GetByIdAsync(Guid id);
    Task AddAsync(TaskItem item);
    Task UpdateAsync(TaskItem item);
    Task DeleteAsync(TaskItem item);
}
