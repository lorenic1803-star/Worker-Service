using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AnalisisOpiniones.Data.Persistence.Csv.Readers;

public abstract class BaseCsvReader<T> where T : class
{
    protected readonly string _filePath;
    protected readonly CsvConfiguration _csvConfig;

    protected BaseCsvReader(string filePath)
    {
        _filePath = filePath;
        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
            PrepareHeaderForMatch = args => args.Header.ToLower()
        };
    }

    protected async Task<List<T>> ReadCsvAsync()
    {
        if (string.IsNullOrWhiteSpace(_filePath) || !File.Exists(_filePath))
        {
            return new List<T>();
        }

        var records = new List<T>();

        using (var reader = new StreamReader(_filePath))
        using (var csv = new CsvReader(reader, _csvConfig))
        {
            await foreach (var record in csv.GetRecordsAsync<T>())
            {
                records.Add(record);
            }
        }

        return records;
    }
}