using AutoMapper;
using GenericController.Controllers;
using GenericController.CQRS.Commands;
using GenericController.CQRS.Handlers;
using GenericController.CQRS.Queries;
using GenericController.Data;
using GenericController.DTOs;
using GenericController.MappingProfiles;
using GenericController.Models;
using GenericController.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

// Register generic repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Register specific handlers for each entity-DTO pair
builder.Services.AddScoped<IRequestHandler<GetAllQuery<MstMarketingNoteDto>, IEnumerable<MstMarketingNoteDto>>, GetAllQueryHandler<MstMarketingNote, MstMarketingNoteDto>>();
builder.Services.AddScoped<IRequestHandler<GetByIdQuery<MstMarketingNoteDto>, MstMarketingNoteDto?>, GetByIdQueryHandler<MstMarketingNote, MstMarketingNoteDto>>();
builder.Services.AddScoped<IRequestHandler<GetPagedQuery<MstMarketingNoteDto>, (IEnumerable<MstMarketingNoteDto> Items, int TotalCount)>, GetPagedQueryHandler<MstMarketingNote, MstMarketingNoteDto>>();
builder.Services.AddScoped<IRequestHandler<CreateCommand<MstMarketingNote, MstMarketingNoteDto>, MstMarketingNoteDto>, CreateCommandHandler<MstMarketingNote, MstMarketingNoteDto>>();
builder.Services.AddScoped<IRequestHandler<UpdateCommand<MstMarketingNote, MstMarketingNoteDto>>, UpdateCommandHandler<MstMarketingNote, MstMarketingNoteDto>>();
builder.Services.AddScoped<IRequestHandler<DeleteCommand<MstMarketingNote>>, DeleteCommandHandler<MstMarketingNote>>();
builder.Services.AddScoped<IRequestHandler<SoftDeleteCommand<MstMarketingNote>>, SoftDeleteCommandHandler<MstMarketingNote>>();

builder.Services.AddScoped<IRequestHandler<GetAllQuery<MstLegalChecklistDto>, IEnumerable<MstLegalChecklistDto>>, GetAllQueryHandler<MstLegalChecklist, MstLegalChecklistDto>>();
builder.Services.AddScoped<IRequestHandler<GetByIdQuery<MstLegalChecklistDto>, MstLegalChecklistDto?>, GetByIdQueryHandler<MstLegalChecklist, MstLegalChecklistDto>>();
builder.Services.AddScoped<IRequestHandler<GetPagedQuery<MstLegalChecklistDto>, (IEnumerable<MstLegalChecklistDto> Items, int TotalCount)>, GetPagedQueryHandler<MstLegalChecklist, MstLegalChecklistDto>>();
builder.Services.AddScoped<IRequestHandler<CreateCommand<MstLegalChecklist, MstLegalChecklistDto>, MstLegalChecklistDto>, CreateCommandHandler<MstLegalChecklist, MstLegalChecklistDto>>();
builder.Services.AddScoped<IRequestHandler<UpdateCommand<MstLegalChecklist, MstLegalChecklistDto>>, UpdateCommandHandler<MstLegalChecklist, MstLegalChecklistDto>>();
builder.Services.AddScoped<IRequestHandler<DeleteCommand<MstLegalChecklist>>, DeleteCommandHandler<MstLegalChecklist>>();
builder.Services.AddScoped<IRequestHandler<SoftDeleteCommand<MstLegalChecklist>>, SoftDeleteCommandHandler<MstLegalChecklist>>();

builder.Services.AddScoped<IRequestHandler<GetAllQuery<MstCommercialChecklistDto>, IEnumerable<MstCommercialChecklistDto>>, GetAllQueryHandler<MstCommercialChecklist, MstCommercialChecklistDto>>();
builder.Services.AddScoped<IRequestHandler<GetByIdQuery<MstCommercialChecklistDto>, MstCommercialChecklistDto?>, GetByIdQueryHandler<MstCommercialChecklist, MstCommercialChecklistDto>>();
builder.Services.AddScoped<IRequestHandler<GetPagedQuery<MstCommercialChecklistDto>, (IEnumerable<MstCommercialChecklistDto> Items, int TotalCount)>, GetPagedQueryHandler<MstCommercialChecklist, MstCommercialChecklistDto>>();
builder.Services.AddScoped<IRequestHandler<CreateCommand<MstCommercialChecklist, MstCommercialChecklistDto>, MstCommercialChecklistDto>, CreateCommandHandler<MstCommercialChecklist, MstCommercialChecklistDto>>();
builder.Services.AddScoped<IRequestHandler<UpdateCommand<MstCommercialChecklist, MstCommercialChecklistDto>>, UpdateCommandHandler<MstCommercialChecklist, MstCommercialChecklistDto>>();
builder.Services.AddScoped<IRequestHandler<DeleteCommand<MstCommercialChecklist>>, DeleteCommandHandler<MstCommercialChecklist>>();
builder.Services.AddScoped<IRequestHandler<SoftDeleteCommand<MstCommercialChecklist>>, SoftDeleteCommandHandler<MstCommercialChecklist>>();

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
