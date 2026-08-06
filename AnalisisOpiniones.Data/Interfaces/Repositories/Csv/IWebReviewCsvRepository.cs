using AnalisisOpiniones.Data.Entities.Csv;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Csv;

/// <summary>
/// Interfaz para el repositorio de lectura de reseñas web desde CSV.
/// </summary>
public interface IWebReviewCsvRepository : IFileReaderRepository<WebReviewCsvRecord>
{
}