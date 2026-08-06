using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Csv.Readers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Csv.Readers;

/// <summary>
/// Lector de reseñas web desde archivo CSV.
/// </summary>
public class WebReviewCsvReader : BaseCsvReader<WebReviewCsvRecord>, IFileReaderRepository<WebReviewCsvRecord>
{
    public WebReviewCsvReader(string filePath) : base(filePath) { }

    /// <inheritdoc />
    public async Task<IEnumerable<WebReviewCsvRecord>> ReadFileAsync(string filepath)
    {
        var reader = new WebReviewCsvReader(filepath);
        return await reader.ReadCsvAsync();
    }
}