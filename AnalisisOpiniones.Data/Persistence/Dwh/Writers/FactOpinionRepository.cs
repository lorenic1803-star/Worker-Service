using AnalisisOpiniones.Data.Interfaces.Repositories.Dwh;
using Microsoft.Extensions.Logging;

namespace AnalisisOpiniones.Data.Persistence.Dwh.Writers;

/// <summary>
/// Mantiene compatibilidad con el nombre singular FactOpinionRepository delegando en FactOpinionesRepository.
/// </summary>
public class FactOpinionRepository : FactOpinionesRepository, IFactOpinionRepository
{
    public FactOpinionRepository(string connectionString, ILogger<FactOpinionesRepository>? logger = null)
        : base(connectionString, logger)
    {
    }
}