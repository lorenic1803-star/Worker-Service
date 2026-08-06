using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;

/// <summary>
/// Interfaz para el repositorio de la dimensión Cliente en el DWH.
/// </summary>
public interface IDimClienteRepository
{
    /// <summary>
    /// Inserta o actualiza un cliente en la dimensión.
    /// </summary>
    Task UpsertAsync(DimCliente cliente);

    /// <summary>
    /// Inserta múltiples clientes en lote.
    /// </summary>
    Task BulkInsertAsync(IEnumerable<DimCliente> clientes);

    /// <summary>
    /// Obtiene todos los clientes de la dimensión.
    /// </summary>
    Task<IEnumerable<DimCliente>> GetAllAsync();

    /// <summary>
    /// Obtiene un cliente por su ID.
    /// </summary>
    Task<DimCliente?> GetByIdAsync(int idCliente);
}