using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces
{
    public interface IFileReaderRepository<TClass> where TClass : class
    {
        Task<IEnumerable<TClass>> ReadFileAsync(string filepath);
    }
}