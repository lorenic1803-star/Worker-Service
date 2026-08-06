using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Csv.Readers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Csv.Readers;

/// <summary>
/// Lector de fuentes de datos desde archivo CSV.
/// </summary>
public class FuenteDatosCsvReader : BaseCsvReader<FuenteDatosCsvRecord>, IFileReaderRepository<FuenteDatosCsvRecord>
{
    public FuenteDatosCsvReader(string filePath) : base(filePath) { }

    /// <inheritdoc />
    public async Task<IEnumerable<FuenteDatosCsvRecord>> ReadFileAsync(string filepath)
    {
        var reader = new FuenteDatosCsvReader(filepath);
        return await reader.ReadCsvAsync();
    }
}