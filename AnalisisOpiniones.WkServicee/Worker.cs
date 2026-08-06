using AnalisisOpiniones.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnalisisOpiniones.WkServicee;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<Worker> _logger;
    private readonly TimeSpan _executionInterval = TimeSpan.FromMinutes(15);

    public Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio Worker ETL iniciado a las: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Iniciando ejecución programada de ETL...");

            using (var scope = _scopeFactory.CreateScope())
            {
                var etlService = scope.ServiceProvider.GetRequiredService<IEtlService>();
                var result = await etlService.ExecuteAsync();

                if (result.Success)
                {
                    _logger.LogInformation("ETL finalizado con ÉXITO: {Message}. Total procesados: {Count}",
                        result.Message, result.ProcessedCount);
                }
                else
                {
                    _logger.LogError("ETL finalizado con ERRORES: {Message}. Errores registrados: {ErrorCount}",
                        result.Message, result.ErrorCount);
                }
            }

            _logger.LogInformation("Próxima ejecución de ETL programada en {Interval} minutos.", _executionInterval.TotalMinutes);
            await Task.Delay(_executionInterval, stoppingToken);
        }
    }
}
