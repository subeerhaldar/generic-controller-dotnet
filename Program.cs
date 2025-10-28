using AutoMapper;
using GenericController.Controllers;
using GenericController.Data;
using GenericController.DTOs;
using GenericController.MappingProfiles;
using GenericController.Models;
using GenericController.Repositories;
using GenericController.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Register generic services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));

// Register specific services for master entities
builder.Services.AddScoped<IGenericService<MstMarketingNote, MstMarketingNoteDto>, GenericService<MstMarketingNote, MstMarketingNoteDto>>();
builder.Services.AddScoped<IGenericService<MstLegalChecklist, MstLegalChecklistDto>, GenericService<MstLegalChecklist, MstLegalChecklistDto>>();
builder.Services.AddScoped<IGenericService<MstCommercialChecklist, MstCommercialChecklistDto>, GenericService<MstCommercialChecklist, MstCommercialChecklistDto>>();

// Register controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
