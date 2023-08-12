global using Microsoft.EntityFrameworkCore;
using simpleapi;
using simpleapi.Data;
using simpleapi.Interfaces;
using simpleapi.Repository;


var builder = WebApplication.CreateBuilder(args);

// setting for connecting to Database
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(connection);
});

// DI. 
// AddTransient means add the service at the top 
builder.Services.AddTransient<Seed>();

// we need to tell the DotNet that we use Depency Injection in Program.cs
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// seeding. it will run before the app starts
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        // Injection the service
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
