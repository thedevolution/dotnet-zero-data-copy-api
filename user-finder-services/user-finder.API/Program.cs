using Microsoft.EntityFrameworkCore;
using user_finder.API.Repositories;
using user_finder.API.Repositories.EF;
using user_finder.API.Services;

var builder = WebApplication.CreateBuilder(args);

//////////////////////////////// ADDITIONS to Program.cs //////////////////////////////////////
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SnowflakeConnection");
builder.Services.AddDbContext<DatabaseContext>(dbContextOptions =>
{
	dbContextOptions.UseSnowflake(connectionString);
	dbContextOptions.LogTo(Console.WriteLine, LogLevel.Information); // or LogLevel.Debug for more detail
	dbContextOptions.EnableSensitiveDataLogging(); // WARNING: Only use in development!
});

builder.Services.AddTransient<IFoundUserRepository, FoundUserRepository>();
builder.Services.AddTransient<IUserFinderService, UserFinderService>();

//////////////////////////////// End ADDITIONS to Program.cs //////////////////////////////////////

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
