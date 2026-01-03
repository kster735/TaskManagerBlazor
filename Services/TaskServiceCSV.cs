namespace TaskManagerBlazor.Services;

using TaskManagerBlazor.Models;
public class CsvTaskService : ITaskService
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);


    public CsvTaskService(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        if (!File.Exists(_filePath))
            return new List<TaskItem>();

        var lines = await File.ReadAllLinesAsync(_filePath);
        return lines.Select(ParseCsvLine).ToList();
    }


    public async Task<TaskItem> GetByTitleAsync(string title)
    {
        var items = await GetAllAsync();
        var item = items.FirstOrDefault(t => t.Title == title);
        // return null if not found
        return item!;
    }

    public async Task AddAsync(TaskItem item)
    {
        // check for duplicates
        var existingItem = await GetByTitleAsync(item.Title);
        if (existingItem != null)
            throw new InvalidOperationException("A task with the same title already exists.");

        var line = ToCsvLine(item);
        await File.AppendAllTextAsync(_filePath, line + Environment.NewLine);
    }

    public async Task UpdateAsync(TaskItem item)
    {
        var items = await GetAllAsync();

        var index = items.FindIndex(t => t.Title == item.Title);
        if (index >= 0)
        {
            items[index] = item;
            await SaveAllAsync(items);
        }
    }

    public async Task DeleteAsync(TaskItem item)
    {
        var items = await GetAllAsync();
        items.RemoveAll(t => t.Title == item.Title);
        await SaveAllAsync(items);
    }

    private async Task SaveAllAsync(List<TaskItem> items)
    {
        await _lock.WaitAsync();
        try
        {
            // If multiple users are accessing the file, this ensures thread safety
            var lines = items.Select(ToCsvLine);
            await File.WriteAllLinesAsync(_filePath, lines);
        }
        finally
        {
            _lock.Release();
        }
    }

    private TaskItem ParseCsvLine(string line)
    {
        var parts = line.Split(',');

        return new TaskItem
        {
            Title = parts[0].Trim('"'),
            Description = parts[1].Trim('"'),
            Completed = bool.Parse(parts[2]),
            Due = DateTime.Parse(parts[3]),
            AlertBefore = string.IsNullOrWhiteSpace(parts[4])
                ? null
                : TimeSpan.Parse(parts[4])
        };
    }

    private string ToCsvLine(TaskItem item)
    {
        return $"\"{item.Title}\",\"{item.Description}\",{item.Completed},{item.Due:O},{item.AlertBefore}";
    }

    public Task<TaskItem> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
