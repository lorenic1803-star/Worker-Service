using AnalisisOpiniones.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;

/// <summary>
/// Contrato para el repositorio de carga masiva y limpieza de hechos en el Data Warehouse.
/// </summary>
public interface IFactOpinionesRepository
{
    /// <summary>
    /// Limpia / trunca la tabla de hechos antes de iniciar el proceso de carga.
    /// </summary>
    Task ClearAsync();

    /// <summary>
    /// Carga masiva de hechos mediante TVP y Stored Procedure.
    /// </summary>
    Task<bool> Load(IEnumerable<FactOpinionesDto> factOpiniones);
}
