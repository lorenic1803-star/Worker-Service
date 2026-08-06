using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Interfaces.Repositories.Csv;
using AnalisisOpiniones.Data.Persistence.Csv.Readers;
using Microsoft.Extensions.Configuration;

namespace AnalisisOpiniones.Data.Factories;

public class FileReaderFactory : IFileReaderFactory
{
    private readonly IConfiguration _configuration;

    public FileReaderFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IFileReaderRepository<T> CreateReader<T>() where T : class
    {
        var typeName = typeof(T).Name;

        object reader = typeName switch
        {
            nameof(ClientCsvRecord) => new ClientCsvReader(GetCsvPath("Clients")),
            nameof(ProductCsvRecord) => new ProductCsvReader(GetCsvPath("Products")),
            nameof(FuenteDatosCsvRecord) => new FuenteDatosCsvReader(GetCsvPath("FuenteDatos")),
            nameof(SocialCommentCsvRecord) => new SocialCommentCsvReader(GetCsvPath("SocialComments")),
            nameof(SurveyCsvRecord) => new SurveyCsvReader(GetCsvPath("Surveys")),
            nameof(WebReviewCsvRecord) => new WebReviewCsvReader(GetCsvPath("WebReviews")),
            _ => throw new NotSupportedException($"No hay lector CSV configurado para el tipo {typeName}")
        };

        return (reader as IFileReaderRepository<T>)!;
    }

    public IFileReaderRepository<T> CreateReader<T>(string filePath) where T : class
    {
        var typeName = typeof(T).Name;

        object reader = typeName switch
        {
            nameof(ClientCsvRecord) => new ClientCsvReader(filePath),
            nameof(ProductCsvRecord) => new ProductCsvReader(filePath),
            nameof(FuenteDatosCsvRecord) => new FuenteDatosCsvReader(filePath),
            nameof(SocialCommentCsvRecord) => new SocialCommentCsvReader(filePath),
            nameof(SurveyCsvRecord) => new SurveyCsvReader(filePath),
            nameof(WebReviewCsvRecord) => new WebReviewCsvReader(filePath),
            _ => throw new NotSupportedException($"No hay lector CSV configurado para el tipo {typeName}")
        };

        return (reader as IFileReaderRepository<T>)!;
    }

    private string GetCsvPath(string key)
    {
        return _configuration[$"CsvPaths:{key}"] ?? string.Empty;
    }
}