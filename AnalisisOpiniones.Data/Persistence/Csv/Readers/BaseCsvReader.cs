using System.Globalization;
using System.IO;
using System.Text;
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
            PrepareHeaderForMatch = args =>
            {
                if (string.IsNullOrWhiteSpace(args.Header)) return string.Empty;
                var h = args.Header.ToLowerInvariant();
                return h
                    .Replace("á", "a")
                    .Replace("é", "e")
                    .Replace("í", "i")
                    .Replace("ó", "o")
                    .Replace("ú", "u")
                    .Replace("ñ", "n");
            },
            Encoding = Encoding.GetEncoding("iso-8859-1")
        };
    }

    protected async Task<List<T>> ReadCsvAsync()
    {
        if (string.IsNullOrWhiteSpace(_filePath) || !File.Exists(_filePath))
        {
            return new List<T>();
        }

        var records = new List<T>();

        using (var reader = new StreamReader(_filePath, Encoding.GetEncoding("iso-8859-1")))
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