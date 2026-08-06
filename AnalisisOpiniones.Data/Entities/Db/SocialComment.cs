namespace AnalisisOpiniones.Data.Entities.Db;

/// <summary>
/// Representa un comentario de red social en la base de datos operacional.
/// </summary>
public class SocialComment
{
    /// <summary>
    /// Identificador de la opinión base (clave primaria y foránea).
    /// </summary>
    public int IdOpinion { get; set; }
}