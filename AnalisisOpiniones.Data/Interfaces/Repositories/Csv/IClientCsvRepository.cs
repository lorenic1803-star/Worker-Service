using AnalisisOpiniones.Data.Entities.Csv;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Csv;

/// <summary>
/// Interfaz para el repositorio de lectura de clientes desde CSV.
/// </summary>
public interface IClientCsvRepository : IFileReaderRepository<ClientCsvRecord>
{
}