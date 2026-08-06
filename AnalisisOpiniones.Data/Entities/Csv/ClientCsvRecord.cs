namespace AnalisisOpiniones.Data.Entities.Csv;

/// <summary>
/// Representa un registro de cliente proveniente del archivo CSV.
/// </summary>
public class ClientCsvRecord
{
    /// <summary>
    /// Identificador único del cliente.
    /// </summary>
    public string? IdCliente { get; set; }

    /// <summary>
    /// Nombre del cliente.
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Correo electrónico del cliente.
    /// </summary>
    public string? Email { get; set; }
}