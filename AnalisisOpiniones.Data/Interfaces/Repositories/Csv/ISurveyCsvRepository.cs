using AnalisisOpiniones.Data.Entities.Csv;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Csv;

/// <summary>
/// Interfaz para el repositorio de lectura de encuestas desde CSV.
/// </summary>
public interface ISurveyCsvRepository : IFileReaderRepository<SurveyCsvRecord>
{
}