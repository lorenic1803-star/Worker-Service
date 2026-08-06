namespace AnalisisOpiniones.Data.Entities.Db;

/// <summary>
/// Representa una categoría en la base de datos operacional.
/// </summary>
public class Categoria
{
    /// <summary>
    /// Identificador único de la categoría.
    /// </summary>
    public int IdCategoria { get; set; }

    /// <summary>
    /// Nombre de la categoría.
    /// </summary>
    public string NombreCategoria { get; set; } = string.Empty;
}