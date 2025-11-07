using Microsoft.EntityFrameworkCore;
using Midas.Infrastructure.Persistence;
using Midas.Infrastructure.Persistence.Repositories;
using System.Text.Json.Serialization;
using Midas.API.Filters;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "5220";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SchemaFilter<FiltroCamposBooleanos>();
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
     Title = "Midas API",
        Version = "v1",
        Description = "API para controle financeiro - Sistema Midas"
});
});

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
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
          .AllowAnyHeader();
    });
});

var app = builder.Build();

// Adicionar CORS antes de outras middlewares
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Midas API v1");
    c.RoutePrefix = "swagger";
        c.DocumentTitle = "Midas API Documentation";
    });
    app.UseDeveloperExceptionPage();
}

// Comentando HTTPS redirect para testes
// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
