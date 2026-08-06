using AnalisisOpiniones.Data.Factories;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using AnalisisOpiniones.Data.Persistence.Dwh.Writers;
using AnalisisOpiniones.Data.Services;
using AnalisisOpiniones.WkServicee;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

string dwhConnStr = builder.Configuration.GetConnectionString("DwhConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? string.Empty;

// Registrar Factoría de lectura de archivos
builder.Services.AddSingleton<IFileReaderFactory, FileReaderFactory>();

// Registrar Repositorios de Data Warehouse
builder.Services.AddScoped<IDimClienteRepository>(_ => new DimClienteRepository(dwhConnStr));
builder.Services.AddScoped<IDimProductoRepository>(_ => new DimProductoRepository(dwhConnStr));
builder.Services.AddScoped<IDimFuenteRepository>(_ => new DimFuenteRepository(dwhConnStr));
builder.Services.AddScoped<IDimClasificacionRepository>(_ => new DimClasificacionRepository(dwhConnStr));
builder.Services.AddScoped<IDimFechaRepository>(_ => new DimFechaRepository(dwhConnStr));
builder.Services.AddScoped<IFactOpinionRepository>(_ => new FactOpinionRepository(dwhConnStr));

// Registrar Servicio de Orquestación ETL
builder.Services.AddScoped<IEtlService, EtlService>();

// Registrar Worker Service
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
