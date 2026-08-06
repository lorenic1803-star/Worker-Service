using AnalisisOpiniones.Data.Factories;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Interfaces.Repositories.Api;
using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using AnalisisOpiniones.Data.Persistence.Api.Readers;
using AnalisisOpiniones.Data.Persistence.Dwh.Writers;
using AnalisisOpiniones.Data.Services;

var builder = WebApplication.CreateBuilder(args);

var transDbConn = builder.Configuration.GetConnectionString("TransactionalDb") 
                 ?? builder.Configuration.GetConnectionString("DefaultConnection")
                 ?? @"Server=.\SQLEXPRESS;Database=OpinionesClientes;Trusted_Connection=True;TrustServerCertificate=True;";

var analyticalDbConn = builder.Configuration.GetConnectionString("AnalyticalDb") 
                    ?? builder.Configuration.GetConnectionString("DwhConnection")
                    ?? @"Server=.\SQLEXPRESS;Database=DW_Opiniones;Trusted_Connection=True;TrustServerCertificate=True;";

builder.Services.AddHttpClient("EtlApiClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddSingleton<IFileReaderFactory, FileReaderFactory>();

builder.Services.AddScoped<IDimClienteRepository>(_ => new DimClienteRepository(analyticalDbConn));
builder.Services.AddScoped<IDimProductoRepository>(_ => new DimProductoRepository(analyticalDbConn));
builder.Services.AddScoped<IDimFuenteRepository>(_ => new DimFuenteRepository(analyticalDbConn));
builder.Services.AddScoped<IDimClasificacionRepository>(_ => new DimClasificacionRepository(analyticalDbConn));
builder.Services.AddScoped<IDimFechaRepository>(_ => new DimFechaRepository(analyticalDbConn));
builder.Services.AddScoped<IFactOpinionRepository>(_ => new FactOpinionRepository(analyticalDbConn));

builder.Services.AddScoped<IOpinionDetalladaApiRepository>(_ => new OpinionDetalladaApiRepository(transDbConn));
builder.Services.AddScoped<IResumenSatisfaccionProductoApiRepository>(_ => new ResumenSatisfaccionProductoApiRepository(transDbConn));
builder.Services.AddScoped<IClasificacionOpinionesPorTipoApiRepository>(_ => new ClasificacionOpinionesPorTipoApiRepository(transDbConn));
builder.Services.AddScoped<ITendenciaSatisfaccionTiempoApiRepository>(_ => new TendenciaSatisfaccionTiempoApiRepository(transDbConn));

builder.Services.AddScoped<IEtlService, EtlService>();
builder.Services.AddScoped<EtlOrchestratorService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API ETL v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();
