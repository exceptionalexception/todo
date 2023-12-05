using Business;
using DataAccessMemory;
using DataAccessDb;
using Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());    
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

// Register dependencies.
builder.Services.AddTransient<ITodoManager, TodoManager>();

var useMemoryConfidValue = builder.Configuration.GetValue<string>("UseMemory");
var useMemory = useMemoryConfidValue != null && bool.Parse(useMemoryConfidValue);

// Register repos based on the config.
if (useMemory)
{
    builder.Services.AddTransient<ITodoRepo, TodoMemoryRepo>();
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrEmpty(connectionString))
    {
        //builder.Services.AddDbContext<TodoDbContext>(connectionString);
        builder.Services.AddTransient<ITodoRepo, TodoDbRepo>();
    }
    else
    {
        throw new Exception($"No connection string was found in appsettings.{builder.Environment.EnvironmentName}.json file.");
    }
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
