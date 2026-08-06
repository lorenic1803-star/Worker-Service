using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Csv.Readers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Csv.Readers;

/// <summary>
/// Lector de comentarios sociales desde archivo CSV.
/// </summary>
public class SocialCommentCsvReader : BaseCsvReader<SocialCommentCsvRecord>, IFileReaderRepository<SocialCommentCsvRecord>
{
    public SocialCommentCsvReader(string filePath) : base(filePath) { }

    /// <inheritdoc />
    public async Task<IEnumerable<SocialCommentCsvRecord>> ReadFileAsync(string filepath)
    {
        var reader = new SocialCommentCsvReader(filepath);
        return await reader.ReadCsvAsync();
    }
}