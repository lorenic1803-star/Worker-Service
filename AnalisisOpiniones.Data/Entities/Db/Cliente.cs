namespace AnalisisOpiniones.Data.Entities.Db;

/// <summary>
/// Representa un cliente en la base de datos operacional.
/// </summary>
public class Cliente
{
    /// <summary>
    /// Identificador único del cliente.
    /// </summary>
    public int IdCliente { get; set; }

    /// <summary>
    /// Nombre del cliente.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del cliente.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}