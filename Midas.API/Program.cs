using Microsoft.EntityFrameworkCore;
using Midas.Infrastructure.Persistence;
using Midas.Infrastructure.Persistence.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext for Oracle
builder.Services.AddDbContext<MidasContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.UseOracleSQLCompatibility("11")));

// Register repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IGastoRepository, GastoRepository>();
builder.Services.AddScoped<IReceitaRepository, ReceitaRepository>();
builder.Services.AddScoped<ICofrinhoRepository, CofrinhoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

// Configure CORS - mais permissivo para desenvolvimento
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Adicionar CORS antes de outras middlewares
app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
