using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Csv.Readers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Csv.Readers;

/// <summary>
/// Lector de encuestas desde archivo CSV.
/// </summary>
public class SurveyCsvReader : BaseCsvReader<SurveyCsvRecord>, IFileReaderRepository<SurveyCsvRecord>
{
    public SurveyCsvReader(string filePath) : base(filePath) { }

    /// <inheritdoc />
    public async Task<IEnumerable<SurveyCsvRecord>> ReadFileAsync(string filepath)
    {
        var reader = new SurveyCsvReader(filepath);
        return await reader.ReadCsvAsync();
    }
}