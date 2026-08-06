using AnalisisOpiniones.Data.Services;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces;

public interface IEtlService
{
    Task<EtlResult> ExecuteAsync();
}