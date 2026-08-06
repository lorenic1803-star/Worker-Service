using AnalisisOpiniones.Data.Entities.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Api;

/// <summary>
/// Interfaz para el repositorio de opiniones detalladas en la API.
/// </summary>
public interface IOpinionDetalladaApiRepository
{
    /// <summary>
    /// Obtiene todas las opiniones detalladas.
    /// </summary>
    Task<IEnumerable<OpinionDetalladaDto>> GetAllAsync();

    /// <summary>
    /// Obtiene opiniones detalladas por ID de producto.
    /// </summary>
    Task<IEnumerable<OpinionDetalladaDto>> GetByProductoAsync(int idProducto);

    /// <summary>
    /// Obtiene opiniones detalladas por ID de cliente.
    /// </summary>
    Task<IEnumerable<OpinionDetalladaDto>> GetByClienteAsync(int idCliente);

    /// <summary>
    /// Obtiene opiniones detalladas por rango de fechas.
    /// </summary>
    Task<IEnumerable<OpinionDetalladaDto>> GetByFechaRangeAsync(DateTime fechaInicio, DateTime fechaFin);
}