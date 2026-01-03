namespace TaskManagerBlazor.Services;

using TaskManagerBlazor.Models;
using TG.Blazor.IndexedDB;

public class TaskServiceIndexedDb : ITaskService
{
    private readonly IndexedDBManager _db;

    private StoreRecord<TaskItem> _stored(TaskItem item) => new StoreRecord<TaskItem>
    {
        Storename = "tasks",
        Data = item,
    };
    public TaskServiceIndexedDb(IndexedDBManager db)
    {
        _db = db;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _db.GetRecords<TaskItem>("tasks");
    }

    public async Task AddAsync(TaskItem item)
    {
        await _db.AddRecord(_stored(item));
    }

    public async Task UpdateAsync(TaskItem item)
    {
        await _db.UpdateRecord(_stored(item));
    }

    public async Task DeleteAsync(TaskItem item)
    {
        await _db.DeleteRecord("tasks", item.Id);
    }

    public async Task<TaskItem> GetByIdAsync(Guid id)
    {
        return await _db.GetRecordById<Guid, TaskItem>("tasks", id);
    }
}
