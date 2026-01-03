using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskManagerBlazor;
using TG.Blazor.IndexedDB;
using TaskManagerBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


builder.Services.AddIndexedDB(dbStore =>
{
    dbStore.DbName = "TaskDb";
    dbStore.Version = 1;

    dbStore.Stores.Add(new StoreSchema
    {
        Name = "tasks",
        PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = false },
        Indexes = new List<IndexSpec>
        {
            new IndexSpec { Name = "title", KeyPath = "title", Auto = false },
            new IndexSpec { Name = "due", KeyPath = "due", Auto = false }
        }
    });
});

builder.Services.AddScoped<ITaskService, TaskServiceIndexedDb>();


await builder.Build().RunAsync();
