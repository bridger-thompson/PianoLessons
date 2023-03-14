using Microsoft.EntityFrameworkCore;
using PianoLessonsApi.Data;
using PianoLessonsApi.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<PianoLessonDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IPianoLessonsRepo, PianoLessonRepo>();
builder.Services.AddScoped<IPianoLessonsApplication, PianoLessonsApplication>();


builder.Services.AddControllers().AddJsonOptions(x =>
	x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
