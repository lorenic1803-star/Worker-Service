using AnalisisOpiniones.Data.Entities.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces.Repositories.Db;

/// <summary>
/// Interfaz para el repositorio de clientes en la base de datos operacional.
/// </summary>
public interface IClienteDbRepository
{
    /// <summary>
    /// Obtiene todos los clientes de la base de datos.
    /// </summary>
    Task<IEnumerable<Cliente>> GetAllAsync();

    /// <summary>
    /// Obtiene un cliente por su ID.
    /// </summary>
    Task<Cliente?> GetByIdAsync(int idCliente);

    /// <summary>
    /// Verifica si existe un cliente con el ID especificado.
    /// </summary>
    Task<bool> ExistsAsync(int idCliente);

    /// <summary>
    /// Verifica si existe un cliente con el email especificado.
    /// </summary>
    Task<bool> EmailExistsAsync(string email);
}