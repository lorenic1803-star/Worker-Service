using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Csv.Readers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Persistence.Csv.Readers;

/// <summary>
/// Lector de clientes desde archivo CSV.
/// </summary>
public class ClientCsvReader : BaseCsvReader<ClientCsvRecord>, IFileReaderRepository<ClientCsvRecord>
{
    public ClientCsvReader(string filePath) : base(filePath) { }

    /// <inheritdoc />
    public async Task<IEnumerable<ClientCsvRecord>> ReadFileAsync(string filepath)
    {
        var reader = new ClientCsvReader(filepath);
        return await reader.ReadCsvAsync();
    }
}