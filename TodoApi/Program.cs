using Business;
using DataAccess;
using DataAccessDb;
using DataAccessMemory;
using Databaser;
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
    builder.Services.AddSingleton<ITodoRepo, TodoMemoryRepo>();
}
else
{
        DbSetup.SetupDb(builder.Configuration);
 
        builder.Services.AddScoped(_ => new TodoDb(builder.Configuration));
        builder.Services.AddTransient<ITodoRepo, TodoDbRepo>();
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

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();

#region Methods





#endregion