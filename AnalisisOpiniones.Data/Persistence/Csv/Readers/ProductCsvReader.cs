using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Csv.Readers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Csv.Readers;

/// <summary>
/// Lector de productos desde archivo CSV.
/// </summary>
public class ProductCsvReader : BaseCsvReader<ProductCsvRecord>, IFileReaderRepository<ProductCsvRecord>
{
    public ProductCsvReader(string filePath) : base(filePath) { }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductCsvRecord>> ReadFileAsync(string filepath)
    {
        var reader = new ProductCsvReader(filepath);
        return await reader.ReadCsvAsync();
    }
}